namespace CarRental.Tests;

/// <summary>
/// Tests for car rental functionalities.
/// </summary>
public class LinqQueryTests(CarRentalDataFixture testData) : IClassFixture<CarRentalDataFixture>
{
    /// <summary>
    /// Displays information about all customers who rented cars of the specified model, sorted by full name.
    /// </summary>
    [Fact]
    public void GetCustomersByModel()
    {
        const int targetModelId = 1;

        var expectedFullNames = new List<string>
        {
            "Иванов Иван Иванович",
            "Кузнецова Мария Дмитриевна",
            "Сидоров Алексей Петрович"
        };

        var actualFullNames = testData.Rentals
            .Join(testData.Cars,
                r => r.CarId,
                c => c.Id,
                (r, c) => new { Rental = r, Car = c })
            .Where(x => testData.ModelGenerations
                .Any(mg => mg.Id == x.Car.ModelGenerationId && mg.CarModelId == targetModelId))
            .Select(x => x.Rental.CustomerId)
            .Distinct()
            .Join(testData.Customers,
                cid => cid,
                c => c.Id,
                (_, c) => c.FullName)
            .OrderBy(name => name)
            .ToList();

        Assert.Equal(expectedFullNames, actualFullNames);
    }

    /// <summary>
    /// Displays information about cars that are currently rented.
    /// </summary>
    [Fact]
    public void GetCurrentlyRentedCars()
    {
        var now = new DateTime(2025, 10, 16, 12, 0, 0);

        var expectedPlates = new List<string>
        {
            "А123ВС 777",
            "Е789КХ 777"
        };

        var actualPlates = testData.Rentals
            .Where(r => r.PickupDateTime <= now && r.PickupDateTime.AddHours(r.Hours) > now)
            .Join(testData.Cars,
                r => r.CarId,
                c => c.Id,
                (r, c) => c.LicensePlate)
            .OrderBy(plate => plate)
            .ToList();

        Assert.Equal(expectedPlates, actualPlates);
    }

    /// <summary>
    /// Displays the top 5 most frequently rented cars.
    /// </summary>
    [Fact]
    public void GetTop5MostRentedCars()
    {
        var expectedPlates = new List<string>
        {
            "А123ВС 777",
            "Е789КХ 777",
            "В456ОР 777",
            "К001МР 777",
            "М234ТН 777"
        };

        var actualPlates = testData.Rentals
            .GroupBy(r => r.CarId)
            .Select(g => new
            {
                CarId = g.Key,
                Count = g.Count()
            })
            .OrderByDescending(x => x.Count)
            .ThenBy(x => x.CarId)
            .Take(5)
            .Join(testData.Cars,
                x => x.CarId,
                c => c.Id,
                (x, c) => c.LicensePlate)
            .ToList();

        Assert.Equal(expectedPlates, actualPlates);
    }

    /// <summary>
    /// Displays the number of rentals for each car.
    /// </summary>
    [Fact]
    public void GetRentalCountForEachCar()
    {
        var expected = new Dictionary<string, int>
        {
            { "А123ВС 777", 3 },
            { "В456ОР 777", 1 },
            { "Е789КХ 777", 2 },
            { "К001МР 777", 1 },
            { "М234ТН 777", 1 },
            { "Н567УХ 777", 1 },
            { "О890ЦВ 777", 1 }
        };

        var actual = testData.Rentals
            .GroupBy(r => r.CarId)
            .Select(g => new
            {
                Plate = testData.Cars.First(c => c.Id == g.Key).LicensePlate,
                Count = g.Count()
            })
            .OrderBy(x => x.Plate)
            .ToDictionary(x => x.Plate, x => x.Count);

        Assert.Equal(expected, actual);
    }

    /// <summary>
    /// Displays the top 5 customers by total rental amount.
    /// </summary>
    [Fact]
    public void GetTop5CustomersByTotalCost()
    {
        var expectedFullNames = new List<string>
        {
            "Иванов Иван Иванович",
            "Петрова Анна Сергеевна",
            "Кузнецова Мария Дмитриевна",
            "Сидоров Алексей Петрович",
            "Смирнов Дмитрий Александрович"
        };

        var actualFullNames = testData.Rentals
            .Join(testData.Cars,
                r => r.CarId,
                c => c.Id,
                (r, c) => new { Rental = r, Car = c })
            .Join(testData.ModelGenerations,
                x => x.Car.ModelGenerationId,
                mg => mg.Id,
                (x, mg) => new
                {
                    x.Rental.CustomerId,
                    Cost = mg.HourlyRate * x.Rental.Hours
                })
            .GroupBy(x => x.CustomerId)
            .Select(g => new
            {
                CustomerId = g.Key,
                Total = g.Sum(x => x.Cost)
            })
            .OrderByDescending(x => x.Total)
            .Take(5)
            .Join(testData.Customers,
                x => x.CustomerId,
                c => c.Id,
                (x, c) => c.FullName)
            .ToList();

        Assert.Equal(expectedFullNames, actualFullNames);
    }
}