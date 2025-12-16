using BSM.Framework.Mediator.Abstractions;
using BSM.Server.Common.Models;

namespace BSM.Server.Features.Inventories.Import;

public class ImportStockCommand : ICommand<HandlerResult>
{
    public string? Code { get; set; }
    public string? BookCode { get; set; }
    public int? Quantity { get; set; }
    public DateTime? ImportDate { get; set; }
}
