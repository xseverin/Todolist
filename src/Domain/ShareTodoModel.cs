using System.ComponentModel.DataAnnotations;

public class ShareTodoModel
{
    [Required(ErrorMessage = "The email field is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "The todoId field is required.")]
    public Guid TodoId { get; set; }
}