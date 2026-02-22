using CarRental.Application.Validation;
using CarRental.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CarRental.Application.Dtos.CarModels;
/// <summary>
/// DTO for creating a new car model.
/// </summary>
public class CarModelCreateDto
{
    /// <summary>
    /// Name of the car model (e.g. Toyota Camry, BMW X5).
    /// </summary>
    [Required(ErrorMessage = "Model name is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Model name must be between 2 and 100 characters")]
    public required string Name { get; set; }

    /// <summary>
    /// Drivetrain type.
    /// </summary>
    [Required(ErrorMessage = "Drivetrain type is required")]
    [EnumRange(typeof(DriverType))]
    public required DriverType DriverType { get; set; }

    /// <summary>
    /// Number of seats (including driver).
    /// </summary>
    [Required(ErrorMessage = "Seating capacity is required")]
    [Range(2, 20, ErrorMessage = "Seating capacity must be between 2 and 20")]
    public required byte SeatingCapacity { get; set; }

    /// <summary>
    /// Body type / style.
    /// </summary>
    [Required(ErrorMessage = "Body type is required")]
    [EnumRange(typeof(BodyType))]
    public required BodyType BodyType { get; set; }

    /// <summary>
    /// Vehicle class / segment.
    /// </summary>
    [Required(ErrorMessage = "Car class is required")]
    [EnumRange(typeof(CarClass))]
    public required CarClass CarClass { get; set; }
}