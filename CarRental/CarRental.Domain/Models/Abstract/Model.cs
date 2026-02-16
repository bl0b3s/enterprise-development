namespace CarRental.Domain.Models.Abstract;

/// <summary>
/// Base class for all entities with an identifier
/// </summary>
public abstract class Model
{
    /// <summary>
    /// identifier of the entity
    /// </summary>
    public virtual int Id { get; set; }
}