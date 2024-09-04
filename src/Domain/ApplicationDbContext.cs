using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Domain
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Todo> Todos { get; set; }
        public DbSet<SharedTodo> SharedTodos { get; set; }
        
        public DbSet<UserDetail> UserDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>().HasKey(u => u.Id);
            modelBuilder.Entity<Todo>().HasKey(t => t.Id);

            modelBuilder.Entity<Todo>()
                .HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.ApplicationUserId);

            modelBuilder.Entity<SharedTodo>().HasKey(st => st.Id);

            modelBuilder.Entity<SharedTodo>()
                .HasOne(st => st.SharedByUser)
                .WithMany()
                .HasForeignKey(st => st.SharedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SharedTodo>()
                .HasOne(st => st.SharedWithUser)
                .WithMany()
                .HasForeignKey(st => st.SharedWithUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SharedTodo>()
                .HasOne(st => st.Todo)
                .WithMany()
                .HasForeignKey(st => st.TodoId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.UserDetail)
                .WithOne(ud => ud.User)
                .HasForeignKey<UserDetail>(ud => ud.UserId);

            base.OnModelCreating(modelBuilder);
        }
    }
}