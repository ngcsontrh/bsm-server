var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.BSM_Server>("bsm-server");

builder.Build().Run();
