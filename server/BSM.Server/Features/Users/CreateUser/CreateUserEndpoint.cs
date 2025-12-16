using BSM.Framework.Mediator.Abstractions;
using BSM.Framework.MinimalApi;
using Microsoft.AspNetCore.Mvc;

namespace BSM.Server.Features.Users.CreateUser;

public class CreateUserEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/users", async (ISender sender, [FromBody] CreateUserCommand command) =>
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
        .WithTags("Users")
        .WithName("CreateUser");
    }
}
