using System.ComponentModel.DataAnnotations;

namespace Domain;

public class AddTodoModel
{
    [Required(ErrorMessage = "The name field is required.")]
    [StringLength(100, ErrorMessage = "The name must be between 1 and 100 characters long.", MinimumLength = 1)]
    public string Name { get; set; }
    
    public Guid? ParentId { get; set; }
}