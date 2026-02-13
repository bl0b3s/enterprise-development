namespace CarRental.Domain_.Models;

/// <summary>
/// Поколение модели (справочник)
/// </summary>
public class ModelGeneration
{
    /// <summary>
    /// Уникальный идентификатор поколения
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Внешний ключ на модель
    /// </summary>
    public required int ModelId { get; set; }

    /// <summary>
    /// Год выпуска
    /// </summary>
    public required int Year { get; set; }

    /// <summary>
    /// Объем двигателя в литрах
    /// </summary>
    public required double EngineVolume { get; set; }

    /// <summary>
    /// Тип коробки передач
    /// </summary>
    public required string Transmission { get; set; }

    /// <summary>
    /// Стоимость аренды в час
    /// </summary>
    public required decimal RentalPricePerHour { get; set; }

    /// <summary>
    /// Ссылка на модель (навигационное свойство)
    /// </summary>
    public required CarModel Model { get; set; }
}
