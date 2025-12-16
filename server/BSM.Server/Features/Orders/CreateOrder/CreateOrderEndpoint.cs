using BSM.Framework.Mediator.Abstractions;
using BSM.Framework.Mediator.Implements;
using BSM.Framework.MinimalApi;

namespace BSM.Server.Features.Orders.CreateOrder;

public class CreateOrderEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/order/create", async (ISender sender, CreateOrderCommand command) =>
        {
            var result = await sender.Send(command);
            if (result.IsFailure)
            {
                if (result.ValidationErrors != null)
                    return Results.ValidationProblem(result.ValidationErrors);
                return Results.Problem(result.Error);
            }
            return Results.Created();
        })
        .WithName("Create Order")
        .WithTags("Order");
    }
}
