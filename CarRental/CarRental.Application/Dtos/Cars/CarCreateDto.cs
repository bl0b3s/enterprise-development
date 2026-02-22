using System.ComponentModel.DataAnnotations;

namespace CarRental.Application.Dtos.Cars;
/// <summary>
/// DTO for creating a new car.
/// </summary>
public class CarCreateDto
{
    /// <summary>
    /// License plate number (e.g. A123BC 777).
    /// </summary>
    [Required(ErrorMessage = "License plate is required")]
    [StringLength(12, MinimumLength = 6, ErrorMessage = "License plate must be between 6 and 12 characters")]
    [RegularExpression(@"^[АВЕКМНОРСТУХ]\d{3}[АВЕКМНОРСТУХ]{2}\s\d{3}$",
        ErrorMessage = "License plate format example: A123BC 777")]
    public required string LicensePlate { get; set; }

    /// <summary>
    /// Color of the vehicle.
    /// </summary>
    [Required(ErrorMessage = "Color is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Color must be between 3 and 50 characters")]
    public required string Color { get; set; }

    /// <summary>
    /// ID of the model generation this car belongs to.
    /// </summary>
    [Required(ErrorMessage = "Model generation ID is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid model generation ID")]
    public required int ModelGenerationId { get; set; }
}