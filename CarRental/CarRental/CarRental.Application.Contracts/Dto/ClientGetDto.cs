namespace CarRental.Application.Contracts.Dto;

/// <summary>
/// DTO для получения информации о клиенте
/// </summary>
/// <param name="Id">Уникальный идентификатор клиента</param>
/// <param name="LicenseNumber">Номер водительского удостоверения</param>
/// <param name="FullName">Полное имя клиента</param>
/// <param name="BirthDate">Дата рождения клиента</param>
public record ClientGetDto(
    int Id,
    string LicenseNumber,
    string FullName,
    DateOnly BirthDate
);