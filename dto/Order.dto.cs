using System.ComponentModel.DataAnnotations;

public class OrderDto
{

    [Required]
    public OrderItemDto[] OrderItems { get; set; }
 
}


public class OrderItemDto
{
    [Required]
    public int Quantity { get; set; }
    
    [Required]
    public int ProductId { get; set; }
 
}

public class ChangeStatusOrderDto {

    [Required]
    [EnumDataType(typeof(OrderStatus))]
    public required OrderStatus Status { get; set; }
}
