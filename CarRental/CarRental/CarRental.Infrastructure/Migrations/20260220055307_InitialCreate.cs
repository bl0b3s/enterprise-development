using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CarRental.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "car_models",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    drive_type = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    seats_count = table.Column<int>(type: "int", nullable: false),
                    body_type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    @class = table.Column<string>(name: "class", type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_car_models", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "clients",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    license_number = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    full_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    birth_date = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clients", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "model_generations",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    year = table.Column<int>(type: "int", nullable: false),
                    engine_volume = table.Column<double>(type: "float", nullable: false),
                    transmission = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    rental_price_per_hour = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    model_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_model_generations", x => x.id);
                    table.ForeignKey(
                        name: "FK_model_generations_car_models_model_id",
                        column: x => x.model_id,
                        principalTable: "car_models",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cars",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    license_plate = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    color = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    model_generation_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cars", x => x.id);
                    table.ForeignKey(
                        name: "FK_cars_model_generations_model_generation_id",
                        column: x => x.model_generation_id,
                        principalTable: "model_generations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "rentals",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    rental_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    rental_hours = table.Column<int>(type: "int", nullable: false),
                    car_id = table.Column<int>(type: "int", nullable: false),
                    client_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rentals", x => x.id);
                    table.ForeignKey(
                        name: "FK_rentals_cars_car_id",
                        column: x => x.car_id,
                        principalTable: "cars",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_rentals_clients_client_id",
                        column: x => x.client_id,
                        principalTable: "clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "car_models",
                columns: new[] { "id", "body_type", "class", "drive_type", "name", "seats_count" },
                values: new object[,]
                {
                    { 1, "Sedan", "Premium", "RWD", "BMW 3 Series", 5 },
                    { 2, "Coupe", "Sports", "RWD", "Ford Mustang", 4 },
                    { 3, "Sedan", "Compact", "FWD", "Honda Civic", 5 },
                    { 4, "SUV", "Off-road", "4WD", "Jeep Wrangler", 5 },
                    { 5, "Coupe", "Luxury", "RWD", "Porsche 911", 4 },
                    { 6, "SUV", "Full-size", "AWD", "Chevrolet Tahoe", 8 },
                    { 7, "Sedan", "Economy", "FWD", "Lada Vesta", 5 },
                    { 8, "SUV", "Mid-size", "AWD", "Subaru Outback", 5 },
                    { 9, "Van", "Commercial", "RWD", "GAZ Gazelle Next", 3 },
                    { 10, "Hatchback", "Hybrid", "FWD", "Toyota Prius", 5 },
                    { 11, "SUV", "Off-road", "4WD", "UAZ Patriot", 5 },
                    { 12, "SUV", "Premium", "AWD", "Lexus RX", 5 },
                    { 13, "SUV", "Luxury", "AWD", "Range Rover Sport", 5 },
                    { 14, "Sedan", "Premium", "AWD", "Audi A4", 5 },
                    { 15, "SUV", "Off-road", "4WD", "Lada Niva Travel", 5 }
                });

            migrationBuilder.InsertData(
                table: "clients",
                columns: new[] { "id", "birth_date", "full_name", "license_number" },
                values: new object[,]
                {
                    { 1, new DateOnly(1988, 3, 15), "Alexander Smirnov", "2023-001" },
                    { 2, new DateOnly(1992, 7, 22), "Marina Kovalenko", "2022-045" },
                    { 3, new DateOnly(1995, 11, 10), "Denis Popov", "2024-012" },
                    { 4, new DateOnly(1985, 5, 3), "Elena Vasnetsova", "2021-078" },
                    { 5, new DateOnly(1990, 9, 30), "Igor Kozlovsky", "2023-056" },
                    { 6, new DateOnly(1993, 2, 14), "Anna Orlova", "2022-123" },
                    { 7, new DateOnly(1987, 8, 18), "Artem Belov", "2024-034" },
                    { 8, new DateOnly(1994, 12, 25), "Sofia Grigorieva", "2021-099" },
                    { 9, new DateOnly(1991, 6, 7), "Pavel Melnikov", "2023-087" },
                    { 10, new DateOnly(1989, 4, 12), "Olga Zakharova", "2022-067" },
                    { 11, new DateOnly(1996, 10, 28), "Mikhail Tikhonov", "2024-005" },
                    { 12, new DateOnly(1986, 1, 19), "Ksenia Fedorova", "2021-112" },
                    { 13, new DateOnly(1997, 7, 3), "Roman Sokolov", "2023-092" },
                    { 14, new DateOnly(1984, 3, 22), "Tatiana Krylova", "2022-031" },
                    { 15, new DateOnly(1998, 11, 15), "Andrey Davydov", "2024-021" }
                });

            migrationBuilder.InsertData(
                table: "model_generations",
                columns: new[] { "id", "engine_volume", "model_id", "rental_price_per_hour", "transmission", "year" },
                values: new object[,]
                {
                    { 1, 2.0, 1, 2200m, "AT", 2023 },
                    { 2, 5.0, 2, 5000m, "AT", 2022 },
                    { 3, 1.5, 3, 1200m, "CVT", 2024 },
                    { 4, 3.6000000000000001, 4, 2800m, "AT", 2023 },
                    { 5, 3.0, 5, 8000m, "AT", 2024 },
                    { 6, 5.2999999999999998, 6, 3500m, "AT", 2022 },
                    { 7, 1.6000000000000001, 7, 700m, "MT", 2023 },
                    { 8, 2.5, 8, 1800m, "AT", 2024 },
                    { 9, 2.7000000000000002, 9, 1500m, "MT", 2022 },
                    { 10, 1.8, 10, 1600m, "CVT", 2023 },
                    { 11, 2.7000000000000002, 11, 1400m, "MT", 2022 },
                    { 12, 3.5, 12, 3200m, "AT", 2024 },
                    { 13, 3.0, 13, 6000m, "AT", 2023 },
                    { 14, 2.0, 14, 2800m, "AT", 2024 },
                    { 15, 1.7, 15, 900m, "MT", 2023 }
                });

            migrationBuilder.InsertData(
                table: "cars",
                columns: new[] { "id", "color", "license_plate", "model_generation_id" },
                values: new object[,]
                {
                    { 1, "Black", "A001AA163", 1 },
                    { 2, "Red", "B777BC163", 2 },
                    { 3, "White", "C123ET163", 3 },
                    { 4, "Green", "E555KH163", 4 },
                    { 5, "Silver", "K234MR163", 5 },
                    { 6, "Gray", "M888OA163", 6 },
                    { 7, "Blue", "N456RS163", 7 },
                    { 8, "Brown", "O789TU163", 8 },
                    { 9, "White", "P321XO163", 9 },
                    { 10, "Black", "S654AM163", 10 },
                    { 11, "Orange", "T987RE163", 11 },
                    { 12, "White", "U246KN163", 12 },
                    { 13, "Black", "H135VT163", 13 },
                    { 14, "Gray", "SH579SA163", 14 },
                    { 15, "Blue", "SCH864RO163", 15 }
                });

            migrationBuilder.InsertData(
                table: "rentals",
                columns: new[] { "id", "car_id", "client_id", "rental_date", "rental_hours" },
                values: new object[,]
                {
                    { 1, 7, 1, new DateTime(2024, 3, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), 48 },
                    { 2, 7, 3, new DateTime(2024, 2, 25, 14, 30, 0, 0, DateTimeKind.Unspecified), 72 },
                    { 3, 7, 5, new DateTime(2024, 2, 20, 9, 15, 0, 0, DateTimeKind.Unspecified), 24 },
                    { 4, 1, 2, new DateTime(2024, 2, 27, 11, 45, 0, 0, DateTimeKind.Unspecified), 96 },
                    { 5, 1, 4, new DateTime(2024, 2, 25, 16, 0, 0, 0, DateTimeKind.Unspecified), 120 },
                    { 6, 2, 6, new DateTime(2024, 2, 23, 13, 20, 0, 0, DateTimeKind.Unspecified), 72 },
                    { 7, 2, 8, new DateTime(2024, 2, 18, 10, 10, 0, 0, DateTimeKind.Unspecified), 48 },
                    { 8, 3, 7, new DateTime(2024, 2, 28, 8, 30, 0, 0, DateTimeKind.Unspecified), 36 },
                    { 9, 4, 9, new DateTime(2024, 2, 15, 12, 0, 0, 0, DateTimeKind.Unspecified), 96 },
                    { 10, 5, 10, new DateTime(2024, 2, 28, 7, 0, 0, 0, DateTimeKind.Unspecified), 168 },
                    { 11, 6, 11, new DateTime(2024, 2, 22, 15, 45, 0, 0, DateTimeKind.Unspecified), 72 },
                    { 12, 8, 12, new DateTime(2024, 2, 26, 9, 20, 0, 0, DateTimeKind.Unspecified), 48 },
                    { 13, 9, 13, new DateTime(2024, 2, 29, 22, 0, 0, 0, DateTimeKind.Unspecified), 60 },
                    { 14, 10, 14, new DateTime(2024, 2, 24, 11, 30, 0, 0, DateTimeKind.Unspecified), 96 },
                    { 15, 11, 15, new DateTime(2024, 2, 10, 14, 15, 0, 0, DateTimeKind.Unspecified), 120 },
                    { 16, 12, 1, new DateTime(2024, 2, 29, 14, 0, 0, 0, DateTimeKind.Unspecified), 48 },
                    { 17, 13, 2, new DateTime(2024, 2, 5, 16, 45, 0, 0, DateTimeKind.Unspecified), 72 },
                    { 18, 14, 3, new DateTime(2024, 2, 12, 10, 10, 0, 0, DateTimeKind.Unspecified), 36 },
                    { 19, 15, 4, new DateTime(2024, 2, 16, 13, 30, 0, 0, DateTimeKind.Unspecified), 84 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_cars_model_generation_id",
                table: "cars",
                column: "model_generation_id");

            migrationBuilder.CreateIndex(
                name: "IX_model_generations_model_id",
                table: "model_generations",
                column: "model_id");

            migrationBuilder.CreateIndex(
                name: "IX_rentals_car_id",
                table: "rentals",
                column: "car_id");

            migrationBuilder.CreateIndex(
                name: "IX_rentals_client_id",
                table: "rentals",
                column: "client_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "rentals");

            migrationBuilder.DropTable(
                name: "cars");

            migrationBuilder.DropTable(
                name: "clients");

            migrationBuilder.DropTable(
                name: "model_generations");

            migrationBuilder.DropTable(
                name: "car_models");
        }
    }
}
