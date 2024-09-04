using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Repository
{
    public interface ISharedTodoRepository
    {
        Task ShareTodoAsync(SharedTodo sharedTodo);
        Task<string> GetSharedTodosByUserIdAsync(string userId);
    }

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

        public async Task<string> GetSharedTodosByUserIdAsync(string userId)
        {
            var sharedTodos = await _context.SharedTodos
                .Include(st => st.Todo)
                .ThenInclude(t => t.Children)
                .Include(st => st.SharedByUser)
                .Where(st => st.SharedWithUserId == userId)
                .ToListAsync();

            var groupedTodos = new Dictionary<string, List<TodoDto>>();

            foreach (var sharedTodo in sharedTodos)
            {
                var email = sharedTodo.SharedByUser.Email;

                if (!groupedTodos.ContainsKey(email))
                {
                    groupedTodos[email] = new List<TodoDto>();
                }

                var rootTodo = sharedTodo.Todo;
                var rootTodoDto = await MapTodoToDtoAsync(rootTodo);
                groupedTodos[email].Add(rootTodoDto);
            }

            // Serializing to JSON with settings to handle reference loops
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            return JsonConvert.SerializeObject(groupedTodos, settings);
        }

        private async Task<TodoDto> MapTodoToDtoAsync(Todo todo)
        {
            var dto = new TodoDto
            {
                Id = todo.Id.ToString(),  // Convert Guid to string
                Name = todo.Name,
                Done = todo.Done
            };

            var children = await _context.Todos
                .Where(t => t.ParentId == todo.Id)
                .ToListAsync();

            foreach (var child in children)
            {
                var childDto = await MapTodoToDtoAsync(child); // Recursive mapping of children
                dto.Children.Add(childDto);
            }

            return dto;
        }
    }

    public class TodoDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool Done { get; set; }
        public List<TodoDto> Children { get; set; } = new List<TodoDto>();
    }
}
