using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;



public class GetOrderDto {
    // id , totoal , status , user , orderItems, number of products in order
    public int Id { get; set;}
    public decimal Total { get; set;}
    public OrderStatus Status { get; set;}
    public string username { get; set;}

    public DateTime OrderDate { get; set;}

    public int NumberOfProducts { get; set;}
}

public class OrderService : IOrderService
{
    private readonly ApplicationDbContext _context;

    public OrderService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GetOrderDto[]> GetOrdersAsync(int pageIndex, int pageSize)
    {
         GetOrderDto[]?  orders= await _context.Orders
            .Select(p => new GetOrderDto
            {
                Id = p.Id,
                Total = p.Total,
                Status = (OrderStatus)p.OrderStatus,
                username = p.User.UserName,
                OrderDate = p.OrderDate,
                NumberOfProducts = p.OrderItems.Count
            })
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToArrayAsync();

            for (int i = 0; i < orders.Length; i++)
            {
                    Console.WriteLine(orders[i].username);
            }
        return orders;
    }

    public async Task<OrderModel?> GetOrderAsync(int id)
    {
        return await _context.Orders.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<GetOrderDto[]> GetUserOrdersAsync(string userId, int pageIndex, int pageSize)
    {
        GetOrderDto[]? orders = await _context.Orders
            .Where(p => p.UserId == userId)
            .Select(p => new GetOrderDto
            {
                Id = p.Id,
                Total = p.Total,
                Status = (OrderStatus)p.OrderStatus,
                username = p.User.UserName,
                OrderDate = p.OrderDate,
                NumberOfProducts = p.OrderItems.Count
            })
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

    public async Task<bool> OrderBelongsToUser(int orderId, string userId)
    {
        return await _context.Orders.AnyAsync(p => p.Id == orderId && p.UserId == userId);
    }

    public async Task<bool> OrderBelongsToSeller(int orderId, string sellerId)
    {   
        //TODO
        return false;
        // return await _context.Orders.AnyAsync(p => p.Id == orderId && p.User.SellerId == sellerId);
    }


    public async Task<bool> UpdateOrderStatusAsync(int id, OrderStatus status)
    {
        var existingOrder = await _context.Orders.FirstOrDefaultAsync(p => p.Id == id);
        if (existingOrder == null)
        {
            throw new Exception("Order not found");
        }

        existingOrder.OrderStatus = status;

        await _context.SaveChangesAsync();
        if (existingOrder.OrderStatus == status)
        {
            return true;
        }
        return false;
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
