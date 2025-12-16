namespace BSM.Domain.Entities;

public class CartItemEntity
{
    public Guid Id { get; set; }
    public Guid BookId { get; set; }
    public Guid CouponId { get; set; }
    public int Quantity { get; set; }

    public BookEntity Book { get; set; } = null!;
}
