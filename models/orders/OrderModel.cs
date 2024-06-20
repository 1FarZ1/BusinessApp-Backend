
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public enum OrderStatus
{
    Pending,
    Processing,
    Shipped,
    Delivered,
    Cancelled
}
public class OrderModel
{

    [Key]
    public int Id { get; set; }


    [Required]
    public decimal Total { get; set; }

[DefaultValue(OrderStatus.Pending)]
    public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;

    [DefaultValue(typeof(DateTime), "")]
    public DateTime OrderDate { get; set; } = DateTime.Now;

    [Required]
    public required string UserId { get; set; }

    [ForeignKey("UserId")]
    public  virtual UserModel User { get; set; }


    public virtual ICollection<OrderItemModel> OrderItems { get; set; } // Virtual navigation property



}


// enum status

public class OrderItemModel
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Range(1, 1000)]
    [DefaultValue(1)]
    public int Quantity { get; set; }

    [Required]
    public int OrderId { get; set; }

    [ForeignKey("OrderId")]
    public virtual OrderModel Order { get; set; } // Virtual navigation property


    [Required]
    public int ProductId { get; set; }

    [ForeignKey("ProductId")]
    public virtual ProductModel Product { get; set; } // Virtual navigation property

    // [Required]
    // public decimal Price { get; set; }
}