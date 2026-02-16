using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRental.Domain.Entities;

/// <summary>
/// Договор аренды (контракт)
/// Фиксирует факт выдачи автомобиля клиенту
/// </summary>
[Table("rentals")]
public class Rental
{
    /// <summary>
    /// Уникальный ID контракта
    /// </summary>
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// Дата и время выдачи автомобиля клиенту
    /// </summary>
    [Column("rental_date")]
    public required DateTime RentalDate { get; set; }

    /// <summary>
    /// Длительность аренды в часах
    /// </summary>
    [Column("rental_hours")]
    public required int RentalHours { get; set; }

    /// <summary>
    /// Внешний ключ на автомобиль
    /// </summary>
    [Column("car_id")]
    public required int CarId { get; set; }

    /// <summary>
    /// Внешний ключ на клиента
    /// </summary>
    [Column("client_id")]
    public required int ClientId { get; set; }

    /// <summary>
    /// Ссылка на арендуемый автомобиль
    /// </summary>
    public Car? Car { get; set; }

    /// <summary>
    /// Ссылка на клиента, арендовавшего машину
    /// </summary>
    public Client? Client { get; set; }
}