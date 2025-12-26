namespace CarRental.Domain_.Models;

/// <summary>
/// Перечисление типов кузова
/// </summary>
public enum BodyType
{
    /// <summary>
    /// Седан (4-5 мест, закрытый)
    /// </summary>
    Sedan = 0,

    /// <summary>
    /// Хэтчбек (компактный, спортивный)
    /// </summary>
    Hatchback = 1,

    /// <summary>
    /// Купе (спортивный, 2-3 места)
    /// </summary>
    Coupe = 2,

    /// <summary>
    /// Внедорожник (SUV, повышенная проходимость)
    /// </summary>
    Suv = 3,

    /// <summary>
    /// Универсал (седан с большим багажником)
    /// </summary>
    Wagon = 4,

    /// <summary>
    /// Пикап (с открытым кузовом сзади)
    /// </summary>
    Pickup = 5
}
