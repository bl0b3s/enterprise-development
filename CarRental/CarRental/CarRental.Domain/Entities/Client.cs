using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRental.Domain.Entities;

/// <summary>
/// Клиент пункта проката
/// </summary>
[Table("clients")]
public class Client
{
    /// <summary>
    /// Уникальный ID клиента
    /// </summary>
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// Номер водительского удостоверения (уникален)
    /// </summary>
    [Column("license_number")]
    [MaxLength(20)]
    public required string LicenseNumber { get; set; }

    /// <summary>
    /// Полное имя клиента (ФИО)
    /// </summary>
    [Column("full_name")]
    [MaxLength(100)]
    public required string FullName { get; set; }

    /// <summary>
    /// Дата рождения
    /// </summary>
    [Column("birth_date")]
    public required DateOnly BirthDate { get; set; }
}