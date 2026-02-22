using CarRental.Application.Dtos.CarModels;
using CarRental.Application.Dtos.Cars;
using CarRental.Application.Dtos.Customers; 
using CarRental.Application.Dtos.ModelGenerations;
using CarRental.Application.Dtos.Rentals;
using CarRental.Application.Profiles;
using CarRental.Application.Services;
using CarRental.Customer.Services;
using CarRental.Infrastructure.Persistence;
using CarRental.Infrastructure.Repositories;
using CarRental.Infrastructure.Repositories.Interfaces;
using CarRental.ServiceDefaults;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("carrentaldb")
        ?? throw new InvalidOperationException("Connection string 'carrentaldb' not found.");

    options.UseSqlServer(connectionString);
    options.UseLazyLoadingProxies();
});

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ICrudService<CarModelResponseDto, CarModelCreateDto, CarModelUpdateDto>, CarModelService>();
builder.Services.AddScoped<ICrudService<ModelGenerationResponseDto, ModelGenerationCreateDto, ModelGenerationUpdateDto>, ModelGenerationService>();
builder.Services.AddScoped<ICrudService<CarResponseDto, CarCreateDto, CarUpdateDto>, CarService>();
builder.Services.AddScoped<ICrudService<CustomerResponseDto, CustomerCreateDto, CustomerUpdateDto>, CustomerService>();
builder.Services.AddScoped<ICrudService<RentalResponseDto, RentalCreateDto, RentalUpdateDto>, RentalService>();

builder.Services.AddAutoMapper(config =>
{
    config.AddProfile(new MappingProfile());
});

builder.Services.AddGrpc();

var app = builder.Build();
app.MapDefaultEndpoints();
app.MapGrpcService<RequestStreamingService>();
app.Run();