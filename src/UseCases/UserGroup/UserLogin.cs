using Repository;
namespace UseCases.UserGroup
{
    public class UserLoginRequest
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
    }
    public class UserLoginResponse
    {
        public string AccessToken { get; set; } = "";
        public string RefreshToken { get; set; } = "";
    }
    public partial class UserService
    {
        public async Task<Result<UserLoginResponse>> UserLoginAsync(UserLoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return new Result<UserLoginResponse>().SetError("error", "Email or password is incorrect");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, true);
            
            if (!result.Succeeded)
                return new Result<UserLoginResponse>().SetError("error", "Email or password is incorrect");
            
            var token = await GenerateUserToken(user);
            return new Result<UserLoginResponse>().SetSuccess(token);
        }

    }
}
