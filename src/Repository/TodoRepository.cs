using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Repository
{
    public interface ITodoRepository
    {
        Task AddTodoAsync(Todo todo);
        Task MakeDoneAsync(Guid todoId, string userId);
        Task MakeUndoneAsync(Guid todoId, string userId);
        Task ChangeNameAsync(Guid todoId, string name, string userId);
        Task<string> GetTodosByUserIdAsync(string userId);
        Task<Todo?> GetTodoByIdAsync(Guid todoId, string userId);
        Task DeleteAsync(Guid id, string userId);
        Task DeleteAllDoneAsync(string userId);
        Task SaveChangesAsync();
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

            await MarkTodoAndChildrenAsDoneAsync(todo);

            await SaveChangesAsync();
        }

        private async Task MarkTodoAndChildrenAsDoneAsync(Todo todo)
        {
            todo.Done = true;

            var children = await _dbContext.Todos
                .Where(t => t.ParentId == todo.Id)
                .ToListAsync();

            foreach (var child in children)
            {
                await MarkTodoAndChildrenAsDoneAsync(child);
            }

            _dbContext.Todos.Update(todo);
        }

        public async Task MakeUndoneAsync(Guid todoId, string userId)
        {
            var todo = await GetTodoByIdAsync(todoId, userId);

            if (todo == null)
            {
                throw new NullReferenceException("There is no todo with such id for the user.");
            }

            await MarkTodoAndChildrenAsUndoneAsync(todo);

            await SaveChangesAsync();
        }

        private async Task MarkTodoAndChildrenAsUndoneAsync(Todo todo)
        {
            todo.Done = false;

            var children = await _dbContext.Todos
                .Where(t => t.ParentId == todo.Id)
                .ToListAsync();

            foreach (var child in children)
            {
                await MarkTodoAndChildrenAsUndoneAsync(child);
            }

            _dbContext.Todos.Update(todo);
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

            await DeleteTodoAndChildrenAsync(todo);

            await SaveChangesAsync();
        }

        private async Task DeleteTodoAndChildrenAsync(Todo todo)
        {
            var children = await _dbContext.Todos
                .Where(t => t.ParentId == todo.Id)
                .ToListAsync();

            foreach (var child in children)
            {
                await DeleteTodoAndChildrenAsync(child);
            }

            _dbContext.Todos.Remove(todo);
        }

        public async Task DeleteAllDoneAsync(string userId)
        {
            var completedTodos = _dbContext.Todos
                .Where(t => t.Done && t.ApplicationUserId == userId);

            _dbContext.Todos.RemoveRange(completedTodos);
            await SaveChangesAsync();
        }

        public async Task SaveChangesAsync() => await _dbContext.SaveChangesAsync();

        public async Task<List<Todo>> GetRootTodosAsync(string userId)
        {
            var rootTodos = await _dbContext.Todos
                .Where(t => t.ApplicationUserId == userId && t.ParentId == null)
                .ToListAsync();

            foreach (var rootTodo in rootTodos)
            {
                await LoadChildrenAsync(rootTodo, 1);
            }

            return rootTodos;
        }

        private async Task LoadChildrenAsync(Todo parent, int depth)
        {
            var children = await _dbContext.Todos
                .Where(t => t.ParentId == parent.Id && t.Deep == depth)
                .Include(t => t.Children)
                .ToListAsync();

            parent.Children = children;

            if (depth < 2)
            {
                foreach (var child in children)
                {
                    await LoadChildrenAsync(child, depth + 1);
                }
            }
        }

        public async Task<string> GetTodosByUserIdAsync(string userId)
        {
            var rootTodos = await GetRootTodosAsync(userId);

            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            return JsonConvert.SerializeObject(rootTodos, settings);
        }
    }
}
