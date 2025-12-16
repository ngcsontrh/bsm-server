using BSM.Framework.Mediator.Abstractions;
using BSM.Framework.MinimalApi;
using Microsoft.AspNetCore.Mvc;

namespace BSM.Server.Features.Coupons.CreateCoupon;

public class CreateCouponEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/coupons", async (ISender sender, [FromBody] CreateCouponCommand command) =>
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
        .WithTags("Coupons")
        .WithName("CreateCoupon");
    }
}
