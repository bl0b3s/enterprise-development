namespace CarRental.Application.Contracts.Dto;

/// <summary>
/// DTO для создания и обновления моделей автомобилей
/// </summary>
/// <param name="Name">Название модели автомобиля (например, "BMW 3 Series")</param>
/// <param name="DriveType">Тип привода (FWD, RWD, AWD, 4WD)</param>
/// <param name="SeatsCount">Количество посадочных мест в автомобиле</param>
/// <param name="BodyType">Тип кузова (Sedan, SUV, Coupe, и т.д.)</param>
/// <param name="Class">Класс автомобиля (Economy, Premium, Luxury, и т.д.)</param>
public record CarModelEditDto(
    string Name,
    string DriveType,
    int SeatsCount,
    string BodyType,
    string Class
);