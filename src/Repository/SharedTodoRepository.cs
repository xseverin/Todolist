using Domain;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class SharedTodoRepository : ISharedTodoRepository
{
    private readonly ApplicationDbContext _context;

    public SharedTodoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task ShareTodoAsync(SharedTodo sharedTodo)
    {
        await _context.SharedTodos.AddAsync(sharedTodo);
        await _context.SaveChangesAsync();
    }
        
    public async Task<IEnumerable<SharedTodoDTO>> GetSharedTodosByUserIdAsync(string userId)
    {
        return await _context.SharedTodos
            .Include(st => st.Todo)
            .Include(st => st.SharedByUser)
            .Where(st => st.SharedWithUserId == userId)
            .Select(st => new SharedTodoDTO
            {
                SharedByUserEmail = st.SharedByUser.Email,
                TodoName = st.Todo.Name
            })
            .ToListAsync();
    }
}