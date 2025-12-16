namespace BSM.Domain.Entities;

public class StockImportEntity
{
    public Guid Id { get; set; }
    public Guid BookId { get; set; }
    public string Code { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime ImportDate { get; set; }

    public BookEntity Book { get; set; } = null!;
}
