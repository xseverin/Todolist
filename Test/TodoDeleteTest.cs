// Update the test method in TodoUseCasesTests

using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Repository;
using UseCases;

[TestFixture]
public class TodoUseCasesTests
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
    public async Task Delete_ShouldReturnBadRequest_WhenUserIdIsNull()
    {
        // Arrange
        Claim? userId = null;
        var todoId = Guid.NewGuid();

        // Act
        var result = await _todoUseCases.Delete(todoId, userId);

        // Assert
        Assert.IsInstanceOf<BadRequestObjectResult>(result);
        var badRequestResult = result as BadRequestObjectResult;
        Assert.AreEqual("User ID is not available.", badRequestResult.Value);
    }

    [Test]
    public async Task Delete_ShouldReturnNotFound_WhenTodoIdIsEmpty()
    {
        // Arrange
        var userId = new Claim(ClaimTypes.NameIdentifier, "user-id-123");
        var todoId = Guid.Empty;

        // Act
        var result = await _todoUseCases.Delete(todoId, userId);

        // Assert
        Assert.IsInstanceOf<NotFoundResult>(result);
    }

    [Test]
    public async Task Delete_ShouldReturnOk_WhenTodoIsDeletedSuccessfully()
    {
        // Arrange
        var userId = new Claim(ClaimTypes.NameIdentifier, "user-id-123");
        var todoId = Guid.NewGuid();

        _todoRepositoryMock.Setup(repo => repo.DeleteAsync(todoId, userId.Value)).Returns(Task.CompletedTask);

        // Act
        var result = await _todoUseCases.Delete(todoId, userId);

        // Assert
        Assert.IsInstanceOf<OkResult>(result);
        _todoRepositoryMock.Verify(repo => repo.DeleteAsync(todoId, userId.Value), Times.Once);
    }
}