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
    public class TodoGetTodoTests
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
        public async Task GetTodos_ShouldReturnBadRequest_WhenUserIdIsNull()
        {
            // Arrange
            Claim? userId = null;

            // Act
            var result = await _todoUseCases.GetTodos(userId);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.AreEqual("User ID is not available.", badRequestResult.Value);
        }

        [Test]
        public async Task GetTodos_ShouldReturnOk_WhenTodosAreRetrievedSuccessfully()
        {
            // Arrange
            var userId = new Claim(ClaimTypes.NameIdentifier, "user-id-123");
            var todos = new List<Todo>
            {
                new Todo { Id = Guid.NewGuid(), Name = "Todo 1", Done = true, ApplicationUserId = userId.ToString()},
                new Todo { Id = Guid.NewGuid(), Name = "Todo 2" , Done = true, ApplicationUserId = userId.ToString()}
            };

            _todoRepositoryMock.Setup(repo => repo.GetTodosByUserIdAsync(It.IsAny<string>()))
                .ReturnsAsync("Serialized JSON string of shared todos");
            
            // Act
            var result = await _todoUseCases.GetTodos(userId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual("Serialized JSON string of shared todos", okResult.Value);
        }
    }
