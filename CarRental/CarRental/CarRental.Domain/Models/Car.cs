namespace CarRental.Domain_.Models;

/// <summary>
/// Автомобиль в парке
/// </summary>
public class Car
{
    /// <summary>
    /// Уникальный ID автомобиля в парке
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Внешний ключ на поколение модели
    /// </summary>
    public required int ModelGenerationId { get; set; }

    /// <summary>
    /// Государственный номер
    /// </summary>
    public required string LicensePlate { get; set; }

    /// <summary>
    /// Цвет кузова
    /// </summary>
    public required string Color { get; set; }

    /// <summary>
    /// Ссылка на поколение модели этого автомобиля
    /// </summary>
    public required ModelGeneration ModelGeneration { get; set; }
}

