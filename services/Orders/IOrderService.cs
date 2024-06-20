using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

public interface  IOrderService
{
    Task<GetOrderDto[]> GetOrdersAsync(int pageIndex, int pageSize);

    Task<GetOrderDto[]> GetUserOrdersAsync(string userId, int pageIndex, int pageSize);
    Task<OrderModel?> GetOrderAsync(int id);
    Task<OrderModel> AddOrderAsync(OrderDto order, string userId);
    Task<bool> DeleteOrderAsync(int id);

    
    Task<bool> UpdateOrderStatusAsync(int id, OrderStatus status);

    // check if ordser belongs to user
    Task<bool> OrderBelongsToUser(int orderId, string userId);

    Task<bool> OrderBelongsToSeller(int orderId, string sellerId);

}
