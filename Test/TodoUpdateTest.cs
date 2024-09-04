using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Repository;
using UseCases;


    [TestFixture]
    public class TodoUpdateTest
    {
        private Mock<ITodoRepository> _todoRepositoryMock;
        private TodoUseCases _todoUseCases;

        [SetUp]
        public void SetUp()
        {
            _todoRepositoryMock = new Mock<ITodoRepository>();
            _todoUseCases = new TodoUseCases(
                _todoRepositoryMock.Object,
                null, // ISharedTodoRepository is not needed for this test
                null  // IApplicationUserRepository is not needed for this test
            );
        }

        [Test]
        public async Task UpdateTodo_ShouldReturnBadRequest_WhenUserIdIsNull()
        {
            // Arrange
            Claim? userId = null;
            var todoId = Guid.NewGuid();
            var model = new AddTodoModel { Name = "Updated Todo" };

            // Act
            var result = await _todoUseCases.UpdateTodo(todoId, userId, model);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.AreEqual("User ID is not available.", badRequestResult.Value);
        }

        [Test]
        public async Task UpdateTodo_ShouldReturnNotFound_WhenTodoItemNotFound()
        {
            // Arrange
            var userId = new Claim(ClaimTypes.NameIdentifier, "user-id-123");
            var todoId = Guid.NewGuid();
            var model = new AddTodoModel { Name = "Updated Todo" };

            _todoRepositoryMock.Setup(repo => repo.GetTodoByIdAsync(todoId, userId.Value)).ReturnsAsync((Todo)null);

            // Act
            var result = await _todoUseCases.UpdateTodo(todoId, userId, model);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.AreEqual("Todo item not found.", notFoundResult.Value);
        }

        [Test]
        public async Task UpdateTodo_ShouldReturnOk_WhenTodoIsUpdatedSuccessfully()
        {
            // Arrange
            var userId = new Claim(ClaimTypes.NameIdentifier, "user-id-123");
            var todoId = Guid.NewGuid();
            var model = new AddTodoModel { Name = "Updated Todo" };
            var todo = new Todo { Id = todoId, Name = "Old Todo" };

            _todoRepositoryMock.Setup(repo => repo.GetTodoByIdAsync(todoId, userId.Value)).ReturnsAsync(todo);
            _todoRepositoryMock.Setup(repo => repo.ChangeNameAsync(todoId, model.Name, userId.Value)).Returns(Task.CompletedTask);
            _todoRepositoryMock.Setup(repo => repo.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _todoUseCases.UpdateTodo(todoId, userId, model);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual("Todo item added successfully.", okResult.Value);
            _todoRepositoryMock.Verify(repo => repo.ChangeNameAsync(todoId, model.Name, userId.Value), Times.Once);
            _todoRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }
    }
