using CarRental.Application.Contracts.Dto;
using CarRental.Generator.Kafka.Host;
using CarRental.Generator.Kafka.Host.Serializers;
using Confluent.Kafka;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddKafkaProducer<Guid, IList<RentalEditDto>>("carrental-kafka",
    configureBuilder: builder =>
    {
        builder.SetKeySerializer(new GuidKeySerializer());
        builder.SetValueSerializer(new RentalValueSerializer());
    },
    configureSettings: settings =>
    {
        settings.Config.Acks = Acks.All;
    }
);

builder.Services.AddScoped<RentalKafkaProducer>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var assemblies = AppDomain.CurrentDomain.GetAssemblies()
        .Where(a => a.GetName().Name!.StartsWith("CarRental"))
        .Distinct();

    foreach (var assembly in assemblies)
    {
        var xmlFile = $"{assembly.GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        if (File.Exists(xmlPath))
            c.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
