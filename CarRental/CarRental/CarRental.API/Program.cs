using CarRental.Application.Contracts;
using CarRental.Application.Contracts.Dto;
using CarRental.Domain.Data;
using CarRental.Domain.Entities;
using CarRental.Domain.Interfaces;
using CarRental.Infrastructure.Kafka;
using CarRental.Infrastructure.Kafka.Deserializers;
using CarRental.Infrastructure.Persistence;
using CarRental.Infrastructure.Repositories;
using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddSingleton<CarRentalFixture>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var basePath = AppContext.BaseDirectory;

    var xmlApiPath = Path.Combine(basePath, "CarRental.Api.xml");
    if (File.Exists(xmlApiPath))
    {
        c.IncludeXmlComments(xmlApiPath, includeControllerXmlComments: true);
    }

    var xmlContractsPath = Path.Combine(basePath, "CarRental.Application.Contracts.xml");
    if (File.Exists(xmlContractsPath))
    {
        c.IncludeXmlComments(xmlContractsPath);
    }

    var xmlDomainPath = Path.Combine(basePath, "CarRental.Domain.xml");
    if (File.Exists(xmlDomainPath))
    {
        c.IncludeXmlComments(xmlDomainPath);
    }
});

builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IRepository<Car>, DbRepository<Car>>();
builder.Services.AddScoped<IRepository<Client>, DbRepository<Client>>();
builder.Services.AddScoped<IRepository<CarModel>, DbRepository<CarModel>>();
builder.Services.AddScoped<IRepository<ModelGeneration>, DbRepository<ModelGeneration>>();
builder.Services.AddScoped<IRepository<Rental>, DbRepository<Rental>>();

builder.Services.AddHostedService<RentalKafkaConsumer>();

builder.AddKafkaConsumer<Guid, IList<RentalEditDto>>("car-rental-kafka",
    configureBuilder: builder =>
    {
        builder.SetKeyDeserializer(new GuidKeyDeserializer());
        builder.SetValueDeserializer(new RentalValueDeserializer());
    },
    configureSettings: settings =>
    {
        settings.Config.GroupId = "rental-consumer";
        settings.Config.AutoOffsetReset = AutoOffsetReset.Earliest;
    }
);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Car Rental API"));
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapDefaultEndpoints();
app.MapControllers();
app.Run();