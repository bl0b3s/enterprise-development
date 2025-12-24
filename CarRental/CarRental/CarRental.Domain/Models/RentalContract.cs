using CarRental.Domain_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Domain_.Models;

/// <summary>
/// Договор аренды (контракт)
/// Фиксирует факт выдачи автомобиля клиенту
/// </summary>
public class RentalContract
{
    /// <summary>
    /// Уникальный ID контракта
    /// </summary>
    public required int ContractId { get; set; }

    /// <summary>
    /// Внешний ключ на автомобиль
    /// </summary>
    public required int CarId { get; set; }

    /// <summary>
    /// Внешний ключ на клиента
    /// </summary>
    public required int CustomerId { get; set; }

    /// <summary>
    /// Дата и время выдачи автомобиля клиенту
    /// </summary>
    public required DateTime IssuanceTime { get; set; }

    /// <summary>
    /// Длительность аренды в часах
    /// </summary>
    public required int DurationHours { get; set; }

    /// <summary>
    /// Дата и время возврата (может быть null, если машина ещё в аренде)
    /// </summary>
    public DateTime? ReturnTime { get; set; }

    /// <summary>
    /// Ссылка на арендуемый автомобиль
    /// </summary>
    public Car? Car { get; set; }

    /// <summary>
    /// Ссылка на клиента, арендовавшего машину
    /// </summary>
    public Customer? Customer { get; set; }
}
