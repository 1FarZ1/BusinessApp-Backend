using System.ComponentModel.DataAnnotations;

public class OrderDto
{

    

    [Required]
    public string Status { get; set; }
    
    [Required]
    public decimal Total { get; set; }
    

    [Required]
    public OrderItemDto[] OrderItems { get; set; }
 
}


public class OrderItemDto
{
    [Required]
    public int Quantity { get; set; }
    
    [Required]
    public int ProductId { get; set; }
    
    [Required]
    public int OrderId { get; set; }
    
 
}