using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
public class UserServiceResetPasswordTests
{
    private Mock<UserManager<ApplicationUser>> _userManagerMock;
    private Mock<SignInManager<ApplicationUser>> _signInManagerMock;
    private ApplicationDbContext _context;
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
            Mock.Of<IUserStore<ApplicationUser>>(),
            null, null, null, null, null, null, null, null);

        _signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
            _userManagerMock.Object,
            Mock.Of<IHttpContextAccessor>(),
            Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(),
            null, null, null, null);

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new ApplicationDbContext(options);

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
            _context,
            _tokenSettingsMock.Object.Value,
            _tokenUtilMock.Object,
            _userDetailRepositoryMock.Object,
            _emailServiceMock.Object,
            _userRepositoryMock.Object);
    }

    
    [TearDown]
    public void TearDown()
    {
        // Clear the Users table to ensure a clean state for the next test
        var users = _context.Users.ToList();
        _context.Users.RemoveRange(users);
        _context.SaveChanges();
        
        _context.Dispose();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
 
        
        _context.Dispose();
    }

    // cannot check password validation because it is not possible to mock the PasswordValidator without inserted it in UseService constructor
    /*[Test]
    public async Task ResetPassword_ShouldReturnSuccess_WhenPasswordIsReset()
    {
        // Arrange
        var token = "validToken";
        var request = new ResetPasswordRequest { Password = "NewPassword123!" };
        var user = new ApplicationUser { Id = "1", ResetPasswordToken = "hashedToken", ResetPasswordExpire = DateTime.UtcNow.AddMinutes(10) };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        _userManagerMock.Setup(um => um.FindByIdAsync(user.Id))
            .ReturnsAsync(user);

        _userManagerMock.Setup(um => um.PasswordValidators)
            .Returns(new List<IPasswordValidator<ApplicationUser>> { new PasswordValidator<ApplicationUser>() });

        _userManagerMock.Setup(um => um.UpdateAsync(user))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _userService.ResetPassword(token, request);

        // Assert
        Assert.IsTrue(result.IsSucceed);
        Assert.IsTrue(result.Data);
    }*/

    [Test]
    public async Task ResetPassword_ShouldReturnError_WhenUserNotFoundByToken()
    {
        // Arrange
        var token = "invalidToken";
        var request = new ResetPasswordRequest { Password = "NewPassword123!" };

        // No need to add a user in this case

        // Act
        var result = await _userService.ResetPassword(token, request);

        // Assert
        Assert.IsFalse(result.IsSucceed);
        Assert.AreEqual("Invalid or expired token", result.Messages["token"][0]);
    }

    [Test]
    public async Task ResetPassword_ShouldReturnError_WhenUserNotFoundById()
    {
        // Arrange
        var token = "validToken";
        var request = new ResetPasswordRequest { Password = "NewPassword123!" };
        var user = new ApplicationUser { Id = "1", ResetPasswordToken = "hashedToken", ResetPasswordExpire = DateTime.UtcNow.AddMinutes(10) };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        _userManagerMock.Setup(um => um.FindByIdAsync(user.Id))
            .ReturnsAsync((ApplicationUser)null);
        
        _tokenUtilMock.Setup(um => um.HashToken(It.IsAny<string>()))
            .Returns("hashedToken");

        // Act
        var result = await _userService.ResetPassword(token, request);

        // Assert
        Assert.IsFalse(result.IsSucceed);
        Assert.AreEqual("User not found", result.Messages["user"][0]);
    }

    /*[Test]
    public async Task ResetPassword_ShouldReturnError_WhenPasswordValidationFails()
    {
        // Arrange
        var token = "validToken";
        var request = new ResetPasswordRequest { Password = "weak" };
        var user = new ApplicationUser { Id = "1", ResetPasswordToken = "hashedToken", ResetPasswordExpire = DateTime.UtcNow.AddMinutes(10) };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        _userManagerMock.Setup(um => um.FindByIdAsync(user.Id))
            .ReturnsAsync(user);

        _userManagerMock.Setup(um => um.PasswordValidators)
            .Returns(new List<IPasswordValidator<ApplicationUser>> { new PasswordValidator<ApplicationUser>() });

        // Act
        var result = await _userService.ResetPassword(token, request);

        // Assert
        Assert.IsFalse(result.IsSucceed);
        Assert.AreEqual("Password is not valid", result.Messages["password"][0]);
    }*/
}
