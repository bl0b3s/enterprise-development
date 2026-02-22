using CarRental.Infrastructure.Data.Interfaces;
using CarRental.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using CarRental.Domain.Data;
using CarRental.Domain.Enums;
using CarRental.Domain.Models;

namespace CarRental.Infrastructure.Data;

/// <summary>
/// Entity Framework data seeder that uses predefined data from DataSeed class.
/// Provides methods for seeding, clearing and resetting database data for Car Rental application.
/// </summary>
public class EfDataSeeder(AppDbContext context, ILogger<EfDataSeeder> logger, DataSeed data)
    : IDataSeeder
{
    /// <summary>
    /// Seeds the database with initial test or development data.
    /// Entities are seeded in dependency order: independent entities first.
    /// Explicit Id values from seed data are ignored — database generates them automatically.
    /// </summary>
    public async Task SeedAsync()
    {
        logger.LogInformation("Starting database seeding...");

        try
        {
            await SeedCarModelsAsync();
            await SeedModelGenerationsAsync();
            await SeedCarsAsync();
            await SeedCustomersAsync();
            await SeedRentalsAsync();

            logger.LogInformation("Database seeding completed successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred during database seeding");
            throw;
        }
    }

    /// <summary>
    /// Removes all data from the database tables.
    /// Entities are cleared in reverse dependency order to avoid foreign key violations.
    /// </summary>
    public async Task ClearAsync()
    {
        logger.LogInformation("Clearing all database data...");

        try
        {
            await ClearRentalsAsync();
            await ClearCarsAsync();
            await ClearModelGenerationsAsync();
            await ClearCarModelsAsync();
            await ClearCustomersAsync();

            logger.LogInformation("Database clearing completed successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred during database clearing");
            throw;
        }
    }

    /// <summary>
    /// Clears the database and then seeds it with initial data.
    /// Useful for development, testing and demo scenarios.
    /// </summary>
    public async Task ResetAsync()
    {
        logger.LogInformation("Resetting database...");
        await ClearAsync();
        await SeedAsync();
        logger.LogInformation("Database reset completed successfully");
    }

    /// <summary>
    /// Seeds CarModels table if it is empty, ignoring explicit Id values.
    /// </summary>
    private async Task SeedCarModelsAsync()
    {
        if (await context.CarModels.AnyAsync())
        {
            logger.LogInformation("CarModels already exist, skipping seeding");
            return;
        }

        logger.LogInformation("Seeding {Count} car models", data.CarModels.Count);

        var modelsToInsert = data.CarModels.Select(m => new CarModel
        {
            Name = m.Name,
            DriverType = m.DriverType,
            SeatingCapacity = m.SeatingCapacity,
            BodyType = m.BodyType,
            CarClass = m.CarClass
        }).ToList();

        context.CarModels.AddRange(modelsToInsert);
        await context.SaveChangesAsync();

        logger.LogInformation("CarModels seeded successfully");
    }

    /// <summary>
    /// Seeds ModelGenerations table if it is empty, ignoring explicit Id values.
    /// References to CarModel are preserved via original Id mapping.
    /// </summary>
    private async Task SeedModelGenerationsAsync()
    {
        if (await context.ModelGenerations.AnyAsync())
        {
            logger.LogInformation("ModelGenerations already exist, skipping seeding");
            return;
        }

        logger.LogInformation("Seeding {Count} model generations", data.ModelGenerations.Count);

        // Создаём словарь для сопоставления старого Id модели → новой (сгенерированной базой)
        var carModelIdMap = (await context.CarModels.ToListAsync())
            .ToDictionary(
                cm => data.CarModels.First(ds => ds.Name == cm.Name && ds.CarClass == cm.CarClass).Id,
                cm => cm.Id);

        var generationsToInsert = data.ModelGenerations.Select(g => new ModelGeneration
        {
            CarModelId = carModelIdMap[g.CarModelId],
            ProductionYear = g.ProductionYear,
            EngineVolumeLiters = g.EngineVolumeLiters,
            TransmissionType = g.TransmissionType,
            HourlyRate = g.HourlyRate
        }).ToList();

        context.ModelGenerations.AddRange(generationsToInsert);
        await context.SaveChangesAsync();

        logger.LogInformation("ModelGenerations seeded successfully");
    }

    /// <summary>
    /// Seeds Cars table if it is empty, ignoring explicit Id values.
    /// References to ModelGeneration are preserved via mapping.
    /// </summary>
    private async Task SeedCarsAsync()
    {
        if (await context.Cars.AnyAsync())
        {
            logger.LogInformation("Cars already exist, skipping seeding");
            return;
        }

        logger.LogInformation("Seeding {Count} cars", data.Cars.Count);

        // Словарь старый Id поколения → новый
        var generationIdMap = (await context.ModelGenerations.ToListAsync())
            .ToDictionary(
                mg => data.ModelGenerations.First(ds => ds.ProductionYear == mg.ProductionYear && ds.HourlyRate == mg.HourlyRate).Id,
                mg => mg.Id);

        var carsToInsert = data.Cars.Select(c => new Car
        {
            LicensePlate = c.LicensePlate,
            Color = c.Color,
            ModelGenerationId = generationIdMap[c.ModelGenerationId]
        }).ToList();

        context.Cars.AddRange(carsToInsert);
        await context.SaveChangesAsync();

        logger.LogInformation("Cars seeded successfully");
    }

    /// <summary>
    /// Seeds Customers table if it is empty, ignoring explicit Id values.
    /// </summary>
    private async Task SeedCustomersAsync()
    {
        if (await context.Customers.AnyAsync())
        {
            logger.LogInformation("Customers already exist, skipping seeding");
            return;
        }

        logger.LogInformation("Seeding {Count} customers", data.Customers.Count);

        var customersToInsert = data.Customers.Select(c => new Customer
        {
            DriverLicenseNumber = c.DriverLicenseNumber,
            FullName = c.FullName,
            DateOfBirth = c.DateOfBirth
        }).ToList();

        context.Customers.AddRange(customersToInsert);
        await context.SaveChangesAsync();

        logger.LogInformation("Customers seeded successfully");
    }

    /// <summary>
    /// Seeds Rentals table if it is empty, ignoring explicit Id values.
    /// References to Customer and Car are preserved via mapping.
    /// </summary>
    private async Task SeedRentalsAsync()
    {
        if (await context.Rentals.AnyAsync())
        {
            logger.LogInformation("Rentals already exist, skipping seeding");
            return;
        }

        logger.LogInformation("Seeding {Count} rentals", data.Rentals.Count);

        // Словари для сопоставления
        var customerIdMap = (await context.Customers.ToListAsync())
            .ToDictionary(
                c => data.Customers.First(ds => ds.DriverLicenseNumber == c.DriverLicenseNumber).Id,
                c => c.Id);

        var carIdMap = (await context.Cars.ToListAsync())
            .ToDictionary(
                c => data.Cars.First(ds => ds.LicensePlate == c.LicensePlate).Id,
                c => c.Id);

        var rentalsToInsert = data.Rentals.Select(r => new Rental
        {
            CustomerId = customerIdMap[r.CustomerId],
            CarId = carIdMap[r.CarId],
            PickupDateTime = r.PickupDateTime,
            Hours = r.Hours
        }).ToList();

        context.Rentals.AddRange(rentalsToInsert);
        await context.SaveChangesAsync();

        logger.LogInformation("Rentals seeded successfully");
    }

    /// <summary>
    /// Removes all records from the Rentals table.
    /// </summary>
    private async Task ClearRentalsAsync()
    {
        var count = await context.Rentals.ExecuteDeleteAsync();
        LogClearResult("rentals", count);
    }

    /// <summary>
    /// Removes all records from the Cars table.
    /// </summary>
    private async Task ClearCarsAsync()
    {
        var count = await context.Cars.ExecuteDeleteAsync();
        LogClearResult("cars", count);
    }

    /// <summary>
    /// Removes all records from the ModelGenerations table.
    /// </summary>
    private async Task ClearModelGenerationsAsync()
    {
        var count = await context.ModelGenerations.ExecuteDeleteAsync();
        LogClearResult("model generations", count);
    }

    /// <summary>
    /// Removes all records from the CarModels table.
    /// </summary>
    private async Task ClearCarModelsAsync()
    {
        var count = await context.CarModels.ExecuteDeleteAsync();
        LogClearResult("car models", count);
    }

    /// <summary>
    /// Removes all records from the Customers table.
    /// </summary>
    private async Task ClearCustomersAsync()
    {
        var count = await context.Customers.ExecuteDeleteAsync();
        LogClearResult("customers", count);
    }

    /// <summary>
    /// Logs the result of a table clearing operation.
    /// </summary>
    /// <param name="entityName">Plural name of the entity type that was cleared.</param>
    /// <param name="count">Number of records that were removed.</param>
    private void LogClearResult(string entityName, int count)
    {
        if (count > 0)
        {
            logger.LogInformation("Removed {Count} {EntityName}", count, entityName);
        }
        else
        {
            logger.LogInformation("No {EntityName} to remove", entityName);
        }
    }
}