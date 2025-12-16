using System.Diagnostics;
using BSM.Framework.Mediator.Abstractions;

namespace BSM.Server.Common.Behaviors;

public class PerformanceBehavior<TRequest, TResponse>(ILogger<PerformanceBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var timer = Stopwatch.StartNew();
        var requestName = typeof(TRequest).Name;

        try
        {
            return await next();
        }
        finally
        {
            timer.Stop();
            var elapsedMilliseconds = timer.ElapsedMilliseconds;

            if (elapsedMilliseconds > 500)
            {
                logger.LogWarning("Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@Request}",
                    requestName, elapsedMilliseconds, request);
            }
            else
            {
                logger.LogInformation("Request: {Name} completed in {ElapsedMilliseconds} ms", requestName,
                    elapsedMilliseconds);
            }
        }
    }
}

public class PerformanceBehavior<TRequest>(ILogger<PerformanceBehavior<TRequest>> logger)
    : IPipelineBehavior<TRequest>
    where TRequest : IRequest
{
    public async Task Handle(TRequest request, RequestHandlerDelegate next, CancellationToken cancellationToken)
    {
        var timer = Stopwatch.StartNew();
        var requestName = typeof(TRequest).Name;

        try
        {
            await next();
        }
        finally
        {
            timer.Stop();
            var elapsedMilliseconds = timer.ElapsedMilliseconds;

            if (elapsedMilliseconds > 500)
            {
                logger.LogWarning("Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@Request}",
                    requestName, elapsedMilliseconds, request);
            }
            else
            {
                logger.LogInformation("Request: {Name} completed in {ElapsedMilliseconds} ms", requestName,
                    elapsedMilliseconds);
            }
        }
    }
}
