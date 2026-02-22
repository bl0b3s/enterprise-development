using AutoMapper;
using CarRental.Application.Contracts.Dto;
using CarRental.Domain.Entities;
using CarRental.Domain.Interfaces;
using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CarRental.Infrastructure.Kafka;

/// <summary>
/// Kafka consumer для обработки сообщений с записями об аренде машин
/// </summary>
/// <param name="consumer">Экземпляр Kafka consumer</param>
/// <param name="scopeFactory">Фабрика scope для получения scoped зависимостей</param>
/// <param name="configuration">Конфигурация для чтения Kafka настроек</param>
/// <param name="logger">Логгер</param>
public class RentalKafkaConsumer(
    IConsumer<Guid, IList<RentalEditDto>> consumer,
    ILogger<RentalKafkaConsumer> logger,
    IConfiguration configuration,
    IMapper mapper,
    IServiceScopeFactory scopeFactory) : BackgroundService
{
    private readonly string _topic = configuration.GetSection("Kafka")["RentalTopicName"] ?? throw new KeyNotFoundException("RentalTopicName section of Kafka is missing");

    /// <summary>
    /// Запуск цикла чтения Kafka сообщений и создания записей об аренде машин
    /// </summary>
    /// <param name="stoppingToken">Токен отмены</param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();

        try
        {
            consumer.Subscribe(_topic);

            logger.LogInformation("Consumer {consumer} subscribed to topic {topic}", consumer.Name, _topic);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to subscribe consumer {consumer} to topic {topic}", consumer.Name, _topic);
            return;
        }

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                ConsumeResult<Guid, IList<RentalEditDto>>? msg = null;

                try
                {
                    msg = consumer.Consume(stoppingToken);

                    var payload = msg?.Message?.Value;
                    if (payload is null || payload.Count == 0)
                        continue;

                    await ProcessMessage(payload, msg!.Message.Key, stoppingToken);

                    consumer.Commit(msg);

                    logger.LogInformation("Committed message {key} from topic {topic} via consumer {consumer}", msg.Message.Key, _topic, consumer.Name);
                }
                catch (ConsumeException ex) when (ex.Error.Code == ErrorCode.UnknownTopicOrPart)
                {
                    logger.LogWarning("Topic {topic} is not available yet, retrying...", _topic);
                    await Task.Delay(2000, stoppingToken);
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    logger.LogInformation("Operation was canceled on consumer {consumer}", consumer.Name);
                    break;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed to consume or process message from topic {topic}", _topic);
                    await Task.Delay(1000, stoppingToken);
                }
            }
        }
        finally
        {
            try
            {
                consumer.Close();
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Error during consumer close");
            }
        }
    }

    /// <summary>
    /// Обработать одно Kafka сообщение и создать запись для валидных контрактов
    /// </summary>
    /// <param name="payload">Список DTO записей об аренде</param>
    /// <param name="messageKey">Ключ сообщения Kafka</param>
    /// <param name="stoppingToken">Токен отмены</param>
    private async Task ProcessMessage(IList<RentalEditDto> payload, Guid messageKey, CancellationToken stoppingToken)
    {
        logger.LogInformation("Processing message {key} from topic {topic} with {count} contracts", messageKey, _topic, payload.Count);

        using var scope = scopeFactory.CreateScope();

        var rentalRepo = scope.ServiceProvider.GetRequiredService<IRepository<Rental>>();
        var carRepo = scope.ServiceProvider.GetRequiredService<IRepository<Car>>();
        var clientRepo = scope.ServiceProvider.GetRequiredService<IRepository<Client>>();

        var carIds = payload.Select(p => p.CarId).Distinct().ToList();
        var clientIds = payload.Select(p => p.ClientId).Distinct().ToList();

        var existingCarIds = await carRepo.GetQueryable()
            .Where(c => carIds.Contains(c.Id))
            .Select(c => c.Id)
            .ToListAsync(stoppingToken);

        var existingClientIds = await clientRepo.GetQueryable()
            .Where(c => clientIds.Contains(c.Id))
            .Select(c => c.Id)
            .ToListAsync(stoppingToken);

        var validCarIds = existingCarIds.ToHashSet();
        var validClientIds = existingClientIds.ToHashSet();

        foreach (var dto in payload)
        {
            stoppingToken.ThrowIfCancellationRequested();

            if (!validCarIds.Contains(dto.CarId) || !validClientIds.Contains(dto.ClientId))
            {
                logger.LogWarning("Skipping Rental contract from message {key} because related car or client doesn't exist, CarId={carId} ClientId={clientId}",
                    messageKey, dto.CarId, dto.ClientId);
                continue;
            }

            try
            {
                var rental = mapper.Map<Rental>(dto);

                await rentalRepo.AddAsync(rental);

                logger.LogInformation("Created rental from message {key} CarId={carId} ClientId={clientId}", messageKey, dto.CarId, dto.ClientId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to create rental from message {key} CarId={carId} ClientId={clientId}", messageKey, dto.CarId, dto.ClientId);
            }
        }
    }
}