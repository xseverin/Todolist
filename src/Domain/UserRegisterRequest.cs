using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Domain;

public class UserRegisterRequest
{
    [Required(ErrorMessage = "The email field is required.")]
    public string Email { get; set; }
    [Required(ErrorMessage = "The password field is required.")]
    public string Password { get; set; }
    [Required(ErrorMessage = "The first name field is required.")]
    public string FirstName { get; set; }
    [Required(ErrorMessage = "The last name field is required.")]
    public string LastName { get; set; }
    [Required(ErrorMessage = "The address field is required.")]
    public string Address { get; set; }
}