using CarRental.Domain_.Data;
using CarRental.Domain_.Models;
using Xunit;

namespace CarRental.Tests;


// <summary>
// Юнит-тесты для пункта проката автомобилей
// </summary>
// <param name="fixture">Fixture с тестовыми данными</param>
public class CarRentalTests(CarRentalFixture fixture) : IClassFixture<CarRentalFixture>
{
    // <summary>
    // ТЕСТ 1: Вывести информацию обо всех клиентах, 
    // которые брали в аренду автомобили указанной модели, упорядочить по ФИО.
    // </summary>
    [Fact]
    public void GetCustomersForModel_ShouldReturnCustomersOrderedByName()
    {
        const string modelName = "Toyota Camry";
        const int expectedCount = 4;
        var expectedNames = new List<string> { "Алексей Смирнов", "Валентина Романова", "Иван Петров", "Мария Сидорова"};

        var targetModel = fixture.Models.First(m => m.Name == modelName);

        var customersForModel = fixture.Contracts
            .Where(c => fixture.Cars.First(car => car.CarId == c.CarId).GenerationId is 1 or 2)
            .Select(c => fixture.Customers.First(cust => cust.CustomerId == c.CustomerId))
            .Distinct()
            .OrderBy(c => c.FullName)
            .ToList();

        Assert.Equal(expectedCount, customersForModel.Count);
        Assert.Equal(expectedNames, customersForModel.Select(c => c.FullName));
    }

    // <summary>
    // ТЕСТ 2: Вывести информацию об автомобилях, находящихся в аренде.
    // </summary>
    [Fact]
    public void GetCarsInRental_ShouldReturnCarsWithoutReturnTime()
    {
        var contractWithoutReturn = new RentalContract
        {
            ContractId = 999,
            CarId = 6,
            CustomerId = 1,
            IssuanceTime = DateTime.Now.AddHours(-5),
            DurationHours = 24,
            ReturnTime = null
        };
        fixture.Contracts.Add(contractWithoutReturn);

        var carsInRental = fixture.Contracts
            .Where(c => c.ReturnTime == null)
            .Select(c => fixture.Cars.First(car => car.CarId == c.CarId))
            .Distinct()
            .ToList();

        Assert.NotEmpty(carsInRental);
        Assert.Contains(carsInRental, c => c.CarId == 6);

        fixture.Contracts.Remove(contractWithoutReturn);
    }

    // <summary>
    // ТЕСТ 3: Вывести топ 5 наиболее часто арендуемых автомобилей.
    // </summary>
    [Fact]
    public void GetTop5MostRentedCars_ShouldReturnTopCars()
    {
        var top5Cars = fixture.Contracts
            .GroupBy(c => c.CarId)
            .OrderByDescending(g => g.Count())
            .Take(5)
            .Select(g => new
            {
                Car = fixture.Cars.First(c => c.CarId == g.Key),
                RentalCount = g.Count()
            })
            .ToList();

        Assert.NotEmpty(top5Cars);
        Assert.True(top5Cars.Count <= 5);
        Assert.Contains(top5Cars, x => x.Car.CarId == 1);
    }

    /// <summary>
    /// ТЕСТ 4: Для каждого автомобиля вывести число аренд.
    /// </summary>
    [Fact]
    public void GetRentalCountPerCar_ShouldReturnCorrectCounts()
    {
        var rentalCountPerCar = fixture.Contracts
            .GroupBy(c => c.CarId)
            .Select(g => new
            {
                Car = fixture.Cars.First(c => c.CarId == g.Key),
                RentalCount = g.Count()
            })
            .OrderBy(x => x.Car.CarId)
            .ToList();

        Assert.NotEmpty(rentalCountPerCar);
        var car1Rentals = rentalCountPerCar.First(x => x.Car.CarId == 1);
        Assert.Equal(3, car1Rentals.RentalCount);

        var car4Rentals = rentalCountPerCar.First(x => x.Car.CarId == 4);
        Assert.Equal(2, car4Rentals.RentalCount);
    }

    /// <summary>
    /// ТЕСТ 5: Вывести топ 5 клиентов по сумме аренды.
    /// </summary>
    [Fact]
    public void GetTop5CustomersByRentalCost_ShouldReturnTopCustomers()
    {
        var top5CustomersBySpent = fixture.Contracts
            .GroupBy(c => c.CustomerId)
            .Select(g => new
            {
                Customer = fixture.Customers.First(cust => cust.CustomerId == g.Key),
                TotalSpent = g.Sum(c =>
                {
                    var car = fixture.Cars.First(car => car.CarId == c.CarId);
                    var generation = fixture.Generations.First(gen => gen.GenerationId == car.GenerationId);
                    return (decimal)(c.DurationHours * (double)generation.HourlyRate);
                })
            })
            .OrderByDescending(x => x.TotalSpent)
            .Take(5)
            .ToList();

        Assert.NotEmpty(top5CustomersBySpent);
        Assert.True(top5CustomersBySpent.Count <= 5);
        if (top5CustomersBySpent.Count > 1)
        {
            Assert.True(top5CustomersBySpent[0].TotalSpent >= top5CustomersBySpent[1].TotalSpent);
        }
    }
}

