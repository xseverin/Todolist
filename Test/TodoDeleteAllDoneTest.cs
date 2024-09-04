using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Repository;
using UseCases;


    [TestFixture]
    public class TodoDeleteAllDoneTests
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
        public async Task DeleteAllDone_ShouldReturnBadRequest_WhenUserIdIsNull()
        {
            // Arrange
            Claim? userId = null;

            // Act
            var result = await _todoUseCases.DeleteAllDone(userId);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.AreEqual("User ID is not available.", badRequestResult.Value);
        }

        [Test]
        public async Task DeleteAllDone_ShouldReturnOk_WhenTodosAreDeletedSuccessfully()
        {
            // Arrange
            var userId = new Claim(ClaimTypes.NameIdentifier, "user-id-123");

            _todoRepositoryMock.Setup(repo => repo.DeleteAllDoneAsync(userId.Value)).Returns(Task.CompletedTask);

            // Act
            var result = await _todoUseCases.DeleteAllDone(userId);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
            _todoRepositoryMock.Verify(repo => repo.DeleteAllDoneAsync(userId.Value), Times.Once);
        }
    }
