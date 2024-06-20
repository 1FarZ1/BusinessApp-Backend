using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

    [HttpGet("")]
    public async Task<IActionResult> GetOrders(
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10
    )
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        // need to vbe admin
        if (userId == null
            || !User.IsInRole("Admin")  ) // Check if the user is an admin 
        {
            return Unauthorized();
        }
        OrderModel[]? orders = await _orderService.GetOrdersAsync(
            pageIndex, 
            pageSize
            
        );
        if (orders == null)
        {
            return NotFound();
        }
        return Ok(orders);
    }

    [HttpGet("user")]
    public async Task<IActionResult> GetUserOrders(
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10
    )
    {

        string? username = User.Identity.Name;
        Console.WriteLine(username);
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Unauthorized();
        }
        OrderModel[]? orders = await _orderService.GetUserOrdersAsync(
            userId,
            pageIndex,
            pageSize
        );
        if (orders == null)
        {
            return NotFound();
        }
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(int id)
    {
        // Retrieve the authenticated user's identifier
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        OrderModel? order = await _orderService.GetOrderAsync(id); 
        if (order == null)
        {
            return NotFound();
        }
        return Ok(order);
    }

    [HttpPost]
    public async Task<IActionResult> AddOrder([FromBody] OrderDto order)
    {
        // Retrieve the authenticated user's identifier
        if (order == null)
        {
            return BadRequest();
        }

        if ( order.OrderItems.Length == 0)
        {
            return BadRequest("Order must have at least one item");
        }


        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Unauthorized();
        }
        OrderModel? newOrder = await _orderService.AddOrderAsync(order, userId); 
        return CreatedAtAction(nameof(GetOrder), new { id = newOrder.Id }, newOrder);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        // Retrieve the authenticated user's identifier
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        bool deleted = await _orderService.DeleteOrderAsync(id); 
        if (!deleted)
        {
            return NotFound();
        }
        return NoContent();
    }
}
