using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRental.Domain.Entities;

/// <summary>
/// Автомобиль в парке
/// </summary>
[Table("cars")]
public class Car
{
    /// <summary>
    /// Уникальный ID автомобиля в парке
    /// </summary>
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// Государственный номер
    /// </summary>
    [Column("license_plate")]
    [MaxLength(20)]
    public required string LicensePlate { get; set; }

    /// <summary>
    /// Цвет кузова
    /// </summary>
    [Column("color")]
    [MaxLength(30)]
    public required string Color { get; set; }

    /// <summary>
    /// Внешний ключ на поколение модели
    /// </summary>
    [Column("model_generation_id")]
    public required int ModelGenerationId { get; set; }

    /// <summary>
    /// Ссылка на поколение модели этого автомобиля
    /// </summary>
    public ModelGeneration? ModelGeneration { get; set; }
}