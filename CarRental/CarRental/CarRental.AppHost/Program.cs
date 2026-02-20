var builder = DistributedApplication.CreateBuilder(args);


var sqlServer = builder.AddSqlServer("carrental-sql-server")
                 .AddDatabase("CarRentalDb");

var api = builder.AddProject<Projects.CarRental_Api>("carrental-api")
    .WithReference(sqlServer, "DefaultConnection")
    .WaitFor(sqlServer);

builder.Build().Run();