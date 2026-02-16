using CarRental.Domain.Models.Abstract;

namespace CarRental.Domain.Models;

/// <summary>
/// Represents a car in the car rental service.
/// </summary>
public class Car : Model
{
    /// <summary>
    /// License plate number
    /// </summary>
    public required string LicensePlate { get; set; }

    /// <summary>
    /// Color of the vehicle
    /// </summary>
    public required string Color { get; set; }

    /// <summary>
    /// Foreign key to the model generation this car belongs to
    /// </summary>
    public required int ModelGenerationId { get; set; }
}
