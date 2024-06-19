
using System.ComponentModel.DataAnnotations;

public class LoginModel
{   

    [Required]
    [EmailAddress]
    public required string email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public required string Password { get; set; }
}
