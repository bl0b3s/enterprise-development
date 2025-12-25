namespace CarRental.Domain_.Models;

/// <summary>
/// Перечисление типов коробки передач
/// </summary>
public enum TransmissionType
{
    /// <summary>
    /// Механическая коробка передач (МКПП)
    /// </summary>
    Manual,

    /// <summary>
    /// Автоматическая коробка передач (АКПП)
    /// </summary>
    Automatic,

    /// <summary>
    /// Вариатор (бесступенчатая трансмиссия)
    /// </summary>
    Cvt,

    /// <summary>
    /// Робот (автоматизированная механика)
    /// </summary>
    Robot
}
