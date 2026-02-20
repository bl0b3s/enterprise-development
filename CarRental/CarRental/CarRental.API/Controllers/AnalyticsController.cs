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
    IRepository<Client> clientsRepo,
    IRepository<ModelGeneration> generationsRepo,
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
            .Where(r => r.Car!.ModelGeneration!.Model!.Name == modelName)
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
        var rentedCarIds = await rentalsRepo.GetQueryable()
            .Where(r => r.RentalDate.AddHours(r.RentalHours) > currentDate)
            .Select(r => r.CarId)
            .Distinct()
            .ToListAsync();

        var rentedCars = await carsRepo.GetQueryable()
            .Where(c => rentedCarIds.Contains(c.Id))
            .Include(c => c.ModelGeneration)
                .ThenInclude(m => m!.Model)
            .ToListAsync();

        var result = rentedCars
            .Select(mapper.Map<CarGetDto>)
            .ToList();

        return Ok(result);
    }

    /// <summary>
    /// Получает топ 5 самых популярных арендованных автомобилей
    /// </summary>
    /// <returns>Список автомобилей, которые можно взять напрокат</returns>
    [HttpGet("top-5-most-rented-cars")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CarRentalCountDto>>> GetTop5MostRentedCars()
    {
        var topCarStats = await rentalsRepo.GetQueryable()
            .GroupBy(r => r.CarId)
            .Select(g => new { CarId = g.Key, RentalCount = g.Count() })
            .OrderByDescending(x => x.RentalCount)
            .Take(5)
            .ToListAsync();

        var topCarIds = topCarStats.Select(x => x.CarId).ToList();

        var cars = await carsRepo.GetQueryable()
            .Where(c => topCarIds.Contains(c.Id))
            .Include(c => c.ModelGeneration)
                .ThenInclude(m => m!.Model)
            .ToListAsync();

        var carsDict = cars.ToDictionary(c => c.Id);

        var topCarsResult = topCarStats
            .Where(x => carsDict.ContainsKey(x.CarId))
            .Select(x => new CarRentalCountDto(
                mapper.Map<CarGetDto>(carsDict[x.CarId]),
                x.RentalCount))
            .ToList();

        return Ok(topCarsResult);
    }

    /// <summary>
    /// Получает количество арендованных автомобилей для каждого автомобиля
    /// </summary>
    /// <returns>Список всех автомобилей, которые были взяты в аренду</returns>
    [HttpGet("rental-count-per-car")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CarRentalCountDto>>> GetRentalCountPerCar()
    {
        var rentalCounts = await rentalsRepo.GetQueryable()
            .GroupBy(r => r.CarId)
            .Select(g => new { CarId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.CarId, x => x.Count);

        var cars = await carsRepo.GetQueryable()
            .Include(c => c.ModelGeneration)
                .ThenInclude(m => m!.Model)
            .ToListAsync();

        var carsWithRentalCount = cars
            .Select(car => new CarRentalCountDto(
                mapper.Map<CarGetDto>(car),
                rentalCounts.GetValueOrDefault(car.Id, 0)))
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
        var rentals = await rentalsRepo.GetQueryable()
            .Select(r => new { r.ClientId, r.CarId, r.RentalHours })
            .ToListAsync();

        var cars = await carsRepo.GetQueryable()
            .Select(c => new { c.Id, c.ModelGenerationId })
            .ToListAsync();

        var generations = await generationsRepo.GetQueryable()
            .Select(g => new { g.Id, g.RentalPricePerHour })
            .ToListAsync();

        var carPrices = cars.Join(generations,
            c => c.ModelGenerationId,
            g => g.Id,
            (c, g) => new { CarId = c.Id, Price = g.RentalPricePerHour })
            .ToDictionary(x => x.CarId, x => x.Price);

        var topClientStats = rentals
            .GroupBy(r => r.ClientId)
            .Select(g => new
            {
                ClientId = g.Key,
                TotalAmount = g.Sum(r => r.RentalHours * carPrices.GetValueOrDefault(r.CarId, 0))
            })
            .OrderByDescending(x => x.TotalAmount)
            .Take(5)
            .ToList();

        var topClientIds = topClientStats.Select(x => x.ClientId).ToList();

        var clients = await clientsRepo.GetQueryable()
            .Where(c => topClientIds.Contains(c.Id))
            .ToListAsync();

        var clientsDict = clients.ToDictionary(c => c.Id);

        var result = topClientStats
            .Where(x => clientsDict.ContainsKey(x.ClientId))
            .Select(x => new ClientRentalAmountDto(
                mapper.Map<ClientGetDto>(clientsDict[x.ClientId]),
                x.TotalAmount))
            .ToList();

        return Ok(result);
    }
}