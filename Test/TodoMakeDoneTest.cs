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
    public class TodoMakeDoneTest
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
        public async Task MakeDone_ShouldReturnBadRequest_WhenUserIdIsNull()
        {
            // Arrange
            Claim? userId = null;
            var todoId = Guid.NewGuid();

            // Act
            var result = await _todoUseCases.MakeDone(todoId, userId);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.AreEqual("User ID is not available.", badRequestResult.Value);
        }

        [Test]
        public async Task MakeDone_ShouldReturnOk_WhenTodoIsMarkedAsDoneSuccessfully()
        {
            // Arrange
            var userId = new Claim(ClaimTypes.NameIdentifier, "user-id-123");
            var todoId = Guid.NewGuid();

            _todoRepositoryMock.Setup(repo => repo.MakeDoneAsync(todoId, userId.Value)).Returns(Task.CompletedTask);
            _todoRepositoryMock.Setup(repo => repo.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _todoUseCases.MakeDone(todoId, userId);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
            _todoRepositoryMock.Verify(repo => repo.MakeDoneAsync(todoId, userId.Value), Times.Once);
            _todoRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }
    }
