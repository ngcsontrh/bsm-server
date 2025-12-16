using BSM.Framework.Mediator.Abstractions;
using BSM.Server.Common.Models;

namespace BSM.Server.Features.Inventories.ExportStock;

public class ExportStockCommand : ICommand<HandlerResult>
{
    public string? Code { get; set; }
    public string? BookCode { get; set; }
    public int? Quantity { get; set; }
    public DateTime? ExportDate { get; set; }
}
