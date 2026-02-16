namespace CarRental.Application.Contracts.Dto;

/// <summary>
/// DTO для получения информации об аренде
/// </summary>
/// <param name="Id">Уникальный идентификатор объекта аренды</param>
/// <param name="RentalDate">Дата и время начала аренды</param>
/// <param name="RentalHours">Продолжительность аренды в часах</param>
/// <param name="Car">Информация об арендованном автомобиле</param>
/// <param name="Client">Информация о клиенте</param>
public record RentalGetDto(
    int Id,
    DateTime RentalDate,
    int RentalHours,
    CarGetDto Car,
    ClientGetDto Client
);