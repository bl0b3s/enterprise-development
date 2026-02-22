using System.ComponentModel.DataAnnotations;

namespace CarRental.Application.Dtos.Customers;

/// <summary>
/// DTO for creating a new customer.
/// </summary>
public class CustomerCreateDto
{
    /// <summary>
    /// Driver's license number (unique identifier).
    /// </summary>
    [Required(ErrorMessage = "Driver's license number is required")]
    [StringLength(20, MinimumLength = 8, ErrorMessage = "Driver's license must be between 8 and 20 characters")]
    public required string DriverLicenseNumber { get; set; }

    /// <summary>
    /// Full name of the customer.
    /// </summary>
    [Required(ErrorMessage = "Full name is required")]
    [StringLength(150, MinimumLength = 3, ErrorMessage = "Full name must be between 3 and 150 characters")]
    public required string FullName { get; set; }

    /// <summary>
    /// Date of birth.
    /// </summary>
    [Required(ErrorMessage = "Date of birth is required")]
    public required DateOnly DateOfBirth { get; set; }
}