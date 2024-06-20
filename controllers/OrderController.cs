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



    [Authorize(policy: "User")]
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
        if (orders == null)
        {
            return NotFound();
        }
        return Ok(orders);
    }



    [Authorize(policy: "User")]
    [HttpGet("user")]
    public async Task<IActionResult> GetUserOrders(
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10
    )
    {

        string? username = User.Identity.Name;
        Console.WriteLine(username);
        var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);

        // Console.
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


    [Authorize(policy: "User")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(int id)
    {
            // check if the user is the owner of the order
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized();
        }

        

        OrderModel? order = await _orderService.GetOrderAsync(id); 
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
        // Retrieve the authenticated user's identifier
        if (order == null)
        {
            return BadRequest();
        }

        if ( order.OrderItems.Length == 0)
        {
            return BadRequest("Order must have at least one item");
        }

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            return Unauthorized();
        }
        OrderModel? newOrder = await _orderService.AddOrderAsync(order, userId); 
        if (newOrder == null)
        {
            return BadRequest();
        }

        return Ok("Order added successfully");
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
