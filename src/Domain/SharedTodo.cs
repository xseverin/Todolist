namespace Domain
{
    public class SharedTodo
    {
        public Guid Id { get; set; }
        public string SharedByUserId { get; set; }
        public string SharedWithUserId { get; set; }
        public Guid TodoId { get; set; }
        public DateTime SharedAt { get; set; }

        public ApplicationUser SharedByUser { get; set; }
        public ApplicationUser SharedWithUser { get; set; }
        public Todo Todo { get; set; }
    }
}