using CarRental.Domain.Models.Abstract;

namespace CarRental.Domain.Models;

/// <summary>
/// Rental agreement / contract
/// </summary>
public class Rental : Model
{
    /// <summary>
    /// Customer who rents the car
    /// </summary>
    public required int CustomerId { get; set; }

    /// <summary>
    /// Car being rented
    /// </summary>
    public required int CarId { get; set; }

    /// <summary>
    /// Date and time when the vehicle was handed over
    /// </summary>
    public required DateTime PickupDateTime { get; set; }

    /// <summary>
    /// Duration of the rental in hours
    /// </summary>
    public required int Hours { get; set; }

    /// <summary>
    /// Calculated expected return time
    /// </summary>
    public DateTime ExpectedReturnDateTime => PickupDateTime.AddHours(Hours);
}
