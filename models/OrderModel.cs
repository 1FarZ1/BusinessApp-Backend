
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class OrderModel
{

    [Key]
    public int Id { get; set; }


    [Required]
    public decimal Total { get; set; }

    //orderitems
    [Required]
    public required OrderItemModel[] OrderItems { get; set; }

[DefaultValue("Pending")]
    public string OrderStatus { get; set; } = "Pending";

    [DefaultValue(typeof(DateTime), "")]
    public DateTime OrderDate { get; set; } = DateTime.Now;

    [Required]
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public required UserModel User { get; set; }


       
    
}


// enum status
public enum OrderStatus
{
    Pending,
    Processing,
    Shipped,
    Delivered,
    Cancelled
}
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
    public required OrderModel Order { get; set; }

    [Required]
    public int ProductId { get; set; }

    [ForeignKey("ProductId")]
    public required ProductModel Product { get; set; }

    [Required]
    public decimal Price { get; set; }
}