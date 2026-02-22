using AutoMapper;
using CarRental.Application.Dtos.Cars;
using CarRental.Application.Dtos.Customers;
using CarRental.Domain.Models;
using CarRental.Infrastructure.Repositories.Interfaces;

namespace CarRental.Application.Services;

/// <summary>
/// Implementation of car rental analytic query service.
/// </summary>
public class AnalyticQueryService(
    IRepository<Car> carRepository,
    IRepository<Customer> customerRepository,
    IRepository<Rental> rentalRepository,
    IRepository<ModelGeneration> modelGenerationRepository,
    IMapper mapper
) : IAnalyticQueryService
{
    /// <inheritdoc/>
    public async Task<IEnumerable<CarResponseDto>> GetTop5MostRentedCarsAsync()
    {
        try
        {
            var rentals = await rentalRepository.GetAsync();
            var cars = await carRepository.GetAsync();

            var topCars = rentals
                .GroupBy(r => r.CarId)
                .Select(g => new
                {
                    CarId = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .ThenBy(x => cars.First(c => c.Id == x.CarId).LicensePlate)
                .Take(5)
                .Join(cars,
                    x => x.CarId,
                    c => c.Id,
                    (x, c) => c)
                .ToList();

            return mapper.Map<List<CarResponseDto>>(topCars);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to retrieve top 5 most rented cars", ex);
        }
    }

    /// <inheritdoc/>
    public async Task<Dictionary<string, int>> GetRentalCountForEachCarAsync()
    {
        try
        {
            var rentals = await rentalRepository.GetAsync();
            var cars = await carRepository.GetAsync();

            var counts = rentals
                .GroupBy(r => r.CarId)
                .Select(g => new
                {
                    Plate = cars.First(c => c.Id == g.Key).LicensePlate,
                    Count = g.Count()
                })
                .OrderBy(x => x.Plate)
                .ToDictionary(x => x.Plate, x => x.Count);

            return counts;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to retrieve rental counts for each car", ex);
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<CustomerResponseDto>> GetTop5CustomersByTotalCostAsync()
    {
        try
        {
            var rentals = await rentalRepository.GetAsync();
            var cars = await carRepository.GetAsync();
            var modelGenerations = await modelGenerationRepository.GetAsync();
            var customers = await customerRepository.GetAsync();

            var topCustomers = rentals
                .Join(cars,
                    r => r.CarId,
                    c => c.Id,
                    (r, c) => new { r.CustomerId, c.ModelGenerationId, r.Hours })
                .Join(modelGenerations,
                    x => x.ModelGenerationId,
                    mg => mg.Id,
                    (x, mg) => new
                    {
                        x.CustomerId,
                        Cost = mg.HourlyRate * x.Hours
                    })
                .GroupBy(x => x.CustomerId)
                .Select(g => new
                {
                    CustomerId = g.Key,
                    Total = g.Sum(x => x.Cost)
                })
                .OrderByDescending(x => x.Total)
                .ThenBy(x => customers.First(c => c.Id == x.CustomerId).FullName)
                .Take(5)
                .Join(customers,
                    x => x.CustomerId,
                    c => c.Id,
                    (x, c) => c)
                .ToList();

            return mapper.Map<List<CustomerResponseDto>>(topCustomers);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to retrieve top 5 customers by total cost", ex);
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<CustomerResponseDto>> GetCustomersByModelAsync(int modelId)
    {
        try
        {
            var rentals = await rentalRepository.GetAsync();
            var cars = await carRepository.GetAsync();
            var modelGenerations = await modelGenerationRepository.GetAsync();
            var customers = await customerRepository.GetAsync();

            var customerIds = rentals
                .Join(cars,
                    r => r.CarId,
                    c => c.Id,
                    (r, c) => new { r.CustomerId, c.ModelGenerationId })
                .Where(x => modelGenerations.Any(mg => mg.Id == x.ModelGenerationId && mg.CarModelId == modelId))
                .Select(x => x.CustomerId)
                .Distinct()
                .ToList();

            var customersList = customers
                .Where(c => customerIds.Contains(c.Id))
                .OrderBy(c => c.FullName)
                .ToList();

            return mapper.Map<List<CustomerResponseDto>>(customersList);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to retrieve customers for model ID {modelId}", ex);
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<CarResponseDto>> GetCurrentlyRentedCarsAsync(DateTime now)
    {
        try
        {
            var rentals = await rentalRepository.GetAsync();
            var cars = await carRepository.GetAsync();

            var currentRentals = rentals
                .Where(r => r.PickupDateTime <= now && r.PickupDateTime.AddHours(r.Hours) > now)
                .Join(cars,
                    r => r.CarId,
                    c => c.Id,
                    (r, c) => c)
                .DistinctBy(c => c.Id)
                .OrderBy(c => c.LicensePlate)
                .ToList();

            return mapper.Map<List<CarResponseDto>>(currentRentals);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to retrieve currently rented cars", ex);
        }
    }
}
