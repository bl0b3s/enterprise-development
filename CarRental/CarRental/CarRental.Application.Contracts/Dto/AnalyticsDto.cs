namespace CarRental.Application.Contracts.Dto;

/// <summary>
/// DTO для отображения количества арендованных автомобилей
/// </summary>
/// <param name="Car">Информация об автомобиле</param>
/// <param name="RentalCount">Количество прокатов этого автомобиля</param>
public record CarRentalCountDto(CarGetDto Car, int RentalCount);