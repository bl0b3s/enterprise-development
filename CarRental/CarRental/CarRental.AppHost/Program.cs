var builder = DistributedApplication.CreateBuilder(args);

var sqlServer = builder.AddSqlServer("carrental-sql-server")
                 .AddDatabase("CarRentalDb");

var kafka = builder.AddKafka("carrental-kafka").WithKafkaUI();

builder.AddProject<Projects.CarRental_Api>("carrental-api")
    .WithReference(sqlServer, "DefaultConnection")
    .WithReference(kafka)
    .WithEnvironment("Kafka__RentalTopicName", "rentals")
    .WaitFor(sqlServer)
    .WaitFor(kafka);

builder.AddProject<Projects.CarRental_Generator_Kafka_Host>("carrental-generator-kafka-host")
    .WithReference(kafka)
    .WithEnvironment("Kafka__RentalTopicName", "rentals")
    .WaitFor(kafka);

builder.Build().Run();