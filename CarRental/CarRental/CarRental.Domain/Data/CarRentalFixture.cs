using CarRental.Domain_.Models;
using System.Runtime.ConstrainedExecution;

namespace CarRental.Domain_.Data;


/// <summary>
/// Fixture с тестовыми данными
/// </summary>
public class CarRentalFixture
{
    public List<CarModel> _models;
    public List<ModelGeneration> _generations;
    public List<Car> _cars;
    public List<Clients> _customers;
    public List<RentalContract> _contracts;

    public CarRentalFixture()
    {
        _models =
        [

            new() { ModelId = 1, Name = "Toyota Camry", DriveType = DriveType.FrontWheelDrive, SeatsCount = 5, BodyType = BodyType.Sedan, CarClass = CarClass.Middle },
            new() { ModelId = 2, Name = "BMW X5",        DriveType = DriveType.AllWheelDrive,   SeatsCount = 7, BodyType = BodyType.SUV,   CarClass = CarClass.Premium },
            new() { ModelId = 3, Name = "Volkswagen Golf", DriveType = DriveType.FrontWheelDrive, SeatsCount = 5, BodyType = BodyType.Hatchback, CarClass = CarClass.Economy },
            new() { ModelId = 4, Name = "Audi A6",      DriveType = DriveType.AllWheelDrive,   SeatsCount = 5, BodyType = BodyType.Sedan, CarClass = CarClass.Premium },
            new() { ModelId = 5, Name = "Mercedes C-Class", DriveType = DriveType.RearWheelDrive, SeatsCount = 5, BodyType = BodyType.Sedan, CarClass = CarClass.Premium }
        ];

        _generations =
        [
            new() { GenerationId = 1, ModelId = 1, Year = 2020, EngineVolume = 2.5, TransmissionType = TransmissionType.Automatic, HourlyRate = 50 },
            new() { GenerationId = 2, ModelId = 1, Year = 2022, EngineVolume = 2.5, TransmissionType = TransmissionType.Automatic, HourlyRate = 60 },
            new() { GenerationId = 3, ModelId = 2, Year = 2019, EngineVolume = 3.0, TransmissionType = TransmissionType.Automatic, HourlyRate = 120 },
            new() { GenerationId = 4, ModelId = 2, Year = 2023, EngineVolume = 3.0, TransmissionType = TransmissionType.Automatic, HourlyRate = 150 },
            new() { GenerationId = 5, ModelId = 3, Year = 2021, EngineVolume = 1.5, TransmissionType = TransmissionType.Manual,    HourlyRate = 30  },
            new() { GenerationId = 6, ModelId = 4, Year = 2020, EngineVolume = 2.0, TransmissionType = TransmissionType.Automatic, HourlyRate = 100 },
            new() { GenerationId = 7, ModelId = 5, Year = 2021, EngineVolume = 2.0, TransmissionType = TransmissionType.Automatic, HourlyRate = 110 }
        ];

        _cars = 
        [
            new() { CarId = 1, GenerationId = 1, LicensePlate = "А001РС77", Color = "Белый" },
            new() { CarId = 2, GenerationId = 1, LicensePlate = "А002РС77", Color = "Черный" },
            new() { CarId = 3, GenerationId = 2, LicensePlate = "А003РС77", Color = "Серебристый" },
            new() { CarId = 4, GenerationId = 3, LicensePlate = "В001РС77", Color = "Черный" },
            new() { CarId = 5, GenerationId = 4, LicensePlate = "В002РС77", Color = "Белый" },
            new() { CarId = 6, GenerationId = 5, LicensePlate = "В003РС77", Color = "Синий" },
            new() { CarId = 7, GenerationId = 5, LicensePlate = "В004РС77", Color = "Красный" },
            new() { CarId = 8, GenerationId = 6, LicensePlate = "А004РС77", Color = "Серебристый" },
            new() { CarId = 9, GenerationId = 6, LicensePlate = "А005РС77", Color = "Черный" },
            new() { CarId = 10, GenerationId = 7, LicensePlate = "А006РС77", Color = "Серебристый" }
        ];

        _customers =
        [
            new() { CustomerId = 1, DriverLicenseNumber = "7701123456", FullName = "Иван Петров",        DateOfBirth = new DateOnly(1990, 5, 15) },
            new() { CustomerId = 2, DriverLicenseNumber = "7701234567", FullName = "Мария Сидорова",     DateOfBirth = new DateOnly(1985, 8, 22) },
            new() { CustomerId = 3, DriverLicenseNumber = "7701345678", FullName = "Алексей Смирнов",    DateOfBirth = new DateOnly(1992, 3, 10) },
            new() { CustomerId = 4, DriverLicenseNumber = "7701456789", FullName = "Ольга Волкова",      DateOfBirth = new DateOnly(1988, 11, 30) },
            new() { CustomerId = 5, DriverLicenseNumber = "7701567890", FullName = "Сергей Морозов",     DateOfBirth = new DateOnly(1995, 6, 18) },
            new() { CustomerId = 6, DriverLicenseNumber = "7701678901", FullName = "Елена Николаева",    DateOfBirth = new DateOnly(1989, 2, 25) },
            new() { CustomerId = 7, DriverLicenseNumber = "7701789012", FullName = "Дмитрий Соколов",    DateOfBirth = new DateOnly(1987, 9, 12) },
            new() { CustomerId = 8, DriverLicenseNumber = "7701890123", FullName = "Анна Федорова",      DateOfBirth = new DateOnly(1991, 4, 8) },
            new() { CustomerId = 9, DriverLicenseNumber = "7701901234", FullName = "Борис Козлов",       DateOfBirth = new DateOnly(1993, 7, 20) },
            new() { CustomerId = 10,DriverLicenseNumber = "7702012345", FullName = "Валентина Романова", DateOfBirth = new DateOnly(1986, 12, 5) }
        ];

        _contracts = 
        [
            new() { ContractId = 1,  CarId = 1,  CustomerId = 1,  IssuanceTime = new DateTime(2025, 1, 5, 10, 0, 0), DurationHours = 24, ReturnTime = new DateTime(2025, 1, 6, 10, 0, 0) },
            new() { ContractId = 2,  CarId = 1,  CustomerId = 1,  IssuanceTime = new DateTime(2025, 1, 20, 14, 0, 0), DurationHours = 8,  ReturnTime = new DateTime(2025, 1, 20, 22, 0, 0) },
            new() { ContractId = 3,  CarId = 1,  CustomerId = 2,  IssuanceTime = new DateTime(2025, 1, 10, 9, 0, 0),  DurationHours = 48, ReturnTime = new DateTime(2025, 1, 12, 9, 0, 0) },
            new() { ContractId = 4,  CarId = 2,  CustomerId = 3,  IssuanceTime = new DateTime(2025, 1, 8, 12, 0, 0), DurationHours = 16, ReturnTime = new DateTime(2025, 1, 9, 4, 0, 0) },
            new() { ContractId = 5,  CarId = 4,  CustomerId = 4,  IssuanceTime = new DateTime(2025, 1, 3, 11, 0, 0), DurationHours = 72, ReturnTime = new DateTime(2025, 1, 6, 11, 0, 0) },
            new() { ContractId = 6,  CarId = 5,  CustomerId = 5,  IssuanceTime = new DateTime(2025, 1, 15, 8, 0, 0), DurationHours = 24, ReturnTime = new DateTime(2025, 1, 16, 8, 0, 0) },
            new() { ContractId = 7,  CarId = 6,  CustomerId = 6,  IssuanceTime = new DateTime(2025, 1, 12, 10, 0, 0), DurationHours = 6,  ReturnTime = new DateTime(2025, 1, 12, 16, 0, 0) },
            new() { ContractId = 8,  CarId = 7,  CustomerId = 7,  IssuanceTime = new DateTime(2025, 1, 18, 15, 0, 0), DurationHours = 12, ReturnTime = new DateTime(2025, 1, 19, 3, 0, 0) },
            new() { ContractId = 9,  CarId = 8,  CustomerId = 8,  IssuanceTime = new DateTime(2025, 1, 7, 13, 0, 0),  DurationHours = 20, ReturnTime = new DateTime(2025, 1, 8, 9, 0, 0) },
            new() { ContractId = 10, CarId = 10, CustomerId = 9,  IssuanceTime = new DateTime(2025, 1, 11, 9, 0, 0), DurationHours = 36, ReturnTime = new DateTime(2025, 1, 12, 21, 0, 0) },
            new() { ContractId = 11, CarId = 3,  CustomerId = 10, IssuanceTime = new DateTime(2025, 1, 14, 12, 0, 0), DurationHours = 8,  ReturnTime = new DateTime(2025, 1, 14, 20, 0, 0) },
            new() { ContractId = 12, CarId = 4,  CustomerId = 6,  IssuanceTime = new DateTime(2025, 1, 21, 10, 0, 0), DurationHours = 24, ReturnTime = new DateTime(2025, 1, 22, 10, 0, 0) }
        ];
    }
    public List<CarModel> Models => _models;
    public List<ModelGeneration> Generations => _generations;
    public List<Car> Cars => _cars;
    public List<Clients> Customers => _customers;
    public List<RentalContract> Contracts => _contracts;

    public CarRentalFixture()
    {
        Customers.AddRange(
        [
            new() { CustomerId = 1, FullName = "Иван Петров" },
            new() { CustomerId = 2, FullName = "Мария Сидорова" },
            new() { CustomerId = 3, FullName = "Алексей Смирнов" },
            new() { CustomerId = 4, FullName = "Валентина Романова" }
        ]);

        Cars.AddRange(
        [
            new() { CarId = 1, ModelId = 1, GenerationId = 1, LicensePlate = "A111AA", Color = "Белый" },
            new() { CarId = 2, ModelId = 1, GenerationId = 2, LicensePlate = "A222AA", Color = "Черный" },
            new() { CarId = 3, ModelId = 2, GenerationId = 3, LicensePlate = "B333BB", Color = "Серый" },
            new() { CarId = 4, ModelId = 3, GenerationId = 4, LicensePlate = "C444CC", Color = "Красный" },
            new() { CarId = 5, ModelId = 4, GenerationId = 5, LicensePlate = "D555DD", Color = "Синий" },
            new() { CarId = 6, ModelId = 5, GenerationId = 6, LicensePlate = "E666EE", Color = "Черный" }
        ]);

        Contracts.AddRange(
        [
            new() { ContractId = 1, CarId = 1, CustomerId = 1, IssuanceTime = new DateTime(2024, 1, 1, 10, 0, 0), DurationHours = 5, ReturnTime = new DateTime(2024, 1, 1, 15, 0, 0) },
            new() { ContractId = 2, CarId = 1, CustomerId = 2, IssuanceTime = new DateTime(2024, 2, 1, 9, 0, 0), DurationHours = 24, ReturnTime = new DateTime(2024, 2, 2, 9, 0, 0) },
            new() { ContractId = 3, CarId = 2, CustomerId = 3, IssuanceTime = new DateTime(2024, 3, 1, 12, 0, 0), DurationHours = 10, ReturnTime = new DateTime(2024, 3, 1, 22, 0, 0) },
            new() { ContractId = 999, CarId = 6, CustomerId = 1, IssuanceTime = new DateTime(2024, 4, 1, 10, 0, 0), DurationHours = 24, ReturnTime = null }
        ]);
    }
}