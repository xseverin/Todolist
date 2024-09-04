using System.Security.Claims;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Repository;
using UseCases;


    [TestFixture]
    public class TodoAddTests
    {
        private Mock<ITodoRepository> _todoRepositoryMock;
        private Mock<ISharedTodoRepository> _sharedTodoRepositoryMock;
        private Mock<IApplicationUserRepository> _applicationUserRepositoryMock;
        private TodoUseCases _todoUseCases;

        [SetUp]
        public void SetUp()
        {
            _todoRepositoryMock = new Mock<ITodoRepository>();
            _sharedTodoRepositoryMock = new Mock<ISharedTodoRepository>();
            _applicationUserRepositoryMock = new Mock<IApplicationUserRepository>();

            _todoUseCases = new TodoUseCases(
                _todoRepositoryMock.Object,
                _sharedTodoRepositoryMock.Object,
                _applicationUserRepositoryMock.Object);
        }

        [Test]
        public async Task AddTodo_ShouldReturnBadRequest_WhenUserIdIsNull()
        {
            // Arrange
            Claim? userId = null;
            var model = new AddTodoModel { Name = "Test Todo" };

            // Act
            var result = await _todoUseCases.AddTodoAsync(userId, model);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.AreEqual("User ID is not available.", badRequestResult.Value);
        }

        [Test]
        public async Task AddTodo_ShouldReturnOk_WhenTodoIsAddedSuccessfully()
        {
            // Arrange
            var userId = new Claim(ClaimTypes.NameIdentifier, "user-id-123");
            var model = new AddTodoModel { Name = "Test Todo" };
            var todo = new Todo
            {
                Name = model.Name,
                Done = false,
                ApplicationUserId = userId.Value
            };

            _todoRepositoryMock.Setup(repo => repo.AddTodoAsync(It.IsAny<Todo>())).Returns(Task.CompletedTask);
            _todoRepositoryMock.Setup(repo => repo.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _todoUseCases.AddTodoAsync(userId, model);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual("Todo item added successfully.", okResult.Value);
            _todoRepositoryMock.Verify(repo => repo.AddTodoAsync(It.Is<Todo>(t => t.Name == model.Name && t.ApplicationUserId == userId.Value)), Times.Once);
            _todoRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }
    }

