using System.Security.Claims;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Repository;

namespace UseCases
{
    public partial class TodoUseCases
    {

        public async Task<IActionResult> Share(ShareTodoModel model, Claim? userid)
        {

            if (userid == null)
            {
                return BadRequest("User ID is not available.");
            }

            var sharedWithUser = await _applicationUserRepository.FindIdByGmailAsync(model.Email);

            if (sharedWithUser == null)
            {
                return BadRequest("User with this gmail is not available.");
            }

            var sharedTodo = new SharedTodo
            {
                Id = Guid.NewGuid(),
                SharedByUserId = userid.Value, 
                SharedWithUserId = sharedWithUser.Id,
                TodoId = model.TodoId,
                SharedAt = DateTime.UtcNow
            };

            await _sharedTodoRepository.ShareTodoAsync(sharedTodo);

            return Ok("Todo item shared successfully.");
        }
    }
}