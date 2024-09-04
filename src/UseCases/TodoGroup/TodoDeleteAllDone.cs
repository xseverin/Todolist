using System.Security.Claims;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Repository;

namespace UseCases
{
    public partial class TodoUseCases
    {

        public async Task<IActionResult> DeleteAllDone(Claim? userid)
        {

            if (userid == null)
            {
                return BadRequest("User ID is not available.");
            }

            await _todoRepository.DeleteAllDoneAsync(userid.Value);
            return Ok();
        }
    }
}