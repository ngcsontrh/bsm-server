namespace BSM.Domain.Entities;

public class OrderEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public UserEntity User { get; set; } = null!;
}
