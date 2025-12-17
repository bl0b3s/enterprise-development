using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Domain_.Models;

/// <summary>
/// Модель автомобиля (справочник)
/// </summary>
public class CarModel
{
    /// <summary>
    /// Уникальный идентификатор модели
    /// </summary>
    public required int ModelId { get; set; }

    /// <summary>
    /// Название модели (например, "Toyota Camry")
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Тип привода
    /// </summary>
    public required DriveType DriveType { get; set; }

    /// <summary>
    /// Количество посадочных мест
    /// </summary>
    public required int SeatsCount { get; set; }

    /// <summary>
    /// Тип кузова
    /// </summary>
    public required BodyType BodyType { get; set; }

    /// <summary>
    /// Класс автомобиля
    /// </summary>
    public required CarClass CarClass { get; set; }

    /// <summary>
    /// Коллекция поколений этой модели
    /// </summary>
    public ICollection<ModelGeneration> Generations { get; set; } = new List<ModelGeneration>();
}

