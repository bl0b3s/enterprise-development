using System.ComponentModel.DataAnnotations;

namespace CarRental.Application.Dtos.Rentals;
/// <summary>
/// DTO for creating a new rental agreement.
/// </summary>
public class RentalCreateDto
{
    /// <summary>
    /// ID of the customer renting the car.
    /// </summary>
    [Required(ErrorMessage = "Customer ID is required")]
    [Range(1, int.MaxValue)]
    public required int CustomerId { get; set; }

    /// <summary>
    /// ID of the car being rented.
    /// </summary>
    [Required(ErrorMessage = "Car ID is required")]
    [Range(1, int.MaxValue)]
    public required int CarId { get; set; }

    /// <summary>
    /// Date and time when the car is picked up.
    /// </summary>
    [Required(ErrorMessage = "Pickup date and time is required")]
    public required DateTime PickupDateTime { get; set; }

    /// <summary>
    /// Rental duration in hours.
    /// </summary>
    [Required(ErrorMessage = "Rental duration is required")]
    [Range(1, 8760, ErrorMessage = "Duration must be between 1 hour and 1 year (8760 hours)")]
    public required int Hours { get; set; }
}