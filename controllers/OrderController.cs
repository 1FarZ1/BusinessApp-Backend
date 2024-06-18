using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController( IOrderService orderService)
    { 
        _orderService = orderService;
    }
     

    [HttpGet("")]
    public async Task<IActionResult> GetOrders(
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10
    )
    {
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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(int id)
    {
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
        OrderModel? newOrder = await _orderService.AddOrderAsync(order, 1);
        return CreatedAtAction(nameof(GetOrder), new { id = newOrder.Id }, newOrder);
    }


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

    




}
