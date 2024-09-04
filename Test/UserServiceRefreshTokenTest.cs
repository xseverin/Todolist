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
using NUnit.Framework.Internal;
using Repository;
using UseCases;
using UseCases.UserGroup;


    [TestFixture]
    public class UserServiceRefreshTokenTests
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
    private Mock<ITokenUtil> _tokenUtilMock;

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

        _tokenUtilMock = new Mock<ITokenUtil>();
        _userService = new UserService(
            _userManagerMock.Object,
            _signInManagerMock.Object,
            _contextMock.Object,
            _tokenSettingsMock.Object.Value,
            _tokenUtilMock.Object,
            _userDetailRepositoryMock.Object,
            _emailServiceMock.Object,
            _userRepositoryMock.Object);
        
       
    }

    [Test]
    public async Task UserRefreshTokenAsync_ShouldReturnSuccess_WhenTokensAreValid()
    {
        // Arrange
        var request = new UserRefreshTokenRequest
        {
            AccessToken = "validAccessToken",
            RefreshToken = "validRefreshToken"
        };

        var claims = new List<Claim> { new Claim("UserName", "testuser") };
        var identity = new ClaimsIdentity(claims, "mock");
        var principal = new ClaimsPrincipal(identity);

        _tokenUtilMock.Setup(ts => ts.GetPrincipalFromExpiredToken(It.IsAny<TokenSettings>(), request.AccessToken))
            .Returns(principal);
        
        _tokenUtilMock.Setup(ts => ts.GetToken(It.IsAny<TokenSettings>(), It.IsAny<ApplicationUser>(), It.IsAny<List<Claim>>()))
            .Returns("newAccessToken");

        var user = new ApplicationUser { UserName = "testuser" };
        _userManagerMock.Setup(um => um.FindByNameAsync("testuser"))
            .ReturnsAsync(user);
        _userManagerMock.Setup(um => um.VerifyUserTokenAsync(user, "REFRESHTOKENPROVIDER", "RefreshToken", request.RefreshToken))
            .ReturnsAsync(true);
        _userManagerMock.Setup(um => um.GenerateUserTokenAsync(user, "REFRESHTOKENPROVIDER", "RefreshToken"))
            .ReturnsAsync("new_refresh_token");


        var userServiceMock = new Mock<UserService>(_userManagerMock.Object, null, null, _tokenSettingsMock.Object.Value, _tokenUtilMock.Object, null, null);
        //userServiceMock.Setup(us => us.GenerateUserToken(user))
        //    .ReturnsAsync(new UserLoginResponse { AccessToken = "newAccessToken", RefreshToken = "newRefreshToken" });


        // Act
        var result = await _userService.UserRefreshTokenAsync(request);

        //Console.WriteLine(result);
        //Test.Context.WriteLine(result);
        // Assert

        Assert.Multiple(() =>
        {
            Assert.That(result.IsSucceed, Is.True, "Expected the result to succeed.");
            Assert.That(result.Data.AccessToken, Is.EqualTo("newAccessToken"), "Expected the AccessToken to be 'newAccessToken'.");
            Assert.That(result.Data.RefreshToken, Is.EqualTo("new_refresh_token"), "Expected the RefreshToken to be 'new_refresh_token'.");
        });
    }

        [Test]
        public async Task UserRefreshTokenAsync_ShouldReturnError_WhenUserNotFound()
        {
            // Arrange
            var request = new UserRefreshTokenRequest
            {
                AccessToken = "invalidAccessToken",
                RefreshToken = "validRefreshToken"
            };

            _tokenUtilMock.Setup(ts =>  ts.GetPrincipalFromExpiredToken(It.IsAny<TokenSettings>(), It.IsAny<string>()))
                .Returns((ClaimsPrincipal)null);

            // Act
            var result = await _userService.UserRefreshTokenAsync(request);

            // Assert
            Assert.IsFalse(result.IsSucceed);
            Assert.AreEqual("User not found", result.Messages["email"][0]);
        }

        [Test]
        public async Task UserRefreshTokenAsync_ShouldReturnError_WhenRefreshTokenIsInvalid()
        {
            // Arrange
            var request = new UserRefreshTokenRequest
            {
                AccessToken = "validAccessToken",
                RefreshToken = "invalidRefreshToken"
            };
            
            var mockTokenUtil = new Mock<ITokenUtil>();

            var claims = new List<Claim> { new Claim("UserName", "testuser") };
            var identity = new ClaimsIdentity(claims, "mock");
            var principal = new ClaimsPrincipal(identity);

            _tokenUtilMock.Setup(tu => tu.GetPrincipalFromExpiredToken(It.IsAny<TokenSettings>(), It.IsAny<string>()))
                .Returns(principal);
             //_tokenUtilMock.Setup(tu => tu.GetPrincipalFromExpiredToken(It.IsAny<TokenSettings>(), request.AccessToken))
            //    .Returns(principal);

            var user = new ApplicationUser { UserName = "testuser" };
            _userManagerMock.Setup(um => um.FindByNameAsync("testuser"))
                .ReturnsAsync(user);
            _userManagerMock.Setup(um => um.VerifyUserTokenAsync(user, "REFRESHTOKENPROVIDER", "RefreshToken", request.RefreshToken))
                .ReturnsAsync(false);

            // Act
            var result = await _userService.UserRefreshTokenAsync(request);

            // Assert
            Assert.IsFalse(result.IsSucceed);
            Assert.AreEqual("Refresh token expired", result.Messages["token"][0]);
        }
    }
