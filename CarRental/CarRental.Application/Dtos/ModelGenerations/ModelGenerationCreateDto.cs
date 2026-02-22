using CarRental.Application.Validation;
using CarRental.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CarRental.Application.Dtos.ModelGenerations;

/// <summary>
/// DTO for creating a new model generation.
/// </summary>
public class ModelGenerationCreateDto
{
    /// <summary>
    /// Year when this generation started production.
    /// </summary>
    [Required(ErrorMessage = "Production year is required")]
    [Range(1900, 2100, ErrorMessage = "Production year must be between 1900 and 2100")]
    public required int ProductionYear { get; set; }

    /// <summary>
    /// Engine displacement in liters.
    /// </summary>
    [Required(ErrorMessage = "Engine volume is required")]
    [Range(0.1, 10.0, ErrorMessage = "Engine volume must be between 0.1 and 10.0 liters")]
    public required decimal EngineVolumeLiters { get; set; }

    /// <summary>
    /// Type of transmission.
    /// </summary>
    [Required(ErrorMessage = "Transmission type is required")]
    [EnumRange(typeof(TransmissionType))]
    public required TransmissionType TransmissionType { get; set; }

    /// <summary>
    /// Hourly rental rate for this generation.
    /// </summary>
    [Required(ErrorMessage = "Hourly rate is required")]
    [Range(100, 100000, ErrorMessage = "Hourly rate must be between 100 and 100000")]
    public required decimal HourlyRate { get; set; }

    /// <summary>
    /// ID of the base car model.
    /// </summary>
    [Required(ErrorMessage = "Car model ID is required")]
    [Range(1, int.MaxValue)]
    public required int CarModelId { get; set; }
}