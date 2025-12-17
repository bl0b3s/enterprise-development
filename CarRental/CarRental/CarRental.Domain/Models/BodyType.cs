using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Domain_.Models;

/// <summary>
/// Перечисление типов кузова
/// </summary>
public enum BodyType
{
    /// <summary>
    /// Седан (4-5 мест, закрытый)
    /// </summary>
    Sedan,

    /// <summary>
    /// Хэтчбек (компактный, спортивный)
    /// </summary>
    Hatchback,

    /// <summary>
    /// Купе (спортивный, 2-3 места)
    /// </summary>
    Coupe,

    /// <summary>
    /// Внедорожник (SUV, повышенная проходимость)
    /// </summary>
    SUV,

    /// <summary>
    /// Универсал (седан с большим багажником)
    /// </summary>
    Wagon,

    /// <summary>
    /// Пикап (с открытым кузовом сзади)
    /// </summary>
    Pickup
}
