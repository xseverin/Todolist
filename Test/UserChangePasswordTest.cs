using System.Security.Claims;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using UseCases.UserGroup;



    [TestFixture]
    public class UserChangePasswordTests
    {
        private Mock<UserManager<ApplicationUser>> _userManagerMock;
        private UserService _userService;

        [SetUp]
        public void SetUp()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            _userService = new UserService(_userManagerMock.Object, null, null, null,null,
                null, null, null);
        }

        [Test]
        public async Task ChangePasswordAsync_ShouldReturnErrorResponse_WhenUserNotFound()
        {
            // Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim("Id", "user-id-123") }));
            var request = new ChangePasswordRequest { CurrentPassword = "oldPassword", NewPassword = "newPassword" };

            _userManagerMock.Setup(um => um.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null);

            // Act
            var result = await _userService.ChangePasswordAsync(user, request);

            // Assert
            Assert.IsFalse(result.IsSucceed);
            Assert.AreEqual("User not found.", result.Messages["User"][0]);
        }

        [Test]
        public async Task ChangePasswordAsync_ShouldReturnSuccessResponse_WhenPasswordChangedSuccessfully()
        {
            // Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim("Id", "user-id-123") }));
            var request = new ChangePasswordRequest { CurrentPassword = "oldPassword", NewPassword = "newPassword" };
            var applicationUser = new ApplicationUser();

            _userManagerMock.Setup(um => um.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(applicationUser);
            _userManagerMock.Setup(um => um.ChangePasswordAsync(applicationUser, request.CurrentPassword, request.NewPassword))
                            .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _userService.ChangePasswordAsync(user, request);

            // Assert
            Assert.IsTrue(result.IsSucceed);
            Assert.AreEqual(true, result.Data);
        }

        [Test]
        public async Task ChangePasswordAsync_ShouldReturnErrorResponse_WhenPasswordChangeFails()
        {
            // Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim("Id", "user-id-123") }));
            var request = new ChangePasswordRequest { CurrentPassword = "oldPassword", NewPassword = "newPassword" };
            var applicationUser = new ApplicationUser();

            _userManagerMock.Setup(um => um.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(applicationUser);
            _userManagerMock.Setup(um => um.ChangePasswordAsync(applicationUser, request.CurrentPassword, request.NewPassword))
                            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Failed to change password." }));

            // Act
            var result = await _userService.ChangePasswordAsync(user, request);

            // Assert
            Assert.IsFalse(result.IsSucceed);
            Assert.AreEqual("Failed to change password.", result.Messages["Password"][0]);
        }
    }
