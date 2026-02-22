using Bogus;
using CarRental.Application.Dtos.Grpc;
using CarRental.Producer.Configurations;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Options;

namespace CarRental.Producer.Services;

/// <summary>
/// Service for generating fake car rentals and sending them via gRPC streaming.
/// </summary>
public class RequestGeneratorService(
    ILogger<RequestGeneratorService> logger,
    RentalStreaming.RentalStreamingClient client,
    IOptions<GeneratorOptions> options)
{
    private readonly GeneratorOptions _options = options.Value;

    /// <summary>
    /// Starts automatic generation of rental requests.
    /// Uses settings from configuration for batch size and timing.
    /// </summary>
    public async Task GenerateAutomatically(CancellationToken stoppingToken = default)
    {
        logger.LogInformation(
            "Starting automatic rental generation: batchSize={BatchSize}, limit={Limit}, wait={Wait}s",
            _options.BatchSize, _options.PayloadLimit, _options.WaitTime);

        var counter = 0;

        while (counter < _options.PayloadLimit && !stoppingToken.IsCancellationRequested)
        {
            var success = await GenerateAndSendRentals(_options.BatchSize, stoppingToken);

            if (success)
            {
                counter += _options.BatchSize;
                logger.LogDebug("Sent batch of {BatchSize} rentals. Total: {Total}", _options.BatchSize, counter);
            }

            await Task.Delay(_options.WaitTime * 1000, stoppingToken);
        }

        logger.LogInformation("Automatic generation finished. Total sent: {Total}", counter);
    }

    /// <summary>
    /// Generates and sends a batch of rental requests via gRPC streaming.
    /// Retries up to configured number of times if sending fails.
    /// </summary>
    private async Task<bool> GenerateAndSendRentals(int count, CancellationToken stoppingToken = default)
    {
        var retryCount = 0;

        while (retryCount < _options.MaxRetries && !stoppingToken.IsCancellationRequested)
        {
            try
            {
                var faker = new Faker();

                using var call = client.StreamRentals(
                    deadline: DateTime.UtcNow.AddSeconds(_options.GrpcTimeoutSeconds),
                    cancellationToken: stoppingToken);

                for (var i = 0; i < count; i++)
                {
                    var request = new RentalRequestMessage
                    {
                        CustomerId = faker.Random.Int(
                            _options.Data.CustomerIdRange.Min,
                            _options.Data.CustomerIdRange.Max),

                        CarId = faker.Random.Int(
                            _options.Data.CarIdRange.Min,
                            _options.Data.CarIdRange.Max),

                        PickupDateTime = Timestamp.FromDateTime(
                            faker.Date.Between(
                                DateTime.UtcNow.AddDays(-30),
                                DateTime.UtcNow.AddDays(90))),

                        Hours = faker.Random.Int(2, 336),
                    };

                    await call.RequestStream.WriteAsync(request, stoppingToken);

                    logger.LogDebug(
                        "Sent rental {Index} with customer {CustomerId}, car {CarId}, hours {Hours}",
                        i + 1, request.CustomerId, request.CarId, request.Hours);
                }

                await call.RequestStream.CompleteAsync();

                var responses = new List<RentalResponseMessage>();

                await foreach (var response in call.ResponseStream.ReadAllAsync(stoppingToken))
                {
                    responses.Add(response);
                    logger.LogDebug("Received response: {Success} - {Message}", response.Success, response.Message);
                }

                logger.LogInformation("Successfully completed batch with {ResponseCount} responses", responses.Count);

                return true;
            }
            catch (Exception ex)
            {
                retryCount++;

                logger.LogWarning(ex,
                    "Failed to send batch (attempt {RetryCount}/{MaxRetries})",
                    retryCount, _options.MaxRetries);

                if (retryCount < _options.MaxRetries)
                {
                    await Task.Delay(TimeSpan.FromSeconds(_options.RetryDelaySeconds), stoppingToken);
                }
                else
                {
                    logger.LogError(ex,
                        "Failed to send batch after {MaxRetries} attempts",
                        _options.MaxRetries);

                    return false;
                }
            }
        }

        return false;
    }
}