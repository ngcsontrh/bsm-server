using BSM.Framework.Mediator.Abstractions;
using BSM.Framework.MinimalApi;

namespace BSM.Server.Features.Books.CreateBook;

public class CreateBookEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/books", async (IMediator mediator, CreateBookCommand command) =>
        {
            var result = await mediator.Send(command);
            if (result.IsFailure)
            {
                if (result.ValidationErrors != null)
                    return Results.ValidationProblem(result.ValidationErrors);
                return Results.Problem(result.Error);
            }
            return Results.Created();
        })
        .WithTags("Books")
        .WithName("CreateBook");
    }
}
