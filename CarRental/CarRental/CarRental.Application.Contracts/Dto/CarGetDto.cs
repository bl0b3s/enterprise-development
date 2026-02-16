namespace CarRental.Application.Contracts.Dto;

/// <summary>
/// DTO для получения информации об автомобиле
/// </summary>
/// <param name="Id">Уникальный идентификатор автомобиля</param>
/// <param name="LicensePlate">Номерной знак автомобиля</param>
/// <param name="Color">Цвет автомобиля</param>
/// <param name="ModelGeneration">Информация о создании модели, включая подробные сведения о модели</param>
public record CarGetDto(
    int Id,
    string LicensePlate,
    string Color,
    ModelGenerationGetDto ModelGeneration
);