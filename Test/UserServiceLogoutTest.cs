// Test/UserServiceLogoutTests.cs
using Domain;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UseCases.UserGroup;

[TestFixture]
public class UserServiceLogoutTests
{
    private Mock<UserManager<ApplicationUser>> _userManagerMock;
    //rivate ApplicationDbContext _context;
    private UserService _userService;
    private IConfiguration _configuration;

    [SetUp]
    public void SetUp()
    {
        var apiProjectPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "src", "Api");
        var configBuilder = new ConfigurationBuilder()
            .SetBasePath(apiProjectPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        _configuration = configBuilder.Build();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        //_context = new ApplicationDbContext(options);

        _userManagerMock = new Mock<UserManager<ApplicationUser>>(
            Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

        _userService = new UserService(
            _userManagerMock.Object,
            null, // SignInManager is not needed for logout
            null, //_context, // Use in-memory database context directly
            null, // TokenSettings is not needed for logout
            null,
            null, // UserDetailRepository is not needed for logout
            null, // EmailService is not needed for logout
            null  // UserRepository is not needed for logout
        );
    }

    [Test]
    public async Task UserLogoutAsync_ShouldReturnSuccess_WhenUserIsAuthenticated()
    {
        // Arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Name, "testuser"),
            new Claim("UserName", "testuser")
        }, "mock"));

        var appUser = new ApplicationUser { UserName = "testuser" };

        _userManagerMock.Setup(um => um.FindByNameAsync("testuser"))
            .ReturnsAsync(appUser);
        _userManagerMock.Setup(um => um.UpdateSecurityStampAsync(appUser))
            .Returns(Task.FromResult(IdentityResult.Success));

        // Act
        var result = await _userService.UserLogoutAsync(user);

        // Assert
        Assert.IsTrue(result.IsSucceed);
        Assert.IsTrue(result.Data);
        _userManagerMock.Verify(um => um.UpdateSecurityStampAsync(appUser), Times.Once);
    }

    [Test]
    public async Task UserLogoutAsync_ShouldReturnSuccess_WhenUserIsNotAuthenticated()
    {
        // Arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity());

        // Act
        var result = await _userService.UserLogoutAsync(user);

        // Assert
        Assert.IsTrue(result.IsSucceed);
        Assert.IsTrue(result.Data);
        _userManagerMock.Verify(um => um.UpdateSecurityStampAsync(It.IsAny<ApplicationUser>()), Times.Never);
    }
}
