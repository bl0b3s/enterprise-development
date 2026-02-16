namespace CarRental.Domain.Models;

/// <summary>
/// Модель автомобиля (справочник)
/// </summary>
public class CarModel
{
    /// <summary>
    /// Уникальный идентификатор модели
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Название модели (например, "Toyota Camry")
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Тип привода
    /// </summary>
    public required string DriveType { get; set; }

    /// <summary>
    /// Количество посадочных мест
    /// </summary>
    public required int SeatsCount { get; set; }

    /// <summary>
    /// Тип кузова
    /// </summary>
    public required string BodyType { get; set; }

    /// <summary>
    /// Класс автомобиля
    /// </summary>
    public required string Class { get; set; }
}

