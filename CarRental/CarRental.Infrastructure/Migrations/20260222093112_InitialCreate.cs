using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRental.Infrastructure.Migrations;

/// <inheritdoc />
public partial class InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "CarModels",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                DriverType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                SeatingCapacity = table.Column<byte>(type: "tinyint", nullable: false),
                BodyType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CarClass = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CarModels", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Customers",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                DriverLicenseNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Customers", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "ModelGenerations",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ProductionYear = table.Column<int>(type: "int", nullable: false),
                EngineVolumeLiters = table.Column<decimal>(type: "decimal(4,1)", precision: 4, scale: 1, nullable: false),
                TransmissionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                HourlyRate = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                CarModelId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ModelGenerations", x => x.Id);
                table.ForeignKey(
                    name: "FK_ModelGenerations_CarModels_CarModelId",
                    column: x => x.CarModelId,
                    principalTable: "CarModels",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "Cars",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                LicensePlate = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                Color = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                ModelGenerationId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Cars", x => x.Id);
                table.ForeignKey(
                    name: "FK_Cars_ModelGenerations_ModelGenerationId",
                    column: x => x.ModelGenerationId,
                    principalTable: "ModelGenerations",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "Rentals",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                CustomerId = table.Column<int>(type: "int", nullable: false),
                CarId = table.Column<int>(type: "int", nullable: false),
                PickupDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                Hours = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Rentals", x => x.Id);
                table.ForeignKey(
                    name: "FK_Rentals_Cars_CarId",
                    column: x => x.CarId,
                    principalTable: "Cars",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_Rentals_Customers_CustomerId",
                    column: x => x.CustomerId,
                    principalTable: "Customers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Cars_LicensePlate",
            table: "Cars",
            column: "LicensePlate",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Cars_ModelGenerationId",
            table: "Cars",
            column: "ModelGenerationId");

        migrationBuilder.CreateIndex(
            name: "IX_Customers_DriverLicenseNumber",
            table: "Customers",
            column: "DriverLicenseNumber",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_ModelGenerations_CarModelId",
            table: "ModelGenerations",
            column: "CarModelId");

        migrationBuilder.CreateIndex(
            name: "IX_Rentals_CarId_PickupDateTime",
            table: "Rentals",
            columns: new[] { "CarId", "PickupDateTime" });

        migrationBuilder.CreateIndex(
            name: "IX_Rentals_CustomerId_PickupDateTime",
            table: "Rentals",
            columns: new[] { "CustomerId", "PickupDateTime" });

        migrationBuilder.CreateIndex(
            name: "IX_Rentals_PickupDateTime",
            table: "Rentals",
            column: "PickupDateTime");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Rentals");

        migrationBuilder.DropTable(
            name: "Cars");

        migrationBuilder.DropTable(
            name: "Customers");

        migrationBuilder.DropTable(
            name: "ModelGenerations");

        migrationBuilder.DropTable(
            name: "CarModels");
    }
}
