using CarRental.Domain_.Data;

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

        var clients = fixture.Rentals
            .Where(r => r.Car.ModelGeneration.Model.Name == targetModel)
            .Select(r => r.Client)
            .Distinct()
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

        var rentedCars = fixture.Rentals
            .Where(r => r.RentalDate.AddHours(r.RentalHours) > testDate)
            .Select(r => r.Car)
            .Distinct()
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

        var topCars = fixture.Rentals
            .GroupBy(r => r.Car)
            .Select(g => new { Car = g.Key, RentalCount = g.Count() })
            .OrderByDescending(x => x.RentalCount)
            .Take(5)
            .ToList();

        Assert.Equal(expectedCount, topCars.Count);
        Assert.Equal(expectedTopCarPlate, topCars[0].Car.LicensePlate);
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

        var carsWithRentalCount = fixture.Cars
            .Select(car => new
            {
                Car = car,
                RentalCount = fixture.Rentals.Count(r => r.CarId == car.Id)
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

        var topClients = fixture.Rentals
            .GroupBy(r => r.Client)
            .Select(g => new
            {
                Client = g.Key,
                TotalAmount = g.Sum(r => r.RentalHours * r.Car.ModelGeneration.RentalPricePerHour)
            })
            .OrderByDescending(x => x.TotalAmount)
            .Take(5)
            .ToList();

        Assert.Equal(expectedCount, topClients.Count);
        Assert.Equal(expectedTopClientName, topClients[0].Client.FullName);
    }
}

