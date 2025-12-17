using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Domain_.Models;

/// <summary>
/// Перечисление классов автомобилей
/// </summary>
public enum CarClass
{
    /// <summary>
    /// Эконом класс (дешевле, простые машины)
    /// </summary>
    Economy,

    /// <summary>
    /// Средний класс (среднее качество и цена)
    /// </summary>
    Middle,

    /// <summary>
    /// Премиум класс (люксовые машины)
    /// </summary>
    Premium
}
