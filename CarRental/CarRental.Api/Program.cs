using CarRental.Application.Dtos.CarModels;
using CarRental.Application.Dtos.Cars;
using CarRental.Application.Dtos.Customers;
using CarRental.Application.Dtos.ModelGenerations;
using CarRental.Application.Dtos.Rentals;
using CarRental.Application.Profiles;
using CarRental.Application.Services;
using CarRental.Infrastructure.Data;
using CarRental.Infrastructure.Data.Interfaces;
using CarRental.Infrastructure.Persistence;
using CarRental.Infrastructure.Repositories;
using CarRental.Infrastructure.Repositories.Interfaces;
using CarRental.ServiceDefaults;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddSqlServerDbContext<AppDbContext>("carrentaldb", configureDbContextOptions: opt =>
{
    opt.UseLazyLoadingProxies();
});

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});

builder.Services.AddControllers().AddJsonOptions(opts =>
{
    opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAutoMapper(config =>
{
    config.AddProfile(new MappingProfile());
});

builder.Services.AddSingleton<CarRental.Domain.Data.DataSeed>();
builder.Services.AddScoped<IDataSeeder, EfDataSeeder>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<ICrudService<CarResponseDto, CarCreateDto, CarUpdateDto>, CarService>();
builder.Services.AddScoped<ICrudService<CustomerResponseDto, CustomerCreateDto, CustomerUpdateDto>, CustomerService>();
builder.Services.AddScoped<ICrudService<CarModelResponseDto, CarModelCreateDto, CarModelUpdateDto>, CarModelService>();
builder.Services.AddScoped<ICrudService<ModelGenerationResponseDto, ModelGenerationCreateDto, ModelGenerationUpdateDto>, ModelGenerationService>();
builder.Services.AddScoped<ICrudService<RentalResponseDto, RentalCreateDto, RentalUpdateDto>, RentalService>();

builder.Services.AddScoped<IAnalyticQueryService, AnalyticQueryService>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Car Rental API",
        Version = "v1",
        Description = "Car Rental Management System API"
    });

    var basePath = AppContext.BaseDirectory;
    c.IncludeXmlComments(Path.Combine(basePath, "CarRental.Api.xml"));
    c.IncludeXmlComments(Path.Combine(basePath, "CarRental.Application.xml"));
    c.IncludeXmlComments(Path.Combine(basePath, "CarRental.Domain.xml"));
});

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using var migrationScope = app.Services.CreateScope();
var context = migrationScope.ServiceProvider.GetRequiredService<AppDbContext>();
var logger = migrationScope.ServiceProvider.GetRequiredService<ILogger<Program>>();

try
{
    logger.LogInformation("Applying database migrations...");
    await context.Database.MigrateAsync();
    logger.LogInformation("Migrations applied successfully");
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occurred during database migrations");
    throw;
}

using var seedingScope = app.Services.CreateScope();
var seeder = seedingScope.ServiceProvider.GetRequiredService<IDataSeeder>();
var seedingLogger = seedingScope.ServiceProvider.GetRequiredService<ILogger<Program>>();

try
{
    seedingLogger.LogInformation("Seeding database...");
    await seeder.SeedAsync();
    seedingLogger.LogInformation("Database seeded successfully");
}
catch (Exception ex)
{
    seedingLogger.LogError(ex, "An error occurred during database seeding");
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();