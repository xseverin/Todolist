using System.ComponentModel.DataAnnotations;
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
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add([FromBody] AddTodoModel model)
        {
            var userIdClaim = User.FindFirst("Id");

            return await todoUseCases.AddTodoAsync(userIdClaim, model);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Todo>>> GetTodos()
        {
            var userIdClaim = User.FindFirst("Id");
            return await todoUseCases.GetTodos(userIdClaim);
        }

        [HttpPost("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(Guid id, [FromBody] AddTodoModel model)
        {
            var userId = User.FindFirst("Id");

            return await todoUseCases.UpdateTodo(id, userId, model);
        }

        [HttpPost("{id}")]
        [Authorize]
        public async Task<IActionResult> MakeDone(Guid id)
        {
            
            var userId = User.FindFirst("Id");
            return await todoUseCases.MakeDone(id, userId);

        }

        [HttpPost("{id}")]
        [Authorize]
        public async Task<IActionResult> MakeUnDone(Guid id)
        {
            var userId = User.FindFirst("Id");

            return await todoUseCases.MakeUnDone(id, userId);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = User.FindFirst("Id");
            return await todoUseCases.Delete(id, userId);
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteAllDone()
        {
            var userId = User.FindFirst("Id");

            return await todoUseCases.DeleteAllDone(userId);
        }
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Share([FromBody] ShareTodoModel model)
        {
            var userIdClaim = User.FindFirst("Id");

            return await todoUseCases.Share(model, userIdClaim);
        }
        
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<SharedTodoDTO>>> GetSharedTodos()
        {
            var userIdClaim = User.FindFirst("Id");

            return await todoUseCases.GetSharedTodos(userIdClaim);
        }
    }
    
}