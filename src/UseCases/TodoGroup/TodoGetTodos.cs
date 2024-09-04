using System.Security.Claims;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Repository;

namespace UseCases
{
    public partial class TodoUseCases(
        ITodoRepository todoRepository,
        ISharedTodoRepository sharedTodoRepository,
        IApplicationUserRepository applicationUserRepository) : ControllerBase
    {
        private readonly ITodoRepository _todoRepository = todoRepository;
        private readonly ISharedTodoRepository _sharedTodoRepository = sharedTodoRepository;
        private readonly IApplicationUserRepository _applicationUserRepository = applicationUserRepository;

        public async Task<ActionResult<IEnumerable<Todo>>> GetTodos(Claim? userid)
        {

            if (userid == null)
            {
                return BadRequest("User ID is not available.");
            }
            
            var todos = await _todoRepository.GetTodosByUserIdAsync(userid.Value);
            return Ok(todos);
        }
    }
}