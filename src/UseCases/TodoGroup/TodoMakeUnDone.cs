using System.Security.Claims;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Repository;

namespace UseCases
{
    public partial class TodoUseCases
    {

        public async Task<IActionResult> MakeUnDone(Guid id, Claim? userid)
        {

            if (userid == null)
            {
                return BadRequest("User ID is not available.");
            }

            await _todoRepository.MakeUndoneAsync(id, userid.Value);
            await _todoRepository.SaveChangesAsync();
            return Ok();
        }
    }
}