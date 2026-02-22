using CarRental.Application.Contracts.Dto;
using Confluent.Kafka;

namespace CarRental.Generator.Kafka.Host;

/// <summary>
/// Kafka producer для отправки пачек DTO записей об аренде в указанный топик
/// </summary>
/// <param name="configuration">Конфигурация для чтения Kafka настроек</param>
/// <param name="producer">Kafka producer</param>
/// <param name="logger">Логгер</param>
public class RentalKafkaProducer(
    IConfiguration configuration,
    IProducer<Guid, IList<RentalEditDto>> producer,
    ILogger<RentalKafkaProducer> logger)
{
    private readonly string _topic = configuration.GetSection("Kafka")["RentalTopicName"] ?? throw new KeyNotFoundException("RentalTopicName section of Kafka is missing");

    /// <summary>
    /// Отправить пачку DTO записей об аренде в Kafka
    /// </summary>
    /// <param name="batch">Пачка DTO для отправки</param>
    /// <param name="cancellationToken">Токен отмены</param>
    public async Task SendAsync(IList<RentalEditDto> batch, CancellationToken cancellationToken = default)
    {
        if (batch is null || batch.Count == 0)
        {
            logger.LogWarning("Skipping send because batch is empty");
            return;
        }

        var key = Guid.NewGuid();

        try
        {
            logger.LogInformation("Sending a batch of {count} contracts to {topic} key={key}", batch.Count, _topic, key);

            var message = new Message<Guid, IList<RentalEditDto>>
            {
                Key = key,
                Value = batch
            };

            var delivery = await producer.ProduceAsync(_topic, message, cancellationToken);

            logger.LogInformation(
                "Batch sent to {topic} partition={partition} offset={offset} key={key} count={count}",
                delivery.Topic,
                delivery.Partition.Value,
                delivery.Offset.Value,
                key,
                batch.Count);
        }
        catch (ProduceException<Guid, IList<RentalEditDto>> ex)
        {
            logger.LogError(ex, "Kafka produce failed topic={topic} reason={reason} key={key} count={count}", _topic, ex.Error.Reason, key, batch.Count);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception occurred during sending a batch of {count} contracts to {topic} key={key}", batch.Count, _topic, key);
        }
    }
}