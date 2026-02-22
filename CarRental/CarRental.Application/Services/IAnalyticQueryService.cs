using CarRental.Application.Dtos.Cars;
using CarRental.Application.Dtos.Customers;

namespace CarRental.Application.Services;
/// <summary>
/// Service for complex car rental analytic queries and reports.
/// </summary>
public interface IAnalyticQueryService
{
    /// <summary>
    /// Returns the top 5 most rented cars (by number of rentals).
    /// Sorted by count descending, then by license plate ascending.
    /// </summary>
    Task<IEnumerable<CarResponseDto>> GetTop5MostRentedCarsAsync();

    /// <summary>
    /// Returns number of rentals for each car.
    /// Result is sorted by license plate.
    /// </summary>
    Task<Dictionary<string, int>> GetRentalCountForEachCarAsync();

    /// <summary>
    /// Returns top 5 customers by total rental cost.
    /// Sorted by total cost descending, then by full name ascending.
    /// </summary>
    Task<IEnumerable<CustomerResponseDto>> GetTop5CustomersByTotalCostAsync();

    /// <summary>
    /// Returns all customers who ever rented cars of the specified model.
    /// Sorted by full name.
    /// </summary>
    Task<IEnumerable<CustomerResponseDto>> GetCustomersByModelAsync(int modelId);

    /// <summary>
    /// Returns cars that are currently rented at the given moment.
    /// </summary>
    Task<IEnumerable<CarResponseDto>> GetCurrentlyRentedCarsAsync(DateTime now);
}
