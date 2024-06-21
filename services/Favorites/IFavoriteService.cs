


public interface IFavoriteService
{
    Task<bool> AddUserFavoriteAsync(string userId, int productId);

    Task<bool> RemoveUserFavoriteAsync(string userId, int productId);
    Task<List<UserFavoriteModel>> GetUserFavoritesAsync(string userId);

    Task<bool> isFavoriteProduct (string userId,int productId);
    
    
    }