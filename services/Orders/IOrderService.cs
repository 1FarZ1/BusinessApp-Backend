using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

public interface  IOrderService
{
    Task<GetOrderDto[]> GetOrdersAsync(int pageIndex, int pageSize);

    Task<GetOrderDto[]> GetUserOrdersAsync(string userId, int pageIndex, int pageSize);
    Task<GetOrderDto?> GetOrderAsync(int id);
    Task<OrderModel> AddOrderAsync(OrderDto order, string userId);
    Task<bool> DeleteOrderAsync(int id);    
    Task<bool> UpdateOrderStatusAsync(int id, OrderStatus status);
    Task<bool> OrderBelongsToUser(int orderId, string userId);

    Task<bool> OrderBelongsToSeller(int orderId, string sellerId);

    Task<int> GetOrdersCountAsync();
    Task<int> GetUserOrdersCountAsync(string userId);
    Task<int> GetSellerOrdersCountAsync(string sellerId);
}
