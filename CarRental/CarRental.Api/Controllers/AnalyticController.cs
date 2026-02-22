using CarRental.Application.Dtos.Cars;
using CarRental.Application.Dtos.Customers;
using CarRental.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Api.Controllers;

/// <summary>
/// Controller for analytic queries and reports in the car rental system.
/// Provides endpoints for generating various business intelligence reports and data analytics.
/// </summary>
[ApiController]
[Produces("application/json")]
[Route("api/[controller]")]
public class AnalyticController(IAnalyticQueryService analyticQueryService) : ControllerBase
{
    /// <summary>
    /// Retrieves the top 5 most rented cars.
    /// </summary>
    /// <returns>Collection of top 5 cars by rental count</returns>
    /// <response code="200">Returns list of cars</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpGet("cars/top5-most-rented")]
    [ProducesResponseType(typeof(IEnumerable<CarResponseDto>), 200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<IEnumerable<CarResponseDto>>> GetTop5MostRentedCars()
    {
        try
        {
            var result = await analyticQueryService.GetTop5MostRentedCarsAsync();
            return Ok(result);
        }
        catch (InvalidOperationException)
        {
            return StatusCode(500);
        }
    }

    /// <summary>
    /// Retrieves the number of rentals for each car.
    /// </summary>
    /// <returns>Dictionary with license plate as key and rental count as value</returns>
    /// <response code="200">Returns rental counts per car</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpGet("cars/rental-counts")]
    [ProducesResponseType(typeof(Dictionary<string, int>), 200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<Dictionary<string, int>>> GetRentalCountForEachCar()
    {
        try
        {
            var result = await analyticQueryService.GetRentalCountForEachCarAsync();
            return Ok(result);
        }
        catch (InvalidOperationException)
        {
            return StatusCode(500);
        }
    }

    /// <summary>
    /// Retrieves the top 5 customers by total rental cost.
    /// </summary>
    /// <returns>Collection of top 5 customers by total spent</returns>
    /// <response code="200">Returns list of customers</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpGet("customers/top5-by-total-cost")]
    [ProducesResponseType(typeof(IEnumerable<CustomerResponseDto>), 200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<IEnumerable<CustomerResponseDto>>> GetTop5CustomersByTotalCost()
    {
        try
        {
            var result = await analyticQueryService.GetTop5CustomersByTotalCostAsync();
            return Ok(result);
        }
        catch (InvalidOperationException)
        {
            return StatusCode(500);
        }
    }

    /// <summary>
    /// Retrieves all customers who rented cars of a specific model.
    /// </summary>
    /// <param name="modelId">ID of the car model</param>
    /// <returns>Collection of customers who rented the specified model</returns>
    /// <response code="200">Returns list of customers</response>
    /// <response code="400">If model ID is invalid</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpGet("models/{modelId}/customers")]
    [ProducesResponseType(typeof(IEnumerable<CustomerResponseDto>), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<IEnumerable<CustomerResponseDto>>> GetCustomersByModel([FromRoute] int modelId)
    {
        try
        {
            if (modelId <= 0)
                return BadRequest(new { error = "Model ID must be positive" });

            var result = await analyticQueryService.GetCustomersByModelAsync(modelId);
            return Ok(result);
        }
        catch (InvalidOperationException)
        {
            return StatusCode(500);
        }
    }

    /// <summary>
    /// Retrieves cars that are currently rented at the specified moment.
    /// </summary>
    /// <param name="now">Current date and time (optional, defaults to server time)</param>
    /// <returns>Collection of currently rented cars</returns>
    /// <response code="200">Returns list of cars</response>
    /// <response code="400">If the provided time is invalid</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpGet("cars/currently-rented")]
    [ProducesResponseType(typeof(IEnumerable<CarResponseDto>), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<IEnumerable<CarResponseDto>>> GetCurrentlyRentedCars([FromQuery] DateTime? now = null)
    {
        try
        {
            var currentTime = now ?? DateTime.UtcNow;
            var result = await analyticQueryService.GetCurrentlyRentedCarsAsync(currentTime);
            return Ok(result);
        }
        catch (InvalidOperationException)
        {
            return StatusCode(500);
        }
    }
}