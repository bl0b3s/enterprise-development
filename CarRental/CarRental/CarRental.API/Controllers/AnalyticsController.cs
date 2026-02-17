using AutoMapper;
using CarRental.Application.Contracts.Dto;
using CarRental.Domain.Entities;
using CarRental.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Api.Controllers;

/// <summary>
/// Контроллер для аналитических запросов и отчетов
/// </summary>
[ApiController]
[Route("api/analytics")]
public class AnalyticsController(
    IRepository<Rental> rentalsRepo,
    IRepository<Car> carsRepo,
    IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Получает список клиентов, арендовавших автомобили указанной модели, отсортированный по названию
    /// </summary>
    /// <param name="modelName">Название модели автомобиля</param>
    /// <returns>Список клиентов</returns>
    [HttpGet("clients-by-model")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ClientGetDto>>> GetClientsByModelSortedByName(
        [FromQuery] string modelName)
    {
        var rentalsQuery = rentalsRepo.GetQueryable(
            include: query => query
                .Include(r => r.Car)
                    .ThenInclude(c => c.ModelGeneration)
                        .ThenInclude(mg => mg.Model)
                .Include(r => r.Client));

        var clients = await rentalsQuery
            .Where(r => r.Car.ModelGeneration.Model.Name == modelName)
            .Select(r => r.Client)
            .Distinct()
            .OrderBy(c => c.FullName)
            .ToListAsync();

        var result = clients
            .Select(mapper.Map<ClientGetDto>)
            .ToList();

        return Ok(result);
    }

    /// <summary>
    /// Получает арендованные в данный момент автомобили
    /// </summary>
    /// <param name="currentDate">Текущая дата проверки</param>
    /// <returns>Список арендованных автомобилей</returns>
    [HttpGet("currently-rented-cars")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CarGetDto>>> GetCurrentlyRentedCars(
        [FromQuery] DateTime currentDate)
    {
        var rentals = await rentalsRepo.GetAllAsync(
            include: query => query.Include(r => r.Car));

        var rentedCars = rentals
            .Where(r => r.RentalDate.AddHours(r.RentalHours) > currentDate)
            .Select(r => r.Car)
            .Distinct()
            .Select(mapper.Map<CarGetDto>)
            .ToList();

        return Ok(rentedCars);
    }

    /// <summary>
    /// Получает топ 5 самых популярных арендованных автомобилей
    /// </summary>
    /// <returns>Список автомобилей, которые можно взять напрокат</returns>
    [HttpGet("top-5-most-rented-cars")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CarRentalCountDto>>> GetTop5MostRentedCars()
    {
        var rentals = await rentalsRepo.GetAllAsync(
            include: query => query.Include(r => r.Car));

        var topCars = rentals
            .GroupBy(r => r.Car)
            .Select(g => new { Car = g.Key, RentalCount = g.Count() })
            .OrderByDescending(x => x.RentalCount)
            .Take(5)
            .Select(x => new CarRentalCountDto(
                mapper.Map<CarGetDto>(x.Car),
                x.RentalCount))
            .ToList();

        return Ok(topCars);
    }

    /// <summary>
    /// Получает количество арендованных автомобилей для каждого автомобиля
    /// </summary>
    /// <returns>Список всех автомобилей, которые были взяты в аренду</returns>
    [HttpGet("rental-count-per-car")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CarRentalCountDto>>> GetRentalCountPerCar()
    {
        var rentals = await rentalsRepo.GetAllAsync();
        var cars = await carsRepo.GetAllAsync(
            include: query => query.Include(c => c.ModelGeneration));

        var carsWithRentalCount = cars
            .Select(car => new CarRentalCountDto(
                mapper.Map<CarGetDto>(car),
                rentals.Count(r => r.CarId == car.Id)))
            .OrderByDescending(x => x.RentalCount)
            .ToList();

        return Ok(carsWithRentalCount);
    }

    /// <summary>
    /// Получает топ 5 клиентов по общей сумме аренды
    /// </summary>
    /// <returns>Список клиентов с общей суммой арендной платы</returns>
    [HttpGet("top-5-clients-by-rental-amount")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ClientRentalAmountDto>>> GetTop5ClientsByRentalAmount()
    {
        var rentals = await rentalsRepo.GetAllAsync(
            include: query => query
                .Include(r => r.Car)
                    .ThenInclude(c => c.ModelGeneration)
                .Include(r => r.Client));

        var topClients = rentals
            .Select(r => new
            {
                Client = r.Client,
                Amount = r.RentalHours * r.Car.ModelGeneration.RentalPricePerHour
            })
            .GroupBy(x => x.Client)
            .Select(g => new { Client = g.Key, TotalAmount = g.Sum(x => x.Amount) })
            .OrderByDescending(x => x.TotalAmount)
            .Take(5)
            .Select(x => new ClientRentalAmountDto(
                mapper.Map<ClientGetDto>(x.Client),
                x.TotalAmount))
            .ToList();

        return Ok(topClients);
    }
}