
using Domain;

namespace Repository
{
    public interface ISharedTodoRepository
    {
        Task ShareTodoAsync(SharedTodo sharedTodo);
        Task<IEnumerable<SharedTodoDTO>> GetSharedTodosByUserIdAsync(string userId);
    }
}