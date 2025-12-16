using System.Reflection;
using BSM.Framework.Mediator.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BSM.Framework.Mediator;

public static class MediatorExtensions
{
    public static IServiceCollection AddMediator(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.TryAddScoped<IMediator, Mediator.Implements.Mediator>();
        services.TryAddScoped<ISender>(sp => sp.GetRequiredService<IMediator>());
        services.TryAddScoped<IPublisher>(sp => sp.GetRequiredService<IMediator>());

        if (assemblies.Length == 0)
        {
            assemblies = [Assembly.GetCallingAssembly()];
        }

        var types = assemblies.Distinct().SelectMany(a => a.GetTypes())
            .Where(t => t.IsClass && !t.IsAbstract && !t.IsGenericType)
            .ToList();

        foreach (var type in types)
        {
            var interfaces = type.GetInterfaces();
            foreach (var @interface in interfaces)
            {
                if (@interface.IsGenericType)
                {
                    var genericTypeDef = @interface.GetGenericTypeDefinition();
                    if (genericTypeDef == typeof(IRequestHandler<,>) ||
                        genericTypeDef == typeof(IRequestHandler<>) ||
                        genericTypeDef == typeof(INotificationHandler<>))
                    {
                        services.AddTransient(@interface, type);
                    }
                }
            }
        }

        return services;
    }
}
