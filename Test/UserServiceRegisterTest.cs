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
public class UserServiceRegisterTests
{

    private Mock<UserManager<ApplicationUser>> _userManagerMock;
    private Mock<SignInManager<ApplicationUser>> _signInManagerMock;
    private ApplicationDbContext _contextMock;
    private Mock<IUserDetailRepository> _userDetailRepositoryMock;
    private Mock<EmailService> _emailServiceMock;
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<IOptions<TokenSettings>> _tokenSettingsMock;
    private Mock<IOptions<SmtpSettings>> _smtpSettingsMock;
    private UserService _userService;
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
        _emailServiceMock = new Mock<EmailService>(
            smtpSettings.Host,
            smtpSettings.Port,
            smtpSettings.User,
            smtpSettings.Password);
        _userRepositoryMock = new Mock<IUserRepository>();
        _tokenSettingsMock = new Mock<IOptions<TokenSettings>>();

        _userService = new UserService(
            _userManagerMock.Object,
            _signInManagerMock.Object,
            _contextMock,
            _tokenSettingsMock.Object.Value,
            null,
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
    public async Task UserRegisterAsync_ShouldReturnSuccess_WhenRegistrationIsSuccessful()
    {
        // Arrange
        var request = new UserRegisterRequest
        {
            Email = "test@example.com",
            Password = "Password123!",
            FirstName = "John",
            LastName = "Doe",
            Address = "123 Main St"
        };

        _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _userService.UserRegisterAsync(request);

        // Assert
        Assert.IsTrue(result.IsSucceed);
        Assert.IsTrue(result.Data);
        _userDetailRepositoryMock.Verify(repo => repo.AddUserDetailAsync(It.IsAny<UserDetail>()), Times.Once);
    }

    [Test]
    public async Task UserRegisterAsync_ShouldReturnError_WhenRegistrationFails()
    {
        // Arrange
        var request = new UserRegisterRequest
        {
            Email = "test@example.com",
            Password = "Password123!",
            FirstName = "John",
            LastName = "Doe",
            Address = "123 Main St"
        };

        var identityErrors = new IdentityError[]
        {
            new IdentityError { Code = "DuplicateUserName", Description = "Username already exists." }
        };

        _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed(identityErrors));

        // Act
        var result = await _userService.UserRegisterAsync(request);

        // Assert
        Assert.IsFalse(result.IsSucceed);
        CollectionAssert.Contains(result.Messages.Keys, "DuplicateUserName");
        _userDetailRepositoryMock.Verify(repo => repo.AddUserDetailAsync(It.IsAny<UserDetail>()), Times.Never);
    }
    
    [Test]
    public async Task UserRegisterAsync_ShouldReturnError_WhenPasswordIsWeak()
    {
        // Arrange
        var request = new UserRegisterRequest
        {
            Email = "test@example.com",
            Password = "weak", // Weak password
            FirstName = "John",
            LastName = "Doe",
            Address = "123 Main St"
        };

        var identityErrors = new IdentityError[]
        {
            new IdentityError { Code = "PasswordTooWeak", Description = "The password does not meet the strength requirements." }
        };

        _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed(identityErrors));

        // Act
        var result = await _userService.UserRegisterAsync(request);

        // Assert
        Assert.IsFalse(result.IsSucceed);
        Assert.IsTrue(result.Messages.ContainsKey("PasswordTooWeak"));
        Assert.AreEqual("The password does not meet the strength requirements.", result.Messages["PasswordTooWeak"][0]);
        _userManagerMock.Verify(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
    }
}