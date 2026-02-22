namespace CarRental.Domain.Enums;

/// <summary>
/// Represents the body style or body type of a vehicle.
/// </summary>
public enum BodyType
{
    /// <summary>
    /// Four-door passenger car with a separate trunk (saloon).
    /// </summary>
    Sedan,

    /// <summary>
    /// Compact car with a rear door that opens upwards, giving access to the cargo area.
    /// </summary>
    Hatchback,

    /// <summary>
    /// Similar to a sedan but with a fastback rear end and a hatch-like tailgate.
    /// </summary>
    Liftback,

    /// <summary>
    /// Two-door car with a fixed roof and a sporty design, usually with limited rear seating.
    /// </summary>
    Coupe,

    /// <summary>
    /// Car with a retractable or removable roof (soft-top or hard-top convertible).
    /// </summary>
    Convertible,

    /// <summary>
    /// Sport Utility Vehicle — taller vehicle with off-road capability and higher ground clearance.
    /// </summary>
    SUV,

    /// <summary>
    /// Crossover Utility Vehicle — combines features of a passenger car and an SUV (usually unibody construction).
    /// </summary>
    Crossover,

    /// <summary>
    /// Minivan — family-oriented vehicle with sliding doors and flexible seating arrangements.
    /// </summary>
    Minivan,

    /// <summary>
    /// Pickup truck — vehicle with an open cargo bed at the rear, often used for hauling.
    /// </summary>
    Pickup,

    /// <summary>
    /// Station wagon — passenger car with an extended roofline and a rear door that opens upwards (estate car).
    /// </summary>
    StationWagon
}