namespace CarRental.Application.Contracts.Dto;

/// <summary>
/// DTO для получения информации о генерации модели
/// </summary>
/// <param name="Id">Уникальный идентификатор генерации модели</param>
/// <param name="Year">Год выпуска поколения модели</param>
/// <param name="EngineVolume">Объем двигателя в литрах</param>
/// <param name="Transmission">Тип трансмиссии (MT, AT, вариатор)</param>
/// <param name="RentalPricePerHour">Стоимость аренды в час</param>
/// <param name="Model">Информация о модели автомобиля</param>
public record ModelGenerationGetDto(
    int Id,
    int Year,
    double EngineVolume,
    string Transmission,
    decimal RentalPricePerHour,
    CarModelGetDto Model
);