using BSM.Framework.Mediator.Abstractions;
using BSM.Server.Common.Models;

namespace BSM.Server.Features.Books.CreateBook;

public class CreateBookCommand : IRequest<HandlerResult>
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Author { get; set; }
    public string? Publisher { get; set; }
    public decimal? Price { get; set; }
}
