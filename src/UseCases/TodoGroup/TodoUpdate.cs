using System.Security.Claims;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Repository;

namespace UseCases
{
    public partial class TodoUseCases: ControllerBase
    {
        public async Task<ActionResult> UpdateTodo(Guid id, Claim? userid, AddTodoModel model)
        {
            if (userid == null)
            {
                return BadRequest("User ID is not available.");
            }
            
            var todo = await _todoRepository.GetTodoByIdAsync(id, userid.Value);
            if (todo == null)
            {
                return NotFound("Todo item not found.");
            }
            await _todoRepository.ChangeNameAsync(id, model.Name, userid.Value);
            await _todoRepository.SaveChangesAsync();

            return Ok("Todo item added successfully.");
        }
    }
}