namespace CarRental.Domain_.Models;

/// <summary>
/// Клиент пункта проката
/// </summary>
public class Client
{
    /// <summary>
    /// Уникальный ID клиента
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Номер водительского удостоверения (уникален)
    /// </summary>
    public required string LicenseNumber { get; set; }

    /// <summary>
    /// Полное имя клиента (ФИО)
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// Дата рождения
    /// </summary>
    public required DateOnly DateOfBirth { get; set; }

    /// <summary>
    /// История аренд этого клиента
    /// </summary>
    public List<RentalContract> RentalContracts { get; set; } = new ();
}
