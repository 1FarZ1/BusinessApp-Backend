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

    public async Task<OrderModel[]> GetUserOrdersAsync(string userId, int pageIndex, int pageSize)
    {
        var orders = await _context.Orders
            .Where(p => p.UserId == userId)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToArrayAsync();
        return orders;
    }


public async Task<OrderModel> AddOrderAsync(OrderDto orderDto, string userId)
{
    using var transaction = await _context.Database.BeginTransactionAsync();

    try
    {
        decimal total = 0;
        var orderItems = new List<OrderItemModel>();

        foreach (var item in orderDto.OrderItems)
        {
            var product = await _context.Products.SingleOrDefaultAsync(p => p.Id == item.ProductId);
            if (product == null)
            {
                throw new InvalidOperationException("Product not found");
            }

            total += product.Price * item.Quantity;

            orderItems.Add(new OrderItemModel
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity
            });
        }

        var order = new OrderModel
        {
            OrderItems = orderItems.ToArray(),
            Total = total,
            UserId = userId
        };

        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();

        await transaction.CommitAsync();

        return order;
    }
    catch
    {
        await transaction.RollbackAsync();
        throw;
    }
}

    // public async Task<OrderModel> UpdateOrderAsync(int id, OrderDto order)
    // {
    //     var existingOrder = await _context.Orders.FirstOrDefaultAsync(p => p.Id == id);
    //     if (existingOrder == null)
    //     {
    //         throw new Exception("Order not found");
    //     }

    //     existingOrder.Name = order.Name;
    //     existingOrder.Description = order.Description;
    //     existingOrder.Price = order.Price;

    //     await _context.SaveChangesAsync();
    //     return existingOrder;
    // }

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
