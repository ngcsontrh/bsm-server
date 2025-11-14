using Scalar.Aspire;

var builder = DistributedApplication.CreateBuilder(args);

var sqlserverPassword = builder.AddParameter("sqlserver-password", true);

var database = builder.AddSqlServer("sqlserver", sqlserverPassword, 50510)
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent)
    .AddDatabase("bsm");

var api = builder.AddProject<Projects.BSM_API>("api")
    .WithReference(database)
    .WaitFor(database);

var scalar = builder.AddScalarApiReference(options =>
    {
        options.WithTheme(ScalarTheme.Laserwave);
        options.PreferHttpsEndpoint();
    })
    .WithApiReference(api);

builder.Build().Run();