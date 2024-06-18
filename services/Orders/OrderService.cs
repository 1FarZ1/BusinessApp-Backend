using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;


public class OrderService : IOrderService
{
    private readonly ApplicationDbContext _context;

    public OrderService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OrderModel[]> GetOrdersAsync(int pageIndex, int pageSize)
    {
        var orders = await _context.Orders
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToArrayAsync();
        return orders;
    }

    public async Task<OrderModel?> GetOrderAsync(int id)
    {
        return await _context.Orders.FirstOrDefaultAsync(p => p.Id == id);
    }



    
    public async Task<OrderModel> AddOrderAsync(OrderDto orderDto)
    {   // get the user 

        var order = new OrderModel
        {
                Total = orderDto.Total,
                // order date , date.now
                OrderDate = DateTime.Now,
                // order status1
            
        };

        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task<OrderModel> UpdateOrderAsync(int id, OrderDto order)
    {
        var existingOrder = await _context.Orders.FirstOrDefaultAsync(p => p.Id == id);
        if (existingOrder == null)
        {
            throw new Exception("Order not found");
        }

        existingOrder.Name = order.Name;
        existingOrder.Description = order.Description;
        existingOrder.Price = order.Price;

        await _context.SaveChangesAsync();
        return existingOrder;
    }

    public async Task<bool> DeleteOrderAsync(int id)
    {
        var existingOrder = await _context.Orders.FirstOrDefaultAsync(p => p.Id == id);
        if (existingOrder == null)
        {
            return false;
        }

        _context.Orders.Remove(existingOrder);
        await _context.SaveChangesAsync();
        return true;
    }

    // public async Task<OrderModel[]> SearchOrdersAsync(string query)


}
