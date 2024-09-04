// Test/UserServiceLoginTests.cs

using System.Security.Claims;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Repository;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UseCases;
using UseCases.UserGroup;

[TestFixture]
public class UserServiceLoginTests
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
    private Mock<ITokenUtil> _tokenUtil;

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
        _tokenUtil = new Mock<ITokenUtil>();

        _userService = new UserService(
            _userManagerMock.Object,
            _signInManagerMock.Object,
            _contextMock.Object,
            _tokenSettingsMock.Object.Value, _tokenUtil.Object,
            _userDetailRepositoryMock.Object,
            _emailServiceMock.Object,
            _userRepositoryMock.Object);
    }

    [Test]
    public async Task UserLoginAsync_ShouldReturnSuccess_WhenLoginIsSuccessful()
    {
        // Arrange
        var request = new UserLoginRequest
        {
            Email = "test@example.com",
            Password = "Password123!"
        };

        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email
        };

        _userManagerMock.Setup(um => um.FindByEmailAsync(request.Email))
            .ReturnsAsync(user);

        _signInManagerMock.Setup(sm => sm.CheckPasswordSignInAsync(user, request.Password, true))
            .ReturnsAsync(SignInResult.Success);

        _userManagerMock.Setup(um => um.RemoveAuthenticationTokenAsync(user, "REFRESHTOKENPROVIDER", "RefreshToken"))
            .Returns(Task.FromResult(IdentityResult.Success));

        _userManagerMock.Setup(um => um.GenerateUserTokenAsync(user, "REFRESHTOKENPROVIDER", "RefreshToken"))
            .ReturnsAsync("new_refresh_token");
        
        _tokenUtil.Setup(ts => ts.GetToken(It.IsAny<TokenSettings>(), It.IsAny<ApplicationUser>(), It.IsAny<List<Claim>>()))
            .Returns("newAccessToken");

        _userManagerMock.Setup(um => um.SetAuthenticationTokenAsync(user, "REFRESHTOKENPROVIDER", "RefreshToken", "new_refresh_token"))
            .Returns(Task.FromResult(IdentityResult.Success));

        // Act
        var result = await _userService.UserLoginAsync(request);

        // Assert
        Assert.IsTrue(result.IsSucceed);
        Assert.IsNotNull(result.Data);
        Assert.IsFalse(string.IsNullOrEmpty(result.Data.AccessToken));
        Assert.AreEqual("new_refresh_token", result.Data.RefreshToken);
        _signInManagerMock.Verify(sm => sm.CheckPasswordSignInAsync(user, request.Password, true), Times.Once);
    }

    [Test]
    public async Task UserLoginAsync_ShouldReturnError_WhenLoginFails()
    {
        // Arrange
        var request = new UserLoginRequest
        {
            Email = "test@example.com",
            Password = "wrongpassword"
        };

        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email
        };

        _userManagerMock.Setup(um => um.FindByEmailAsync(request.Email))
            .ReturnsAsync(user);

        _signInManagerMock.Setup(sm => sm.CheckPasswordSignInAsync(user, request.Password, true))
            .ReturnsAsync(SignInResult.Failed);

        // Act
        var result = await _userService.UserLoginAsync(request);

        // Assert
        Assert.IsFalse(result.IsSucceed);
        Assert.IsTrue(result.Messages.ContainsKey("error"));
        Assert.AreEqual("Email or password is incorrect", result.Messages["error"][0]);
        _signInManagerMock.Verify(sm => sm.CheckPasswordSignInAsync(user, request.Password, true), Times.Once);
    }
}