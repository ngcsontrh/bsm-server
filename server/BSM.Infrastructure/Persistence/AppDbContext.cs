using BSM.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BSM.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<BookEntity> Books { get; set; }
    public DbSet<InventoryEntity> Inventories { get; set; }
    public DbSet<OrderEntity> Orders { get; set; }
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<CouponEntity> Coupons { get; set; }
    public DbSet<CartItemEntity> CartItems { get; set; }
    public DbSet<StockImportEntity> StockImports { get; set; }
    public DbSet<StockExportEntity> StockExports { get; set; }
    public DbSet<CustomerEntity> Customers { get; set; }
}
