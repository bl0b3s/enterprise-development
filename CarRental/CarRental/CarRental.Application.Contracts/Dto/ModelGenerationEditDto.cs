namespace CarRental.Application.Contracts.Dto;

/// <summary>
/// DTO для создания и обновления поколений моделей
/// </summary>
/// <param name="Year">Год выпуска поколения модели</param>
/// <param name="EngineVolume">Объем двигателя в литрах</param>
/// <param name="Transmission">Тип трансмиссии (MT, AT, CVT)</param>
/// <param name="RentalPricePerHour">Стоимость аренды в час</param>
/// <param name="ModelId">Идентификатор модели автомобиля</param>
public record ModelGenerationEditDto(
    int Year,
    double EngineVolume,
    string Transmission,
    decimal RentalPricePerHour,
    int ModelId
);