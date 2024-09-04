using System.Runtime.CompilerServices;
using System.Security.Claims;
using Domain;
using Microsoft.AspNetCore.Identity;
using Repository;

[assembly: InternalsVisibleTo("Test")]


namespace UseCases.UserGroup
{
    public partial class UserService(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ApplicationDbContext applicationDbContext,
        TokenSettings tokenSettings, ITokenUtil _tokenUtil, IUserDetailRepository userDetailRepository, IEmailService emailService, IUserRepository userRepository)
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly TokenSettings _tokenSettings = tokenSettings;
        private readonly ApplicationDbContext _context = applicationDbContext;
        private readonly IUserDetailRepository _userDetailRepository = userDetailRepository;
        private readonly IEmailService _emailService = emailService;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ITokenUtil _tokenUtil = _tokenUtil;
        private readonly PasswordHasher<ApplicationUser> _passwordHasher =  new PasswordHasher<ApplicationUser>();
        protected internal async Task<UserLoginResponse> GenerateUserToken(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var token = _tokenUtil.GetToken(_tokenSettings, user, claims);

            await _userManager.RemoveAuthenticationTokenAsync(user, "REFRESHTOKENPROVIDER", "RefreshToken");
            var refreshToken = await _userManager.GenerateUserTokenAsync(user, "REFRESHTOKENPROVIDER", "RefreshToken");
            await _userManager.SetAuthenticationTokenAsync(user, "REFRESHTOKENPROVIDER", "RefreshToken", refreshToken);

            return new UserLoginResponse() { AccessToken = token, RefreshToken = refreshToken };
        }
    }
}