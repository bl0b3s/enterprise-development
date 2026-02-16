using CarRental.Domain.Models.Abstract;
using CarRental.Domain.Enums;

namespace CarRental.Domain.Models;
/// <summary>
/// Generation / specific version of a car model
/// </summary>
public class ModelGeneration : Model
{
    /// <summary>
    /// Year of manufacture / start of production for this generation
    /// </summary>
    public required int ProductionYear { get; set; }

    /// <summary>
    /// Engine displacement in liters
    /// </summary>
    public required decimal EngineVolumeLiters { get; set; }

    /// <summary>
    /// Type of transmission
    /// </summary>
    public required TransmissionType TransmissionType { get; set; }

    /// <summary>
    /// Hourly rental price for this generation
    /// </summary>
    public required decimal HourlyRate { get; set; }

    /// <summary>
    /// Foreign key to the base car model
    /// </summary>
    public required int CarModelId { get; set; }
}
