



public class UserFavoriteModel
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public UserModel User { get; set; }
    public int ProductId { get; set; }
    public ProductModel Product { get; set; }
}