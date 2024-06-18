using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

public class UserModel : IdentityUser
{
    //context

    [Required]
    public string Id { get; set; }


    [Required]
    public string Username { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }



    //orders
    public OrderModel[] Orders { get; set; }

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