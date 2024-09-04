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
    public class TodoShareTest
    {
        private Mock<ISharedTodoRepository> _sharedTodoRepositoryMock;
        private Mock<IApplicationUserRepository> _applicationUserRepositoryMock;
        private TodoUseCases _todoUseCases;

        [SetUp]
        public void SetUp()
        {
            _sharedTodoRepositoryMock = new Mock<ISharedTodoRepository>();
            _applicationUserRepositoryMock = new Mock<IApplicationUserRepository>();
            _todoUseCases = new TodoUseCases(
                null, // ITodoRepository is not needed for this test
                _sharedTodoRepositoryMock.Object,
                _applicationUserRepositoryMock.Object
            );
        }

        [Test]
        public async Task Share_ShouldReturnBadRequest_WhenUserIdIsNull()
        {
            // Arrange
            Claim? userId = null;
            var model = new ShareTodoModel { Email = "test@example.com", TodoId = Guid.NewGuid() };

            // Act
            var result = await _todoUseCases.Share(model, userId);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.AreEqual("User ID is not available.", badRequestResult.Value);
        }

        [Test]
        public async Task Share_ShouldReturnBadRequest_WhenUserWithEmailNotFound()
        {
            // Arrange
            var userId = new Claim(ClaimTypes.NameIdentifier, "user-id-123");
            var model = new ShareTodoModel { Email = "test@example.com", TodoId = Guid.NewGuid() };

            _applicationUserRepositoryMock.Setup(repo => repo.FindIdByGmailAsync(model.Email)).ReturnsAsync((ApplicationUser)null);

            // Act
            var result = await _todoUseCases.Share(model, userId);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.AreEqual("User with this gmail is not available.", badRequestResult.Value);
        }

        [Test]
        public async Task Share_ShouldReturnOk_WhenTodoIsSharedSuccessfully()
        {
            // Arrange
            var userId = new Claim(ClaimTypes.NameIdentifier, "user-id-123");
            var model = new ShareTodoModel { Email = "test@example.com", TodoId = Guid.NewGuid() };
            var sharedWithUser = new ApplicationUser { Id = "shared-user-id-123" };

            _applicationUserRepositoryMock.Setup(repo => repo.FindIdByGmailAsync(model.Email)).ReturnsAsync(sharedWithUser);
            _sharedTodoRepositoryMock.Setup(repo => repo.ShareTodoAsync(It.IsAny<SharedTodo>())).Returns(Task.CompletedTask);

            // Act
            var result = await _todoUseCases.Share(model, userId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual("Todo item shared successfully.", okResult.Value);
            _sharedTodoRepositoryMock.Verify(repo => repo.ShareTodoAsync(It.IsAny<SharedTodo>()), Times.Once);
        }
    }
