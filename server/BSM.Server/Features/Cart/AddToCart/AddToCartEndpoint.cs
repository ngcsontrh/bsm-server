using BSM.Framework.Mediator.Abstractions;
using BSM.Framework.MinimalApi;

namespace BSM.Server.Features.Cart.AddToCart;

public class AddToCartEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/cart", async (ISender sender, AddToCartCommand command) =>
        {
            var result = await sender.Send(command);
            if (result.IsFailure)
            {
                if (result.ValidationErrors != null)
                {
                    return Results.ValidationProblem(result.ValidationErrors);
                }
                return Results.Problem(result.Error);
            }
            return Results.Ok();
        });
    }
}
