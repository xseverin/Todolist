using System.ComponentModel.DataAnnotations;
using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Domain;
using Repository;
using UseCases;
using UseCases.UserGroup;
using UseCases.UserGroup;
namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TodoController(TodoUseCases todoUseCases) : ControllerBase
    {
        private readonly TodoUseCases _todoUseCases = todoUseCases;

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add([FromBody] AddTodoModel model)
        {
            var userIdClaim = User.FindFirst("Id");

            return await _todoUseCases.AddTodoAsync(userIdClaim, model);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Todo>>> GetTodos()
        {
            var userIdClaim = User.FindFirst("Id");
            return await _todoUseCases.GetTodos(userIdClaim);
        }

        [HttpPost("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(Guid id, [FromBody] AddTodoModel model)
        {
            var userId = User.FindFirst("Id");

            return await _todoUseCases.UpdateTodo(id, userId, model);
        }

        [HttpPost("{id}")]
        [Authorize]
        public async Task<IActionResult> MakeDone(Guid id)
        {
            
            var userId = User.FindFirst("Id");
            return await _todoUseCases.MakeDone(id, userId);

        }

        [HttpPost("{id}")]
        [Authorize]
        public async Task<IActionResult> MakeUnDone(Guid id)
        {
            var userId = User.FindFirst("Id");

            return await _todoUseCases.MakeUnDone(id, userId);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = User.FindFirst("Id");
            return await _todoUseCases.Delete(id, userId);
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteAllDone()
        {
            var userId = User.FindFirst("Id");

            return await _todoUseCases.DeleteAllDone(userId);
        }
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Share([FromBody] ShareTodoModel model)
        {
            var userIdClaim = User.FindFirst("Id");

            return await _todoUseCases.Share(model, userIdClaim);
        }
        
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<SharedTodoDTO>>> GetSharedTodos()
        {
            var userIdClaim = User.FindFirst("Id");

            return await _todoUseCases.GetSharedTodos(userIdClaim);
        }
    }
    
}