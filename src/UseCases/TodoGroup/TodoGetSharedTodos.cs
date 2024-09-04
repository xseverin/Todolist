using System.Security.Claims;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Repository;

namespace UseCases
{
    public partial class TodoUseCases
    {

        public async Task<ActionResult<IEnumerable<SharedTodoDTO>>> GetSharedTodos( Claim? userid)
        {

            if (userid == null)
            {
                return BadRequest("User ID is not available.");
            }

            var sharedTodos = await _sharedTodoRepository.GetSharedTodosByUserIdAsync(userid.Value);
       
            return Ok(sharedTodos);
        }
    }
}