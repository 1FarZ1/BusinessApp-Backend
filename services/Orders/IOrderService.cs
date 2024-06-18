using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

public interface  IOrderService
{
    Task<OrderModel[]> GetOrdersAsync(int pageIndex, int pageSize);
    Task<OrderModel?> GetOrderAsync(int id);
    Task<OrderModel> AddOrderAsync(OrderDto order);
    Task<OrderModel> UpdateOrderAsync(int id, OrderDto order);
    Task<bool> DeleteOrderAsync(int id);

    // Task<OrderModel[]> SearchOrdersAsync(string query);
}
