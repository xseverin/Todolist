using System.Security.Claims;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Repository;
using UseCases;
using UseCases.UserGroup;

[TestFixture]
public class UserServiceUpdateProfileTests
{
    private Mock<UserManager<ApplicationUser>> _userManagerMock;
    private Mock<SignInManager<ApplicationUser>> _signInManagerMock;
    private Mock<ApplicationDbContext> _contextMock;
    private Mock<IUserDetailRepository> _userDetailRepositoryMock;
    private Mock<EmailService> _emailServiceMock;
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<IOptions<TokenSettings>> _tokenSettingsMock;
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

        var smtpSettings = _configuration.GetSection("SmtpSettings").Get<SmtpSettings>();
        var tokenSettings = _configuration.GetSection("TokenSettings").Get<TokenSettings>();

        _userManagerMock = new Mock<UserManager<ApplicationUser>>(
            Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
        _signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
            _userManagerMock.Object,
            Mock.Of<IHttpContextAccessor>(),
            Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(),
            null, null, null, null);

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        _contextMock = new Mock<ApplicationDbContext>(options);

        _userDetailRepositoryMock = new Mock<IUserDetailRepository>();
        _emailServiceMock = new Mock<EmailService>(
            smtpSettings.Host,
            smtpSettings.Port,
            smtpSettings.User,
            smtpSettings.Password);
        _userRepositoryMock = new Mock<IUserRepository>();
        _tokenSettingsMock = new Mock<IOptions<TokenSettings>>();
        _tokenSettingsMock.Setup(t => t.Value).Returns(tokenSettings);

        _userService = new UserService(
            _userManagerMock.Object,
            _signInManagerMock.Object,
            _contextMock.Object,
            _tokenSettingsMock.Object.Value,null,
            _userDetailRepositoryMock.Object,
            _emailServiceMock.Object,
            _userRepositoryMock.Object);
    }

    [Test]
    public async Task UpdateProfileAsync_ShouldReturnSuccess_WhenProfileIsUpdated()
    {
        // Arrange
        var user = new ApplicationUser { Id = "1", UserName = "testuser", Email = "oldemail@example.com" };
        var userProfile = new UserProfile { Email = "newemail@example.com", FirstName = "John", LastName = "Doe" };
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.NameIdentifier, user.Id) }));

        _userManagerMock.Setup(um => um.GetUserAsync(claimsPrincipal)).ReturnsAsync(user);
        _userManagerMock.Setup(um => um.SetEmailAsync(user, userProfile.Email)).ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(um => um.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);
        _userDetailRepositoryMock.Setup(udr => udr.GetUserDetailAsync(user.Id)).ReturnsAsync(new UserDetail());
        _userDetailRepositoryMock.Setup(udr => udr.UpdateUserDetailAsync(It.IsAny<UserDetail>()));

        // Act
        var result = await _userService.UpdateProfileAsync(claimsPrincipal, userProfile);

        // Assert
        Assert.IsTrue(result.IsSucceed);
        Assert.IsTrue(result.Data);
    }

    [Test]
    public async Task UpdateProfileAsync_ShouldReturnError_WhenUserNotFound()
    {
        // Arrange
        var userProfile = new UserProfile { Email = "newemail@example.com", FirstName = "John", LastName = "Doe" };
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.NameIdentifier, "1") }));

        _userManagerMock.Setup(um => um.GetUserAsync(claimsPrincipal)).ReturnsAsync((ApplicationUser)null);

        // Act
        var result = await _userService.UpdateProfileAsync(claimsPrincipal, userProfile);

        // Assert
        Assert.IsFalse(result.IsSucceed);
        Assert.AreEqual("User not found.", result.Messages["user"][0]);
    }

    [Test]
    public async Task UpdateProfileAsync_ShouldReturnError_WhenEmailUpdateFails()
    {
        // Arrange
        var user = new ApplicationUser { Id = "1", UserName = "testuser", Email = "oldemail@example.com" };
        var userProfile = new UserProfile { Email = "newemail@example.com", FirstName = "John", LastName = "Doe" };
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.NameIdentifier, user.Id) }));

        _userManagerMock.Setup(um => um.GetUserAsync(claimsPrincipal)).ReturnsAsync(user);
        _userManagerMock.Setup(um => um.SetEmailAsync(user, userProfile.Email)).ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Failed to update email." }));

        // Act
        var result = await _userService.UpdateProfileAsync(claimsPrincipal, userProfile);

        // Assert
        Assert.IsFalse(result.IsSucceed);
        Assert.AreEqual("Failed to update email.", result.Messages["email"][0]);
    }

    [Test]
    public async Task UpdateProfileAsync_ShouldReturnError_WhenUserProfileUpdateFails()
    {
        // Arrange
        var user = new ApplicationUser { Id = "1", UserName = "testuser", Email = "oldemail@example.com" };
        var userProfile = new UserProfile { Email = "newemail@example.com", FirstName = "John", LastName = "Doe" };
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.NameIdentifier, user.Id) }));

        _userManagerMock.Setup(um => um.GetUserAsync(claimsPrincipal)).ReturnsAsync(user);
        _userManagerMock.Setup(um => um.SetEmailAsync(user, userProfile.Email)).ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(um => um.UpdateAsync(user)).ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Failed to update user profile." }));

        // Act
        var result = await _userService.UpdateProfileAsync(claimsPrincipal, userProfile);

        // Assert
        Assert.IsFalse(result.IsSucceed);
        Assert.AreEqual("Failed to update user profile.", result.Messages["user profile"][0]);
    }
    
}