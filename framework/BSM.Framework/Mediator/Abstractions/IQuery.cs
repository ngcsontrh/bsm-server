namespace BSM.Framework.Mediator.Abstractions;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}
