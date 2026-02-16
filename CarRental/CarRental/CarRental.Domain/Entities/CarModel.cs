using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRental.Domain.Entities;

/// <summary>
/// Модель автомобиля (справочник)
/// </summary>
[Table("car_models")]
public class CarModel
{
    /// <summary>
    /// Уникальный идентификатор модели
    /// </summary>
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// Название модели (например, "Toyota Camry")
    /// </summary>
    [Column("name")]
    [MaxLength(50)]
    public required string Name { get; set; }

    /// <summary>
    /// Тип привода
    /// </summary>
    [Column("drive_type")]
    [MaxLength(10)]
    public required string DriveType { get; set; }

    /// <summary>
    /// Количество посадочных мест
    /// </summary>
    [Column("seats_count")]
    public required int SeatsCount { get; set; }

    /// <summary>
    /// Тип кузова
    /// </summary>
    [Column("body_type")]
    [MaxLength(20)]
    public required string BodyType { get; set; }

    /// <summary>
    /// Класс автомобиля
    /// </summary>
    [Column("class")]
    [MaxLength(20)]
    public required string Class { get; set; }
}