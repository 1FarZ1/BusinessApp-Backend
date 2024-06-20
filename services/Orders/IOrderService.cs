using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

public interface  IOrderService
{
    Task<GetOrderDto[]> GetOrdersAsync(int pageIndex, int pageSize);

    Task<OrderModel[]> GetUserOrdersAsync(string userId, int pageIndex, int pageSize);
    Task<OrderModel?> GetOrderAsync(int id);
    Task<OrderModel> AddOrderAsync(OrderDto order, string userId);
    // Task<OrderModel> UpdateOrderAsync(int id, OrderDto order);
    Task<bool> DeleteOrderAsync(int id);

    // Task<OrderModel[]> SearchOrdersAsync(string query);
}
