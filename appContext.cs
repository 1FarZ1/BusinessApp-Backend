using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // products
    public DbSet<ProductModel> Products { get; set; }

    // orders
    public DbSet<OrderModel> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder){
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductModel>().ToTable("Products");
            modelBuilder.Entity<ProductModel>().HasKey(p => p.Id);
            modelBuilder.Entity<ProductModel>().Property(p => p.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<ProductModel>().Property(p => p.Name).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<ProductModel>().Property(p => p.Description).IsRequired().HasMaxLength(500);
            modelBuilder.Entity<ProductModel>().Property(p => p.Price).IsRequired();
            modelBuilder.Entity<ProductModel>().Property(p => p.ImageUrl).IsRequired();
            modelBuilder.Entity<ProductModel>().HasMany(p => p.OrderItems).WithOne(o => o.Product).HasForeignKey(o => o.ProductId);

            // modelBuilder.Entity<ProductModel>().Property(p => p.PictureUrl).IsRequired();
            // modelBuilder.Entity<ProductModel>().Property(p => p.Category).IsRequired();
            // modelBuilder.Entity<ProductModel>().Property(p => p.Stock).IsRequired();
            // modelBuilder.Entity<ProductModel>().Property(p => p.Status).IsRequired();
            // modelBuilder.Entity<ProductModel>().Property(p => p.CreatedDate).IsRequired();
            // modelBuilder.Entity<ProductModel>().Property(p => p.UpdatedDate).IsRequired();

            modelBuilder.Entity<OrderModel>().ToTable("Orders");
            modelBuilder.Entity<OrderModel>().HasKey(o => o.Id);
            modelBuilder.Entity<OrderModel>().Property(o => o.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<OrderModel>().Property(o => o.UserId).IsRequired();
            modelBuilder.Entity<OrderModel>().Property(o => o.Total).IsRequired();
            modelBuilder.Entity<OrderModel>().Property(o => o.OrderStatus).HasDefaultValue(OrderStatus.Pending);
            modelBuilder.Entity<OrderModel>().Property(o => o.OrderDate).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<OrderModel>().HasOne(o => o.User).WithMany(u => u.Orders).HasForeignKey(o => o.UserId);



        


            // modelBuilder.Entity<OrderModel>().Property(o => o.UserName).IsRequired().HasMaxLength(100);
            // modelBuilder.Entity<OrderModel>().Property(o => o.TotalPrice).IsRequired();
            // modelBuilder.Entity<OrderModel>().Property(o => o.Status).IsRequired();
            // modelBuilder.Entity<OrderModel>().Property(o => o.ShippingAddress).IsRequired().HasMaxLength(500);
            // modelBuilder.Entity<OrderModel>().Property(o => o.CreatedDate).IsRequired();
            // modelBuilder.Entity<OrderModel>().Property(o => o.UpdatedDate).IsRequired();


            //order item mode
            modelBuilder.Entity<OrderItemModel>().ToTable("OrderItems");
            modelBuilder.Entity<OrderItemModel>().HasKey(o => o.Id);
            modelBuilder.Entity<OrderItemModel>().Property(o => o.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<OrderItemModel>().Property(o => o.Quantity).IsRequired();
            modelBuilder.Entity<OrderItemModel>().Property(o => o.Price).IsRequired();
            modelBuilder.Entity<OrderItemModel>().Property(o => o.ProductId).IsRequired();
            modelBuilder.Entity<OrderItemModel>().Property(o => o.OrderId).IsRequired();
            modelBuilder.Entity<OrderItemModel>().HasOne(o => o.Product).WithMany(p => p.OrderItems).HasForeignKey(o => o.ProductId);
            modelBuilder.Entity<OrderItemModel>().HasOne(o => o.Order).WithMany(o => o.OrderItems).HasForeignKey(o => o.OrderId);
    
        }

}