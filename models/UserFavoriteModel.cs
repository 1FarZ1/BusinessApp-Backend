



using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class UserFavoriteModel
{
    public int Id { get; set; }


    [Required]
    public string UserId { get; set; }

    [ForeignKey("UserId")]
    public virtual  UserModel User { get; set; }


    [Required]
    public int ProductId { get; set; }

    [ForeignKey("ProductId")]
    public virtual  ProductModel Product { get; set; }
}