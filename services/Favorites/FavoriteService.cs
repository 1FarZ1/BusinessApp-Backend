




using Microsoft.EntityFrameworkCore;

public class FavoriteService : IFavoriteService
{
    private readonly ApplicationDbContext _context;
    

    public FavoriteService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> AddUserFavoriteAsync(string userId, int productId)
    {
        var userFavorite = new UserFavoriteModel
        {
            UserId = userId,
            ProductId = productId
        };
        await _context.UserFavorites.AddAsync(userFavorite);
        await _context.SaveChangesAsync();
        return userFavorite != null;
        }

    public async Task<bool> RemoveUserFavoriteAsync(string userId, int productId)
    {
        var userFavorite = await _context.UserFavorites.FirstOrDefaultAsync(x => x.UserId == userId && x.ProductId == productId);
        if (userFavorite == null) return false;
        _context.UserFavorites.Remove(userFavorite);
        await _context.SaveChangesAsync();
        return userFavorite != null;
    }

    public async Task<List<UserFavoriteModel>> GetUserFavoritesAsync(string userId)
    {
        var userFavorites = await _context.UserFavorites.Where(x => x.UserId == userId).ToListAsync();
        return userFavorites;
    }


    public async Task<bool> isFavoriteProduct(string userId, int productId)
    {
        var userFavorite = await _context.UserFavorites.FirstOrDefaultAsync(x => x.UserId == userId && x.ProductId == productId);
        return userFavorite != null;
    }

    
}