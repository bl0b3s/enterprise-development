namespace CarRental.Domain_.Models;

/// <summary>
/// Автомобиль в парке
/// </summary>
public class Car
{
    /// <summary>
    /// Уникальный ID автомобиля в парке
    /// </summary>
    public required int CarId { get; set; }

    /// <summary>
    /// Внешний ключ на поколение модели
    /// </summary>
    public required int GenerationId { get; set; }

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
    public ModelGeneration? Generation { get; set; }

    /// <summary>
    /// История аренд этого автомобиля
    /// </summary>
    public ICollection<RentalContract> RentalContracts { get; set; } = new List<RentalContract>();
}

