namespace CarRental.Domain.Enums;

/// <summary>
/// Represents the type of vehicle transmission (gearbox), which determines how power is transferred from the engine to the wheels.
/// </summary>
public enum TransmissionType
{
    /// <summary>
    /// Manual transmission — requires the driver to manually shift gears using a clutch pedal and gear stick.
    /// Offers more control and often better fuel efficiency, popular among enthusiasts.
    /// </summary>
    Manual,

    /// <summary>
    /// Automatic transmission — shifts gears automatically without driver input.
    /// Provides convenience and comfort, especially in city driving; modern versions are very efficient.
    /// </summary>
    Automatic,

    /// <summary>
    /// Robotic transmission (automated manual / AMT) — a manual gearbox with computer-controlled clutch and gear shifts.
    /// Usually cheaper than classic automatic, but can have noticeable shift pauses.
    /// </summary>
    Robotic,

    /// <summary>
    /// Continuously Variable Transmission (CVT) — uses a belt/pulley system instead of fixed gears.
    /// Provides seamless acceleration without noticeable gear shifts, often very fuel-efficient.
    /// </summary>
    CVT,

    /// <summary>
    /// Dual-clutch transmission (DCT / DSG / PDK) — uses two separate clutches for odd and even gears.
    /// Combines fast, smooth shifts like a manual with the convenience of an automatic; very popular in performance cars.
    /// </summary>
    DualClutch
}