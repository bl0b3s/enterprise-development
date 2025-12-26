namespace CarRental.Domain_.Models;

/// <summary>
/// Перечисление типов коробки передач
/// </summary>
public enum TransmissionType
{
    /// <summary>
    /// Механическая коробка передач (МКПП)
    /// </summary>
    Manual = 0,

    /// <summary>
    /// Автоматическая коробка передач (АКПП)
    /// </summary>
    Automatic = 1,

    /// <summary>
    /// Вариатор (бесступенчатая трансмиссия)
    /// </summary>
    Cvt = 2,

    /// <summary>
    /// Робот (автоматизированная механика)
    /// </summary>
    Robot = 3
}
