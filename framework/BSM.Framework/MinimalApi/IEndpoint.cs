using Microsoft.AspNetCore.Routing;

namespace BSM.Framework.MinimalApi;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}
