using System.Security.Claims;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Repository;

namespace UseCases
{
    public partial class TodoUseCases : ControllerBase
    {
        public async Task<ActionResult> AddTodoAsync(Claim? userid, AddTodoModel model)
        {
            if (userid == null)
            {
                return BadRequest("User ID is not available.");
            }

            int deep = 0;

            if (model.ParentId.HasValue)
            {
                var parentTodo = await _todoRepository.GetTodoByIdAsync(model.ParentId.Value, userid.Value);
                if (parentTodo == null)
                {
                    return NotFound("Parent todo item not found.");
                }

                deep = parentTodo.Deep + 1;

                if (deep > 2)
                {
                    return BadRequest("Cannot add more than 2 levels of depth.");
                }
            }

            var todo = new Todo
            {
                Name = model.Name,
                Done = false,
                ApplicationUserId = userid.Value,
                ParentId = model.ParentId,
                Deep = deep
            };

            await _todoRepository.AddTodoAsync(todo);
            await _todoRepository.SaveChangesAsync();

            return Ok("Todo item added successfully.");
        }

    }
}
/*public async Task AddTodoAsync(string userId, string name, Guid? parentId)
    {
        int deep = 0;

        if (parentId.HasValue)
        {
            var parent = await _dbContext.Todos.FindAsync(parentId.Value);
            if (parent == null)
            {
                throw new ArgumentException("Parent task not found.");
            }

            deep = parent.Deep + 1;

            if (deep > 2)
            {
                throw new InvalidOperationException("Cannot add more than 2 levels of depth.");
            }
        }

        var todo = new Todo
        {
            Id = Guid.NewGuid(),
            Name = name,
            Done = false,
            ApplicationUserId = userId,
            ParentId = parentId,
            Deep = deep
        };

        _dbContext.Todos.Add(todo);
        await _dbContext.SaveChangesAsync();
    }*/