using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class ApplicationUser : IdentityUser
    {
        //public ICollection<Todo> Todos { get; set; }
        // Navigation property
        
       
        public string? ResetPasswordToken { get; set; }
        public DateTime? ResetPasswordExpire { get; set; }

        public UserDetail UserDetail { get; set; }
    }
}
