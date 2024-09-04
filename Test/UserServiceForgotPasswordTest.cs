using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Repository;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UseCases;
using UseCases.UserGroup;

[TestFixture]
public class UserServiceForgotPasswordTests
{
    private Mock<UserManager<ApplicationUser>> _userManagerMock;
    private Mock<SignInManager<ApplicationUser>> _signInManagerMock;
    private ApplicationDbContext _contextMock;
    private Mock<IUserDetailRepository> _userDetailRepositoryMock;
    private Mock<IEmailService> _emailServiceMock;
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<IOptions<TokenSettings>> _tokenSettingsMock;
    private Mock<IOptions<SmtpSettings>> _smtpSettingsMock;
    private UserService _userService;
    private Mock<ITokenUtil> _tokenUtilMock;
    private IConfiguration _configuration;

    [OneTimeSetUp]
    public void SetUp()
    {
        var apiProjectPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "src", "Api");
        var configBuilder = new ConfigurationBuilder()
            .SetBasePath(apiProjectPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        _configuration = configBuilder.Build();

        var smtpSettings = _configuration.GetSection("SmtpSettings").Get<SmtpSettings>();

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
        _contextMock = new ApplicationDbContext(options);

        _userDetailRepositoryMock = new Mock<IUserDetailRepository>();
        _smtpSettingsMock = new Mock<IOptions<SmtpSettings>>();
        _smtpSettingsMock.Setup(s => s.Value).Returns(smtpSettings);
        _emailServiceMock = new Mock<IEmailService>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _tokenSettingsMock = new Mock<IOptions<TokenSettings>>();
        _tokenUtilMock = new Mock<ITokenUtil>();

        _userService = new UserService(
            _userManagerMock.Object,
            _signInManagerMock.Object,
            _contextMock,
            _tokenSettingsMock.Object.Value,
            _tokenUtilMock.Object,
            _userDetailRepositoryMock.Object,
            _emailServiceMock.Object,
            _userRepositoryMock.Object);
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        // Clear data if necessary
        _contextMock.Database.EnsureDeleted();
        _contextMock.Dispose();
    }
    
    [Test]
    public async Task ForgotPassword_ShouldReturnError_WhenUserNotFound()
    {
        // Arrange
        var request = new ForgotPasswordRequest
        {
            Email = "nonexistent@example.com"
        };

        _userRepositoryMock.Setup(repo => repo.GetUserByEmailAsync(request.Email))
            .ReturnsAsync((ApplicationUser)null);

        // Act
        var result = await _userService.ForgotPassword(request);

        // Assert
        Assert.IsFalse(result.IsSucceed);
        Assert.AreEqual("User not found", result.Messages["User"][0]);
    }

    [Test]
    public async Task ForgotPassword_ShouldReturnSuccess_WhenUserFound()
    {
        // Arrange
        var request = new ForgotPasswordRequest
        {
            Email = "existing@example.com"
        };

        var user = new ApplicationUser
        {
            Email = request.Email,
            UserName = "existingUser",
            ResetPasswordToken = null,
            ResetPasswordExpire = null
        };

        var resetToken = "resetToken";
        var hashedToken = "hashedResetToken";

        _userRepositoryMock.Setup(repo => repo.GetUserByEmailAsync(request.Email))
            .ReturnsAsync(user);

        _tokenUtilMock.Setup(util => util.GenerateResetToken())
            .Returns(resetToken);
        _tokenUtilMock.Setup(util => util.HashToken(resetToken))
            .Returns(hashedToken);

        _emailServiceMock.Setup(service => service.SendEmail(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>()));

        // Act
        var result = await _userService.ForgotPassword(request);

        // Assert
        Assert.IsTrue(result.IsSucceed);
        Assert.IsTrue(result.Data);
        Assert.AreEqual(hashedToken, user.ResetPasswordToken);
        Assert.IsNotNull(user.ResetPasswordExpire);
        _userRepositoryMock.Verify(repo => repo.GetUserByEmailAsync(request.Email), Times.Once);
        _tokenUtilMock.Verify(util => util.GenerateResetToken(), Times.Once);
        _tokenUtilMock.Verify(util => util.HashToken(resetToken), Times.Once);
        _emailServiceMock.Verify(service => service.SendEmail(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>()), Times.Once);
    }
}
