namespace CarRental.Domain.Enums;

/// <summary>
/// Represents the rental category or car class, which typically affects pricing, features and insurance requirements.
/// </summary>
public enum CarClass
{
    /// <summary>
    /// Economy class — the most affordable option, usually small compact cars with basic features and low fuel consumption.
    /// </summary>
    Economy,

    /// <summary>
    /// Compact class — slightly larger than economy, offering more interior space and better comfort while remaining economical.
    /// </summary>
    Compact,

    /// <summary>
    /// Intermediate class — mid-size vehicles providing a good balance between space, comfort and cost (often called "midsize").
    /// </summary>
    Intermediate,

    /// <summary>
    /// Standard class — full-size sedans or similar vehicles with more legroom and trunk space than intermediate.
    /// </summary>
    Standard,

    /// <summary>
    /// Full-size class — large sedans or crossovers designed for maximum passenger and luggage capacity.
    /// </summary>
    FullSize,

    /// <summary>
    /// Luxury class — premium vehicles with high-end interior materials, advanced technology and superior comfort.
    /// </summary>
    Luxury,

    /// <summary>
    /// Premium class — top-tier luxury vehicles, often including executive sedans, high-performance models or ultra-luxury brands.
    /// </summary>
    Premium,

    /// <summary>
    /// SUV class — sport utility vehicles, usually offering higher seating position, more space and sometimes all-wheel drive.
    /// </summary>
    SUV,

    /// <summary>
    /// Minivan class — family-oriented vehicles with sliding doors, flexible seating configurations and large cargo capacity.
    /// </summary>
    Minivan,

    /// <summary>
    /// Sport class — performance-oriented vehicles with sporty handling, powerful engines and dynamic design.
    /// </summary>
    Sport
}