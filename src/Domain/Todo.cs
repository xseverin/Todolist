using System.ComponentModel.DataAnnotations;

namespace Domain;


public class Todo
{
    public Guid Id { get; set; }
    
    [Required]
    
    public string Name { get; set; }
    
    [Required]
    public bool Done { get; set; }
    
    [Required]
    
    public string ApplicationUserId { get; set; } // UserId типа string
    //public Guid ApplicationUserId { get; set; }
    public ApplicationUser User { get; set; }
}
