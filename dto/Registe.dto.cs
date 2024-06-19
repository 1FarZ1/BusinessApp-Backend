

using System.ComponentModel.DataAnnotations;

public class RegisterModel
{


    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
    public required string Username { get; set; }


    [Required]
    [EmailAddress]
    [DataType(DataType.EmailAddress)]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
    public required string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
    public required string Password { get; set; }
}
