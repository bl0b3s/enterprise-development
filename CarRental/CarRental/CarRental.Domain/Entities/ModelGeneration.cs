using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRental.Domain.Entities;

/// <summary>
/// Поколение модели (справочник)
/// </summary>
[Table("model_generations")]
public class ModelGeneration
{
    /// <summary>
    /// Уникальный идентификатор поколения
    /// </summary>
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// Год выпуска
    /// </summary>
    [Column("year")]
    public required int Year { get; set; }

    /// <summary>
    /// Объем двигателя в литрах
    /// </summary>
    [Column("engine_volume")]
    public required double EngineVolume { get; set; }

    /// <summary>
    /// Тип коробки передач
    /// </summary>
    [Column("transmission")]
    [MaxLength(10)]
    public required string Transmission { get; set; }

    /// <summary>
    /// Стоимость аренды в час
    /// </summary>
    [Column("rental_price_per_hour")]
    public required decimal RentalPricePerHour { get; set; }

    /// <summary>
    /// Внешний ключ на модель
    /// </summary>
    [Column("model_id")]
    public required int ModelId { get; set; }

    /// <summary>
    /// Ссылка на модель (навигационное свойство)
    /// </summary>
    public CarModel? Model { get; set; }
}