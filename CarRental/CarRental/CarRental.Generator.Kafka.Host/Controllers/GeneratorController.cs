using CarRental.Application.Contracts.Dto;
using CarRental.Generator.Kafka.Host.Generator;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Generator.Kafka.Host.Controllers;

/// <summary>
/// Контроллер генерации тестовых данных и отправки их в Kafka
/// </summary>
/// <param name="producer">Kafka producer записей об аренде</param>
/// <param name="logger">Логгер</param>
[ApiController]
[Route("api/[controller]")]
public class GeneratorController(
    RentalKafkaProducer producer,
    ILogger<GeneratorController> logger) : ControllerBase
{
    /// <summary>
    /// Генерация записей об аренде и отправка в Kafka батчами с задержкой
    /// </summary>
    /// <param name="listSize">Общий размер списка генерируемых DTO</param>
    /// <param name="batchSize">Размер каждого батча</param>
    /// <param name="delayMs">Задержка между отправками батчей в миллисекундах</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список DTO для создания или обновления записей об аренде</returns>
    [HttpPost("rentals")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<IList<RentalEditDto>>> GenerateRentals(
        [FromQuery] int listSize,
        [FromQuery] int batchSize,
        [FromQuery] int delayMs,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("{method} called with listSize={listSize} batchSize={batchSize} delayMs={delayMs}", nameof(GenerateRentals), listSize, batchSize, delayMs);

        if (listSize <= 0 || listSize > 10000)
            return BadRequest("listSize must be between 1 and 10000");

        if (batchSize <= 0 || listSize > 10000)
            return BadRequest("batchSize must be between 1 and 10000");

        try
        {
            var items = RentalGenerator.Generate(listSize);

            foreach (var batch in items.Chunk(batchSize))
            {
                cancellationToken.ThrowIfCancellationRequested();

                await producer.SendAsync([.. batch], cancellationToken);

                await Task.Delay(delayMs, cancellationToken);
            }

            logger.LogInformation(
                "{method} executed successfully listSize={listSize} batchSize={batchSize} delayMs={delayMs}",
                nameof(GenerateRentals),
                listSize,
                batchSize,
                delayMs);

            return Ok(items);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            logger.LogWarning("{method} cancelled by request", nameof(GenerateRentals));
            return BadRequest("Request cancelled");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An exception happened during {method}", nameof(GenerateRentals));
            return StatusCode(500, $"{ex.Message}\n\r{ex.InnerException?.Message}");
        }
    }
}