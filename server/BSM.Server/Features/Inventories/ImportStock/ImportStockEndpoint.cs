using BSM.Framework.Mediator.Abstractions;
using BSM.Framework.MinimalApi;
using BSM.Server.Features.Inventories.Import;

namespace BSM.Server.Features.Inventories.ImportStock;

public class ImportStockEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/inventories/import-stock", async (ISender sender, ImportStockCommand command) =>
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
        })
        .WithTags("Inventories")
        .WithName("ImportStock");
    }
}
