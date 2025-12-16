namespace BSM.Domain.Entities;

public class InventoryEntity
{
    public Guid Id { get; set; }
    public Guid BookId { get; set; }
    public int Quantity { get; set; }
    public BookEntity Book { get; set; } = null!;
}
