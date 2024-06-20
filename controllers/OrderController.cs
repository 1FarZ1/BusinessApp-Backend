using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

[Authorize] // Restrict access to authenticated users
[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    // private methode to get the user id from the token
    private string GetUserId()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            throw new Exception("User not found");
        }

        return userId;
    }

    [Authorize(policy: "Admin")]
    [HttpGet("")]
    public async Task<IActionResult> GetOrders(
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10
    )
    {
        GetOrderDto[]? orders = await _orderService.GetOrdersAsync(
            pageIndex, 
            pageSize
            
        );
      
        return Ok(orders);
    }



    [Authorize(policy: "User")]
    [HttpGet("user")]
    public async Task<IActionResult> GetUserOrders(
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10
    )
    {//TODO"error handling
       var userId = GetUserId();

        GetOrderDto[]? orders = await _orderService.GetUserOrdersAsync(
            userId,
            pageIndex,
            pageSize
        );
        
        return Ok(orders);
    }


    [Authorize(policy: "User")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(int id)
    {

        var userId = GetUserId();
        bool isUserOrder = await _orderService.OrderBelongsToUser(id, userId);
        if (!isUserOrder)
        {
            return Unauthorized();
        }

        GetOrderDto? order = await _orderService.GetOrderAsync(id); 
        if (order == null)
        {
            return NotFound();
        }
        return Ok(order);
    }



    [Authorize(policy: "User")]
    [HttpPost]
    public async Task<IActionResult> AddOrder([FromBody] OrderDto order)
    {

        if ( order.OrderItems.Length == 0)
        {
            return BadRequest("Order must have at least one item");
        }

       var userId = GetUserId();
    
        OrderModel? newOrder = await _orderService.AddOrderAsync(order, userId); 
        if (newOrder == null)
        {
            return BadRequest();
        }

        return Ok("Order added successfully");
    }


    [Authorize(policy: "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
       
        bool deleted = await _orderService.DeleteOrderAsync(id); 
        if (!deleted)
        {
            return NotFound();
        }
        return NoContent();
    }

    [Authorize(policy: "Seller")]
    [HttpPut("update-status/{id}")]
    public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] OrderStatus status)
    {   
        var sellerId = GetUserId();
        bool isSeller = await _orderService.OrderBelongsToSeller(id, sellerId);
        if (!isSeller)
        {
            return Unauthorized();
        }

        bool isSuccess = await _orderService.UpdateOrderStatusAsync(id, status); 
        if (!isSuccess)
        {
            throw new Exception("Order status update failed");
        }
        return Ok("Order status updated successfully");
    }
}
