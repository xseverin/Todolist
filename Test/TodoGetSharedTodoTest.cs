using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Repository;
using UseCases;


    [TestFixture]
    public class TodoGetUserTodoTests
    {
        private Mock<ISharedTodoRepository> _sharedTodoRepositoryMock;
        private TodoUseCases _todoUseCases;

        [SetUp]
        public void SetUp()
        {
            _sharedTodoRepositoryMock = new Mock<ISharedTodoRepository>();
            _todoUseCases = new TodoUseCases(
                null, // ITodoRepository is not needed for this test
                _sharedTodoRepositoryMock.Object,
                null  // IApplicationUserRepository is not needed for this test
            );
        }

        [Test]
        public async Task GetSharedTodos_ShouldReturnBadRequest_WhenUserIdIsNull()
        {
            // Arrange
            Claim? userId = null;

            // Act
            var result = await _todoUseCases.GetSharedTodos(userId);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.AreEqual("User ID is not available.", badRequestResult.Value);
        }

        [Test]
        public async Task GetSharedTodos_ShouldReturnOk_WhenSharedTodosAreRetrievedSuccessfully()
        {
            // Arrange
            var userId = new Claim(ClaimTypes.NameIdentifier, "user-id-123");
            var sharedTodos = new List<SharedTodoDTO>
            {
                new SharedTodoDTO { SharedByUserEmail = "user1@example.com", TodoName = "Shared Todo 1" },
                new SharedTodoDTO { SharedByUserEmail = "user2@example.com", TodoName = "Shared Todo 2" }
            };

            _sharedTodoRepositoryMock.Setup(repo => repo.GetSharedTodosByUserIdAsync(userId.Value)).ReturnsAsync(sharedTodos);

            // Act
            var result = await _todoUseCases.GetSharedTodos(userId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual(sharedTodos, okResult.Value);
        }
    }
