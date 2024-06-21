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

    public DbSet<OrderItemModel> OrderItems { get; set; }

    public DbSet<CategoryModel> Categories { get; set; }

    public DbSet<SubCategoryModel> SubCategories { get; set; }

    public DbSet<UserFavoriteModel> UserFavorites { get; set; }

       protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies(); // Enable lazy loading proxies
        base.OnConfiguring(optionsBuilder);
    }

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
                    entity.ToTable("OrderItems");
                    entity.HasKey(o => o.Id);
                    entity.Property(o => o.Id).ValueGeneratedOnAdd();
                    entity.Property(o => o.Quantity).IsRequired();
                    entity.Property(o => o.ProductId).IsRequired();
                    entity.Property(o => o.OrderId).IsRequired();
                    entity.HasOne(o => o.Product).WithMany(p => p.OrderItems).HasForeignKey(o => o.ProductId);
                    entity.HasOne(o => o.Order).WithMany(o => o.OrderItems).HasForeignKey(o => o.OrderId);
                }
            );

            modelBuilder.Entity<CategoryModel>(
                entity => {
                    entity.HasKey(c => c.Id);
                    entity.Property(c => c.Id).ValueGeneratedOnAdd();
                    entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
                    entity.Property(c => c.Description).IsRequired().HasMaxLength(500);
                    entity.Property(c => c.ImageUrl).HasDefaultValue("https://via.placeholder.com/150");
                    entity.HasMany(c => c.SubCategories).WithOne(s => s.Category).HasForeignKey(s => s.CategoryId);
                }
            );

            modelBuilder.Entity<SubCategoryModel>(
                entity => {
                    entity.HasKey(s => s.Id);
                    entity.Property(s => s.Id).ValueGeneratedOnAdd();
                    entity.Property(s => s.Name).IsRequired().HasMaxLength(100);
                    entity.Property(s => s.Description).IsRequired().HasMaxLength(500);
                    entity.Property(s => s.ImageUrl).HasDefaultValue("https://via.placeholder.com/150");
                    entity.Property(s => s.CategoryId).IsRequired();
                    entity.HasOne(s => s.Category).WithMany(c => c.SubCategories).HasForeignKey(s => s.CategoryId);
                    entity.HasMany(s => s.Products).WithOne(p => p.SubCategory).HasForeignKey(p => p.SubCategoryId);
                }
            );

            modelBuilder.Entity<UserFavoriteModel>(
                entity => {
                    entity.HasKey(u => u.Id);
                    entity.Property(u => u.Id).ValueGeneratedOnAdd();
                    entity.Property(u => u.UserId).IsRequired();
                    entity.Property(u => u.ProductId).IsRequired();
                    entity.HasOne(u => u.User).WithMany(u => u.UserFavorites).HasForeignKey(u => u.UserId);
                    entity.HasOne(u => u.Product).WithMany(p => p.UserFavorites).HasForeignKey(u => u.ProductId);
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