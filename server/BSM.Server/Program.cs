using BSM.Framework.Mediator;
using BSM.Framework.Mediator.Abstractions;
using BSM.Framework.MinimalApi;
using BSM.Framework.PasswordHasher;
using BSM.Infrastructure.Persistence;
using BSM.Server.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("BSM_DB"));

builder.Services.AddHttpContextAccessor();

builder.Services.AddMediator(typeof(Program).Assembly);
builder.Services.AddEndpoints(typeof(Program).Assembly);

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(BSM.Server.Common.Behaviors.LoggingBehavior<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<>), typeof(BSM.Server.Common.Behaviors.LoggingBehavior<>));

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(BSM.Server.Common.Behaviors.PerformanceBehavior<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<>), typeof(BSM.Server.Common.Behaviors.PerformanceBehavior<>));

builder.Services.AddTransient<IPasswordHasher, PasswordHasher>();

builder.Services.AddOpenApi();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();
app.MapDefaultEndpoints();
app.MapEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
