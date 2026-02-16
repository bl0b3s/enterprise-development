using CarRental.Domain.Models.Abstract;

namespace CarRental.Domain.Models;

/// <summary>
/// Customer / renter of the vehicle
/// </summary>
public class Customer : Model
{
    /// <summary>
    /// Driver's license number (used as unique business identifier)
    /// </summary>
    public required string DriverLicenseNumber { get; set; }

    /// <summary>
    /// Full name of the customer
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// Date of birth
    /// </summary>
    public required DateOnly DateOfBirth { get; set; }
}
