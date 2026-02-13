using CarRental.Domain_.Models;

namespace CarRental.Domain_.Models;

/// <summary>
/// Договор аренды (контракт)
/// Фиксирует факт выдачи автомобиля клиенту
/// </summary>
public class Rental
{
    /// <summary>
    /// Уникальный ID контракта
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Внешний ключ на автомобиль
    /// </summary>
    public required int CarId { get; set; }

    /// <summary>
    /// Внешний ключ на клиента
    /// </summary>
    public required int ClientId { get; set; }

    /// <summary>
    /// Дата и время выдачи автомобиля клиенту
    /// </summary>
    public required DateTime RentalDate { get; set; }

    /// <summary>
    /// Длительность аренды в часах
    /// </summary>
    public required int RentalHours { get; set; }

    /// <summary>
    /// Ссылка на арендуемый автомобиль
    /// </summary>
    public required Car Car { get; set; }

    /// <summary>
    /// Ссылка на клиента, арендовавшего машину
    /// </summary>
    public required Client Client { get; set; }
}
