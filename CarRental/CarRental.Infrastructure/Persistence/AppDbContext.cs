using CarRental.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Infrastructure.Persistence;

/// <summary>
/// Entity Framework database context for the Car Rental application.
/// Represents a session with the database and provides access to entity sets.
/// </summary>
public class AppDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the AppDbContext class.
    /// </summary>
    /// <param name="options">The options to be used by the DbContext.</param>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    /// <summary>
    /// Gets or sets the Cars entity set.
    /// </summary>
    public DbSet<Car> Cars { get; set; } = null!;

    /// <summary>
    /// Gets or sets the CarModels entity set.
    /// </summary>
    public DbSet<CarModel> CarModels { get; set; } = null!;

    /// <summary>
    /// Gets or sets the ModelGenerations entity set.
    /// </summary>
    public DbSet<ModelGeneration> ModelGenerations { get; set; } = null!;

    /// <summary>
    /// Gets or sets the Customers entity set.
    /// </summary>
    public DbSet<Customer> Customers { get; set; } = null!;

    /// <summary>
    /// Gets or sets the Rentals entity set.
    /// </summary>
    public DbSet<Rental> Rentals { get; set; } = null!;

    /// <summary>
    /// Configures the model that was discovered by convention from the entity types
    /// exposed in DbSet properties on the derived context.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.DriverLicenseNumber).IsRequired().HasMaxLength(20);
            entity.Property(e => e.FullName).IsRequired().HasMaxLength(150);
            entity.Property(e => e.DateOfBirth).IsRequired();

            entity.HasIndex(e => e.DriverLicenseNumber).IsUnique();
        });

        modelBuilder.Entity<CarModel>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.DriverType).IsRequired().HasConversion<string>();
            entity.Property(e => e.SeatingCapacity).IsRequired();
            entity.Property(e => e.BodyType).IsRequired().HasConversion<string>();
            entity.Property(e => e.CarClass).IsRequired().HasConversion<string>();
        });

        modelBuilder.Entity<ModelGeneration>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.ProductionYear).IsRequired();
            entity.Property(e => e.EngineVolumeLiters).IsRequired().HasPrecision(4, 1);
            entity.Property(e => e.TransmissionType).IsRequired().HasConversion<string>();
            entity.Property(e => e.HourlyRate).IsRequired().HasPrecision(10, 2);
            entity.Property(e => e.CarModelId).IsRequired();

            entity.HasOne<CarModel>()
                  .WithMany()
                  .HasForeignKey(e => e.CarModelId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.LicensePlate).IsRequired().HasMaxLength(12);
            entity.Property(e => e.Color).IsRequired().HasMaxLength(50);
            entity.Property(e => e.ModelGenerationId).IsRequired();

            entity.HasOne<ModelGeneration>()
                  .WithMany()
                  .HasForeignKey(e => e.ModelGenerationId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.LicensePlate).IsUnique();
        });

        modelBuilder.Entity<Rental>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.CustomerId).IsRequired();
            entity.Property(e => e.CarId).IsRequired();
            entity.Property(e => e.PickupDateTime).IsRequired();
            entity.Property(e => e.Hours).IsRequired();

            entity.HasOne<Customer>()
                  .WithMany()
                  .HasForeignKey(e => e.CustomerId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<Car>()
                  .WithMany()
                  .HasForeignKey(e => e.CarId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.PickupDateTime);
            entity.HasIndex(e => new { e.CarId, e.PickupDateTime });
            entity.HasIndex(e => new { e.CustomerId, e.PickupDateTime });
        });
    }
}