namespace CarRental.Application.Contracts.Dto;

/// <summary>
/// DTO для создания и обновления автомобилей
/// </summary>
/// <param name="LicensePlate">Номерной знак автомобиля</param>
/// <param name="Color">Цвет автомобиля</param>
/// <param name="ModelGenerationId">Идентификатор поколения модели</param>
public record CarEditDto(
    string LicensePlate,
    string Color,
    int ModelGenerationId
);