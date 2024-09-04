using System.ComponentModel.DataAnnotations;

namespace Domain;

public class UserDetail
{
    public Guid Id { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public string Address { get; set; }
    [Required]
    
    // Foreign Key
    public string UserId { get; set; }

    // Navigation property
    public ApplicationUser User { get; set; }
}
