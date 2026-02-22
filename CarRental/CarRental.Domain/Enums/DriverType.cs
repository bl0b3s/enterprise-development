namespace CarRental.Domain.Enums;

/// <summary>
/// Represents the drivetrain type (which wheels receive power from the engine).
/// This affects vehicle handling, traction, fuel efficiency and suitability for different conditions.
/// </summary>
public enum DriverType
{
    /// <summary>
    /// Front-Wheel Drive (FWD) — power is delivered to the front wheels. 
    /// Common in most modern compact and mid-size cars due to good traction in wet/snowy conditions 
    /// and efficient use of interior space.
    /// </summary>
    FrontWheelDrive,

    /// <summary>
    /// Rear-Wheel Drive (RWD) — power is sent to the rear wheels. 
    /// Provides better handling balance and is preferred in sports cars, 
    /// rear-engine vehicles and many classic/luxury models.
    /// </summary>
    RearWheelDrive,

    /// <summary>
    /// All-Wheel Drive (AWD) — power is distributed to all four wheels permanently or on-demand. 
    /// Offers improved traction and stability, especially in adverse weather, 
    /// without the full complexity of traditional 4×4 systems.
    /// </summary>
    AllWheelDrive,

    /// <summary>
    /// Four-Wheel Drive (4WD / 4×4) — typically a more robust system with selectable modes 
    /// (2WD / 4WD high / 4WD low) and often a transfer case. 
    /// Designed primarily for off-road capability and heavy-duty use.
    /// </summary>
    FourWheelDrive
}