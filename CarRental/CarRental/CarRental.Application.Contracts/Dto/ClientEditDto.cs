namespace CarRental.Application.Contracts.Dto;

/// <summary>
/// DTO для создания и обновления клиентов
/// </summary>
/// <param name="LicenseNumber">Номер водительского удостоверения</param>
/// <param name="FullName">Полное имя клиента</param>
/// <param name="BirthDate">Дата рождения клиента</param>
public record ClientEditDto(
    string LicenseNumber,
    string FullName,
    DateOnly BirthDate
);