var builder = DistributedApplication.CreateBuilder(args);

var password = builder.AddParameter("DatabasePassword", secret: true);

var carrentalDb = builder.AddSqlServer("sqlserver", password: password)
                         .AddDatabase("carrentaldb");

builder.AddProject<Projects.CarRental_Api>("carrental-api")
       .WithReference(carrentalDb)
       .WithExternalHttpEndpoints()
       .WaitFor(carrentalDb);

var consumer = builder.AddProject<Projects.CarRental_Consumer>("grpc-consumer")
    .WithReference(carrentalDb)
    .WaitFor(carrentalDb);

builder.AddProject<Projects.CarRental_Producer>("grpc-producer")
    .WaitFor(carrentalDb)
    .WaitFor(consumer);

builder.Build().Run();