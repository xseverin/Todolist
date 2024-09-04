using System.Security.Claims;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Repository;

namespace UseCases
{
    public partial class TodoUseCases: ControllerBase
    {
        public async Task<ActionResult> AddTodoAsync(Claim? userid, AddTodoModel model)
        {
            if (userid == null)
            {
                return BadRequest("User ID is not available.");
            }

            var todo = new Todo
            {
                Name = model.Name,
                Done = false,
                ApplicationUserId = userid.Value
            };

            await _todoRepository.AddTodoAsync(todo);
            await _todoRepository.SaveChangesAsync();

            return Ok("Todo item added successfully.");
        }
    }
}

