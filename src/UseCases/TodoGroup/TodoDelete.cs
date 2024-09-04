using System.Security.Claims;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Repository;

namespace UseCases
{
    public partial class TodoUseCases
    {

        public async Task<IActionResult> Delete(Guid id, Claim? userid)
        {

            if (userid == null)
            {
                return BadRequest("User ID is not available.");
            }

           
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            await _todoRepository.DeleteAsync(id, userid.Value);
            return Ok();
        }
    }
}


