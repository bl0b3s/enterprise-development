using CarRental.Domain.Enums;
using CarRental.Domain.Models.Abstract;

namespace CarRental.Domain.Models;

/// <summary>
/// Car model (e.g. Toyota Camry, BMW X5)
/// </summary>
public class CarModel : Model
{
    /// <summary>
    /// Name of the model
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Type of drivetrain
    /// </summary>
    public required DriverType DriveType { get; set; }

    /// <summary>
    /// Number of seats (including driver)
    /// </summary>
    public required byte SeatingCapacity { get; set; }

    /// <summary>
    /// Body style / type
    /// </summary>
    public required BodyType BodyType { get; set; }

    /// <summary>
    /// Vehicle class / market segment
    /// </summary>
    public required CarClass CarClass { get; set; }
}
