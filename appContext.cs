using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : IdentityDbContext<UserModel>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    
    public DbSet<ProductModel> Products { get; set; }
    public DbSet<OrderModel> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder){


            modelBuilder.Entity<ProductModel>(
                entity => {
                    entity.HasKey(p => p.Id);
                    entity.Property(p => p.Id).ValueGeneratedOnAdd();
                    entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
                    entity.Property(p => p.Description).IsRequired().HasMaxLength(500);
                    entity.Property(p => p.Price).IsRequired();
                    entity.Property(p => p.ImageUrl).HasDefaultValue("https://via.placeholder.com/150");
                    entity.HasMany(p => p.OrderItems).WithOne(o => o.Product).HasForeignKey(o => o.ProductId);
                }
            );
            
            modelBuilder.Entity<OrderModel>(
                entity => {
                    entity.HasKey(o => o.Id);
                    entity.Property(o => o.Id).ValueGeneratedOnAdd();
                    entity.Property(o => o.UserId).IsRequired();
                    entity.Property(o => o.Total).IsRequired();
                    entity.Property(o => o.OrderStatus).HasDefaultValue(OrderStatus.Pending);
                    entity.Property(o => o.OrderDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
                    entity.HasOne(o => o.User).WithMany(u => u.Orders).HasForeignKey(o => o.UserId);
                    entity.HasMany(o => o.OrderItems).WithOne(o => o.Order).HasForeignKey(o => o.OrderId);
                }
            );


            modelBuilder.Entity<OrderItemModel>(
                entity => {
                    entity.HasKey(o => o.Id);
                    entity.Property(o => o.Id).ValueGeneratedOnAdd();
                    entity.Property(o => o.Quantity).IsRequired();
                    entity.Property(o => o.ProductId).IsRequired();
                    entity.Property(o => o.OrderId).IsRequired();
                    entity.HasOne(o => o.Product).WithMany(p => p.OrderItems).HasForeignKey(o => o.ProductId);
                    entity.HasOne(o => o.Order).WithMany(o => o.OrderItems).HasForeignKey(o => o.OrderId);
                }
            );
            
            // modelBuilder.Entity<ProductModel>().Property(p => p.PictureUrl).IsRequired();
            // modelBuilder.Entity<ProductModel>().Property(p => p.Category).IsRequired();
            // modelBuilder.Entity<ProductModel>().Property(p => p.Stock).IsRequired();
            // modelBuilder.Entity<ProductModel>().Property(p => p.Status).IsRequired();
            // modelBuilder.Entity<ProductModel>().Property(p => p.CreatedDate).IsRequired();
            // modelBuilder.Entity<ProductModel>().Property(p => p.UpdatedDate).IsRequired();
            // modelBuilder.Entity<OrderModel>().Property(o => o.UserName).IsRequired().HasMaxLength(100);
            // modelBuilder.Entity<OrderModel>().Property(o => o.TotalPrice).IsRequired();
            // modelBuilder.Entity<OrderModel>().Property(o => o.Status).IsRequired();
            // modelBuilder.Entity<OrderModel>().Property(o => o.ShippingAddress).IsRequired().HasMaxLength(500);
            // modelBuilder.Entity<OrderModel>().Property(o => o.CreatedDate).IsRequired();
            // modelBuilder.Entity<OrderModel>().Property(o => o.UpdatedDate).IsRequired();
            // modelBuilder.Entity<OrderItemModel>().Property(o => o.Price).IsRequired();
                base.OnModelCreating(modelBuilder);

        }

}