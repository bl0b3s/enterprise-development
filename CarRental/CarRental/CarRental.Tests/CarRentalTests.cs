using CarRental.Domain.Data;

namespace CarRental.Tests;


/// <summary>
/// Юнит-тесты для пункта проката автомобилей
/// </summary>
public class CarRentalTests(CarRentalFixture fixture) : IClassFixture<CarRentalFixture>
{
    /// <summary>
    /// ТЕСТ 1: Вывести информацию обо всех клиентах, 
    /// которые брали в аренду автомобили указанной модели, упорядочить по ФИО.
    /// </summary>
    [Fact]
    public void GetClientsByModelSortedByName()
    {
        const string targetModel = "Lada Vesta";
        const int expectedCount = 3;
        const string expectedFirstName = "Alexander Smirnov";
        const string expectedSecondName = "Denis Popov";
        const string expectedThirdName = "Igor Kozlovsky";

        var modelId = fixture.CarModels.FirstOrDefault(m => m.Name == targetModel)?.Id;

        var generationIds = fixture.ModelGenerations
            .Where(mg => mg.ModelId == modelId)
            .Select(mg => mg.Id)
            .ToList();

        var carIds = fixture.Cars
            .Where(c => generationIds.Contains(c.ModelGenerationId))
            .Select(c => c.Id)
            .ToList();

        var clientIds = fixture.Rentals
            .Where(r => carIds.Contains(r.CarId))
            .Select(r => r.ClientId)
            .Distinct()
            .ToList();

        var clients = fixture.Clients
            .Where(c => clientIds.Contains(c.Id))
            .OrderBy(c => c.FullName)
            .ToList();

        Assert.Equal(expectedCount, clients.Count);
        Assert.Equal(expectedFirstName, clients[0].FullName);
        Assert.Equal(expectedSecondName, clients[1].FullName);
        Assert.Equal(expectedThirdName, clients[2].FullName);
    }

    /// <summary>
    /// ТЕСТ 2: Вывести информацию об автомобилях, находящихся в аренде.
    /// </summary>
    [Fact]
    public void GetCurrentlyRentedCars()
    {
        var testDate = new DateTime(2024, 3, 5, 12, 0, 0);
        var expectedPlate = "K234MR163";

        var rentedCarIds = fixture.Rentals
            .Where(r => r.RentalDate.AddHours(r.RentalHours) > testDate)
            .Select(r => r.CarId)
            .Distinct()
            .ToList();

        var rentedCars = fixture.Cars
            .Where(c => rentedCarIds.Contains(c.Id))
            .ToList();

        Assert.Contains(rentedCars, c => c.LicensePlate == expectedPlate);
    }

    /// <summary>
    /// ТЕСТ 3: Вывести топ 5 наиболее часто арендуемых автомобилей.
    /// </summary>
    [Fact]
    public void GetTop5MostRentedCars()
    {
        const int expectedCount = 5;
        const string expectedTopCarPlate = "N456RS163";
        const int expectedTopCarRentalCount = 3;

        var topCarStats = fixture.Rentals
            .GroupBy(r => r.CarId)
            .Select(g => new { CarId = g.Key, RentalCount = g.Count() })
            .OrderByDescending(x => x.RentalCount)
            .Take(5)
            .ToList();

        var topCarIds = topCarStats.Select(x => x.CarId).ToList();
        var carsDict = fixture.Cars
            .Where(c => topCarIds.Contains(c.Id))
            .ToDictionary(c => c.Id);

        var topCars = topCarStats
            .Select(x => new
            {
                Car = carsDict.GetValueOrDefault(x.CarId),
                x.RentalCount
            })
            .Where(x => x.Car != null)
            .ToList();

        Assert.Equal(expectedCount, topCars.Count);
        Assert.Equal(expectedTopCarPlate, topCars[0].Car?.LicensePlate);
        Assert.Equal(expectedTopCarRentalCount, topCars[0].RentalCount);
    }

    /// <summary>
    /// ТЕСТ 4: Для каждого автомобиля вывести число аренд.
    /// </summary>
    [Fact]
    public void GetRentalCountPerCar()
    {
        const int expectedTotalCars = 15;
        const int expectedLadaVestaRentalCount = 3;
        const int expectedBmwRentalCount = 2;
        const int ladaVestaCarId = 7;
        const int bmwCarId = 1;

        var rentalCounts = fixture.Rentals
            .GroupBy(r => r.CarId)
            .ToDictionary(g => g.Key, g => g.Count());

        var carsWithRentalCount = fixture.Cars
            .Select(car => new
            {
                Car = car,
                RentalCount = rentalCounts.GetValueOrDefault(car.Id, 0)
            })
            .ToList();

        Assert.Equal(expectedTotalCars, carsWithRentalCount.Count);

        var ladaVesta = carsWithRentalCount.First(c => c.Car.Id == ladaVestaCarId);
        var bmw = carsWithRentalCount.First(c => c.Car.Id == bmwCarId);

        Assert.Equal(expectedLadaVestaRentalCount, ladaVesta.RentalCount);
        Assert.Equal(expectedBmwRentalCount, bmw.RentalCount);
        Assert.True(carsWithRentalCount.All(x => x.RentalCount >= 0));
    }

    /// <summary>
    /// ТЕСТ 5: Вывести топ 5 клиентов по сумме аренды.
    /// </summary>
    [Fact]
    public void GetTop5ClientsByRentalAmount()
    {
        const int expectedCount = 5;
        const string expectedTopClientName = "Olga Zakharova";

        var carPrices = fixture.Cars
            .Join(fixture.ModelGenerations,
                c => c.ModelGenerationId,
                g => g.Id,
                (c, g) => new { CarId = c.Id, Price = g.RentalPricePerHour })
            .ToDictionary(x => x.CarId, x => x.Price);

        var topClientStats = fixture.Rentals
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
        var clientsDict = fixture.Clients
            .Where(c => topClientIds.Contains(c.Id))
            .ToDictionary(c => c.Id);

        var topClients = topClientStats
            .Select(x => new
            {
                Client = clientsDict.GetValueOrDefault(x.ClientId),
                x.TotalAmount
            })
            .Where(x => x.Client != null)
            .ToList();

        Assert.Equal(expectedCount, topClients.Count);
        Assert.Equal(expectedTopClientName, topClients[0].Client?.FullName);
    }
}

