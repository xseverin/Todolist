using Domain;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public interface ITodoRepository
    {
        Task AddTodoAsync(Todo todo);
        Task MakeDoneAsync(Guid todoId, string userId);
        Task MakeUndoneAsync(Guid todoId, string userId);
        Task ChangeNameAsync(Guid todoId, string name, string userId);
        Task<IEnumerable<Todo>> GetTodosByUserIdAsync(string userId);
        Task<Todo?> GetTodoByIdAsync(Guid todoId, string userId);
        Task DeleteAsync(Guid id, string userId);
        Task DeleteAllDoneAsync(string userId);
        Task SaveChangesAsync();
        Task<List<Todo>> GetAllAsync(string userId);
    }
    
    public class TodoRepository : ITodoRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TodoRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddTodoAsync(Todo todo)
        {
            await _dbContext.Todos.AddAsync(todo);
        }

        public async Task MakeDoneAsync(Guid todoId, string userId)
        {
            var todo = await GetTodoByIdAsync(todoId, userId);

            if (todo == null)
            {
                throw new NullReferenceException("There is no todo with such id for the user.");
            }

            todo.Done = true;

            _dbContext.Todos.Update(todo);
            await SaveChangesAsync();
        }

        public async Task MakeUndoneAsync(Guid todoId, string userId)
        {
            var todo = await GetTodoByIdAsync(todoId, userId);

            if (todo == null)
            {
                throw new NullReferenceException("There is no todo with such id for the user.");
            }

            todo.Done = false;

            _dbContext.Todos.Update(todo);
            await SaveChangesAsync();
        }

        public async Task ChangeNameAsync(Guid todoId, string name, string userId)
        {
            var todo = await GetTodoByIdAsync(todoId, userId);

            if (todo == null)
            {
                throw new NullReferenceException("There is no todo with such id for the user.");
            }

            todo.Name = name;

            _dbContext.Todos.Update(todo);
            await SaveChangesAsync();
        }

        public async Task<IEnumerable<Todo>> GetTodosByUserIdAsync(string userId)
        {
            return await _dbContext.Todos
                .Where(t => t.ApplicationUserId == userId)
                .ToListAsync();
        }
        public async Task<Todo?> GetTodoByIdAsync(Guid todoId, string userId)
        {
            return await _dbContext.Todos
                .Where(t => t.Id == todoId && t.ApplicationUserId == userId)
                .FirstOrDefaultAsync();
        }

        public async Task DeleteAsync(Guid id, string userId)
        {
            var todo = await GetTodoByIdAsync(id, userId);

            if (todo == null)
            {
                throw new NullReferenceException("There is no todo with such id for the user.");
            }

            _dbContext.Todos.Remove(todo);
            await SaveChangesAsync();
        }
        
        public async Task DeleteAllDoneAsync(string userId)
        {
            var completedTodos = _dbContext.Todos
                .Where(t => t.Done && t.ApplicationUserId == userId);

            _dbContext.Todos.RemoveRange(completedTodos);
            await SaveChangesAsync();
        }

        public async Task SaveChangesAsync() => await _dbContext.SaveChangesAsync();

        public async Task<List<Todo>> GetAllAsync(string userId)
        {
            return await _dbContext.Todos
                .Where(t => t.ApplicationUserId == userId)
                .ToListAsync();
        }
    }
}