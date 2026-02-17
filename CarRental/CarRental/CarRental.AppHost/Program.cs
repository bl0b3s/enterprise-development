using Aspire.Hosting;
using Aspire.Hosting.SqlServer;

var builder = DistributedApplication.CreateBuilder(args);


var sqlServer = builder.AddSqlServer("bikerental-sql-server")
                 .AddDatabase("BikeRentalDb");

var api = builder.AddProject<Projects.CarRental_Api>("carrental-api")
    .WithReference(sqlServer, "DefaultConnection")
    .WaitFor(sqlServer);

builder.Configuration["ASPIRE_DASHBOARD_UNSECURED_ALLOW_ANONYMOUS"] = "true";

builder.Build().Run();