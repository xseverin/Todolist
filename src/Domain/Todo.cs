using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Todo
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public bool Done { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }

        public ApplicationUser User { get; set; }

        public Guid? ParentId { get; set; }
        public Todo Parent { get; set; }
        public ICollection<Todo> Children { get; set; } = new HashSet<Todo>();
        
        public int Deep { get; set; }
    }
}