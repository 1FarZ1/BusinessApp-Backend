using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

public class UserModel : IdentityUser
{


    public virtual ICollection<OrderModel> Orders { get; set; } 

    // public virtual ICollection<ReviewModel> Reviews { get; set; }

    public virtual ICollection<UserFavoriteModel> UserFavorites { get; set; }


    public virtual ICollection<ReviewModel> Reviews { get; set; }

    // public string PhoneNumber { get; set; }
    // public string Address { get; set; }
    // public string City { get; set; }
    // public string State { get; set; }
    // public string ZipCode { get; set; }
    // public string Country { get; set; }
    // public string ProfilePicture { get; set; }
    // public string Role { get; set; }
    // public string Status { get; set; }
}