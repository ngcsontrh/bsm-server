using System.Collections.Concurrent;
using System.Reflection;
using BSM.Framework.Mediator.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace BSM.Framework.Mediator.Implements;

public class Mediator(IServiceProvider serviceProvider) : IMediator
{
    private static readonly ConcurrentDictionary<Type, MethodInfo> _sendMethodCache = new();
    private static readonly ConcurrentDictionary<Type, MethodInfo> _sendVoidMethodCache = new();

    public async Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
        where TNotification : INotification
    {
        if (notification == null) throw new ArgumentNullException(nameof(notification));

        var notificationType = notification.GetType();
        var handlerType = typeof(INotificationHandler<>).MakeGenericType(notificationType);

        var handlers = serviceProvider.GetServices(handlerType);

        foreach (var handler in handlers)
        {
            var method = handler!.GetType().GetMethod("Handle")!;
            await (Task)method.Invoke(handler, [notification, cancellationToken])!;
        }
    }

    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        var requestType = request.GetType();

        var method = _sendMethodCache.GetOrAdd(requestType, rt =>
            typeof(Mediator).GetMethod(nameof(SendInternal), BindingFlags.NonPublic | BindingFlags.Instance)!
                .MakeGenericMethod(rt, typeof(TResponse))
        );

        return (Task<TResponse>)method.Invoke(this, [request, cancellationToken])!;
    }

    public Task Send(IRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        var requestType = request.GetType();

        var method = _sendVoidMethodCache.GetOrAdd(requestType, rt =>
            typeof(Mediator).GetMethod(nameof(SendVoidInternal), BindingFlags.NonPublic | BindingFlags.Instance)!
                .MakeGenericMethod(rt)
        );

        return (Task)method.Invoke(this, [request, cancellationToken])!;
    }

    private async Task<TResponse> SendInternal<TRequest, TResponse>(TRequest request,
        CancellationToken cancellationToken)
        where TRequest : IRequest<TResponse>
    {
        var handler = serviceProvider.GetRequiredService<IRequestHandler<TRequest, TResponse>>();
        var behaviors = serviceProvider.GetServices<IPipelineBehavior<TRequest, TResponse>>().Reverse();

        RequestHandlerDelegate<TResponse> next = () => handler.Handle(request, cancellationToken);

        foreach (var behavior in behaviors)
        {
            var current = next;
            next = () => behavior.Handle(request, current, cancellationToken);
        }

        return await next();
    }

    private async Task SendVoidInternal<TRequest>(TRequest request, CancellationToken cancellationToken)
        where TRequest : IRequest
    {
        var handler = serviceProvider.GetRequiredService<IRequestHandler<TRequest>>();
        var behaviors = serviceProvider.GetServices<IPipelineBehavior<TRequest>>().Reverse();

        RequestHandlerDelegate next = () => handler.Handle(request, cancellationToken);

        foreach (var behavior in behaviors)
        {
            var current = next;
            next = () => behavior.Handle(request, current, cancellationToken);
        }

        await next();
    }
}
