using BSM.Framework.Mediator.Abstractions;
using BSM.Framework.MinimalApi;

namespace BSM.Server.Features.Inventories.ExportStock;

public class ExportStockEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/inventories/export-stock", async (ISender sender, ExportStockCommand command) =>
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
            .WithName("ExportStock");
    }
}
