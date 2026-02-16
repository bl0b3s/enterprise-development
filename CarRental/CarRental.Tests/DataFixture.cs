using CarRental.Domain.Models;

namespace CarRental.Tests;

/// <summary>
/// Provides sample data for car rental domain entities
/// </summary>
public class CarRentalDataFixture
{
    public List<CarModel> CarModels { get; } = new()
    {
        new() { Id = 1, Name = "Toyota Camry", DriveType = DriveType.FrontWheelDrive, SeatingCapacity = 5, BodyType = BodyType.Sedan, CarClass = CarClass.Intermediate },
        new() { Id = 2, Name = "Kia Rio", DriveType = DriveType.FrontWheelDrive, SeatingCapacity = 5, BodyType = BodyType.Hatchback, CarClass = CarClass.Economy },
        new() { Id = 3, Name = "BMW X5", DriveType = DriveType.AllWheelDrive, SeatingCapacity = 5, BodyType = BodyType.SUV, CarClass = CarClass.Premium },
        new() { Id = 4, Name = "Hyundai Solaris", DriveType = DriveType.FrontWheelDrive, SeatingCapacity = 5, BodyType = BodyType.Sedan, CarClass = CarClass.Economy },
        new() { Id = 5, Name = "Volkswagen Tiguan", DriveType = DriveType.AllWheelDrive, SeatingCapacity = 5, BodyType = BodyType.Crossover, CarClass = CarClass.Intermediate },
    };

    public List<ModelGeneration> ModelGenerations { get; } = new()
    {
        new() { Id = 1, CarModelId = 1, ProductionYear = 2023, EngineVolumeLiters = 2.5m, TransmissionType = TransmissionType.Automatic, HourlyRate = 1200 },
        new() { Id = 2, CarModelId = 1, ProductionYear = 2019, EngineVolumeLiters = 2.0m, TransmissionType = TransmissionType.CVT, HourlyRate = 950 },
        new() { Id = 3, CarModelId = 2, ProductionYear = 2024, EngineVolumeLiters = 1.6m, TransmissionType = TransmissionType.Automatic, HourlyRate = 650 },
        new() { Id = 4, CarModelId = 3, ProductionYear = 2022, EngineVolumeLiters = 3.0m, TransmissionType = TransmissionType.Automatic, HourlyRate = 3500 },
        new() { Id = 5, CarModelId = 4, ProductionYear = 2023, EngineVolumeLiters = 1.6m, TransmissionType = TransmissionType.Automatic, HourlyRate = 700 },
        new() { Id = 6, CarModelId = 5, ProductionYear = 2021, EngineVolumeLiters = 2.0m, TransmissionType = TransmissionType.DualClutch, HourlyRate = 1400 },
    };

    public List<Car> Cars { get; } = new()
    {
        new() { Id = 1, LicensePlate = "А123ВС 777", Color = "Черный", ModelGenerationId = 1 },
        new() { Id = 2, LicensePlate = "В456ОР 777", Color = "Белый", ModelGenerationId = 2 },
        new() { Id = 3, LicensePlate = "Е789КХ 777", Color = "Синий", ModelGenerationId = 3 },
        new() { Id = 4, LicensePlate = "К001МР 777", Color = "Серебро", ModelGenerationId = 4 },
        new() { Id = 5, LicensePlate = "М234ТН 777", Color = "Красный", ModelGenerationId = 5 },
        new() { Id = 6, LicensePlate = "Н567УХ 777", Color = "Серый", ModelGenerationId = 1 },
        new() { Id = 7, LicensePlate = "О890ЦВ 777", Color = "Черный", ModelGenerationId = 3 },
    };

    public List<Customer> Customers { get; } = new()
    {
        new() { Id = 1, DriverLicenseNumber = "1234 567890", FullName = "Иванов Иван Иванович", DateOfBirth = new DateOnly(1985, 3, 12) },
        new() { Id = 2, DriverLicenseNumber = "2345 678901", FullName = "Петрова Анна Сергеевна", DateOfBirth = new DateOnly(1992, 7, 19) },
        new() { Id = 3, DriverLicenseNumber = "3456 789012", FullName = "Сидоров Алексей Петрович", DateOfBirth = new DateOnly(1978, 11, 5) },
        new() { Id = 4, DriverLicenseNumber = "4567 890123", FullName = "Кузнецова Мария Дмитриевна", DateOfBirth = new DateOnly(1990, 4, 28) },
        new() { Id = 5, DriverLicenseNumber = "5678 901234", FullName = "Смирнов Дмитрий Александрович", DateOfBirth = new DateOnly(1982, 9, 15) },
        new() { Id = 6, DriverLicenseNumber = "6789 012345", FullName = "Волкова Ольга Николаевна", DateOfBirth = new DateOnly(1995, 1, 8) },
    };

    public List<Rental> Rentals { get; } = new()
    {
        new() { Id = 1, CustomerId = 1, CarId = 1, PickupDateTime = new DateTime(2025, 10, 1, 9, 0, 0), Hours = 24 },
        new() { Id = 2, CustomerId = 2, CarId = 3, PickupDateTime = new DateTime(2025, 10, 3, 14, 0, 0), Hours = 8 },
        new() { Id = 3, CustomerId = 1, CarId = 1, PickupDateTime = new DateTime(2025, 9, 15, 10, 0, 0), Hours = 48 },
        new() { Id = 4, CustomerId = 3, CarId = 4, PickupDateTime = new DateTime(2025, 10, 5, 11, 30, 0), Hours = 5 },
        new() { Id = 5, CustomerId = 4, CarId = 1, PickupDateTime = new DateTime(2025, 10, 7, 8, 0, 0), Hours = 72 },
        new() { Id = 6, CustomerId = 2, CarId = 3, PickupDateTime = new DateTime(2025, 9, 20, 16, 0, 0), Hours = 24 },
        new() { Id = 7, CustomerId = 5, CarId = 7, PickupDateTime = new DateTime(2025, 10, 10, 12, 0, 0), Hours = 12 },
        new() { Id = 8, CustomerId = 1, CarId = 6, PickupDateTime = new DateTime(2025, 10, 12, 9, 0, 0), Hours = 36 },
        new() { Id = 9, CustomerId = 6, CarId = 2, PickupDateTime = new DateTime(2025, 10, 14, 13, 0, 0), Hours = 4 },
        new() { Id = 10, CustomerId = 3, CarId = 1, PickupDateTime = new DateTime(2025, 9, 25, 10, 0, 0), Hours = 24 },
        new() { Id = 11, CustomerId = 2, CarId = 3, PickupDateTime = new DateTime(2025, 10, 10, 8, 0, 0), Hours = 240 },
        new() { Id = 12, CustomerId = 5, CarId = 1, PickupDateTime = new DateTime(2025, 10, 15, 14, 0, 0), Hours = 36  }
    };
}