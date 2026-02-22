using CarRental.Application.Dtos.Cars;
using CarRental.Application.Dtos.Customers;
using CarRental.Application.Dtos.Grpc;
using CarRental.Application.Dtos.Rentals;
using CarRental.Application.Services;
using Grpc.Core;

namespace CarRental.Customer.Services;

/// <summary>
/// gRPC service for streaming car rental requests.
/// </summary>
public class RequestStreamingService(
    ILogger<RequestStreamingService> logger,
    IServiceScopeFactory scopeFactory) : RentalStreaming.RentalStreamingBase
{
    public override async Task StreamRentals(
        IAsyncStreamReader<RentalRequestMessage> requestStream,
        IServerStreamWriter<RentalResponseMessage> responseStream,
        ServerCallContext context)
    {
        logger.LogInformation("Started bidirectional streaming from {Peer}", context.Peer);

        try
        {
            await foreach (var request in requestStream.ReadAllAsync(context.CancellationToken))
            {
                logger.LogInformation(
                    "Received rental request: customer {CustomerId}, car {CarId}, pickup {Pickup}, hours {Hours}",
                    request.CustomerId, request.CarId, request.PickupDateTime.ToDateTime(), request.Hours);

                var (success, rentalId, errorMessage) = await ProcessRentalRequest(request);

                await responseStream.WriteAsync(new RentalResponseMessage
                {
                    Success = success,
                    Message = success
                        ? $"Rental created successfully (ID: {rentalId}) for customer {request.CustomerId}"
                        : $"Failed to create rental: {errorMessage}",
                    RentalId = success ? rentalId : 0,
                    ErrorCode = success ? "" : "RENTAL_CREATION_FAILED"
                });

                logger.LogInformation("Sent response for customer {CustomerId} → success: {Success}",
                    request.CustomerId, success);
            }
        }
        catch (OperationCanceledException)
        {
            logger.LogWarning("Streaming cancelled by client {Peer}", context.Peer);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error in streaming");
            throw;
        }

        logger.LogInformation("Finished streaming rentals");
    }

    private async Task<(bool Success, long RentalId, string ErrorMessage)> ProcessRentalRequest(RentalRequestMessage request)
    {
        using var scope = scopeFactory.CreateScope();

        try
        {
            var rentalService = scope.ServiceProvider.GetRequiredService<ICrudService<RentalResponseDto, RentalCreateDto, RentalUpdateDto>>();
            var customerService = scope.ServiceProvider.GetRequiredService<ICrudService<CustomerResponseDto, CustomerCreateDto, CustomerUpdateDto>>();
            var carService = scope.ServiceProvider.GetRequiredService<ICrudService<CarResponseDto, CarCreateDto, CarUpdateDto>>();

            var customer = await customerService.GetAsync(request.CustomerId);
            if (customer == null)
            {
                logger.LogWarning("Customer not found: {CustomerId}", request.CustomerId);
                return (false, 0, $"Customer {request.CustomerId} not found");
            }

            var car = await carService.GetAsync(request.CarId);
            if (car == null)
            {
                logger.LogWarning("Car not found: {CarId}", request.CarId);
                return (false, 0, $"Car {request.CarId} not found");
            }

            var pickupDateTime = request.PickupDateTime.ToDateTime();

            if (pickupDateTime < DateTime.UtcNow)
            {
                logger.LogWarning("Pickup date is in the past: {PickupDateTime}", pickupDateTime);
                return (false, 0, "Pickup date cannot be in the past");
            }

            if (request.Hours <= 0)
            {
                logger.LogWarning("Invalid rental duration: {Hours}", request.Hours);
                return (false, 0, "Rental duration must be positive");
            }

            var createDto = new RentalCreateDto
            {
                CustomerId = request.CustomerId,
                CarId = request.CarId,
                PickupDateTime = pickupDateTime,
                Hours = request.Hours
            };

            var createdRental = await rentalService.CreateAsync(createDto);

            logger.LogInformation(
                "Rental created successfully: ID {RentalId}, customer {CustomerId}, car {CarId}",
                createdRental?.Id, request.CustomerId, request.CarId);

            return (true, createdRental?.Id ?? 0, "");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to process rental request for customer {CustomerId}", request.CustomerId);
            return (false, 0, ex.Message);
        }
    }
}