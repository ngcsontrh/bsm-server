using BSM.Framework.Mediator.Abstractions;

namespace BSM.Server.Common.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        logger.LogInformation("Starting Request: {Name} {@Request}", requestName, request);

        var response = await next();

        logger.LogInformation("Completed Request: {Name}", requestName);

        return response;
    }
}

public class LoggingBehavior<TRequest>(ILogger<LoggingBehavior<TRequest>> logger)
    : IPipelineBehavior<TRequest>
    where TRequest : IRequest
{
    public async Task Handle(TRequest request, RequestHandlerDelegate next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        logger.LogInformation("Starting Request: {Name} {@Request}", requestName, request);

        await next();

        logger.LogInformation("Completed Request: {Name}", requestName);
    }
}
