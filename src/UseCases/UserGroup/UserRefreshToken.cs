using Repository;

namespace UseCases.UserGroup
{
    public class UserRefreshTokenRequest
    {
        public string AccessToken { get; set; } = "";
        public string RefreshToken { get; set; } = "";
    }
    public class UserRefreshTokenResponse
    {
        public string AccessToken { get; set; } = "";
        public string RefreshToken { get; set; } = "";
    }
    public partial class UserService
    {
        public async Task<AppResponse<UserRefreshTokenResponse>> UserRefreshTokenAsync(UserRefreshTokenRequest request)
        {
            var principal = _tokenUtil.GetPrincipalFromExpiredToken(_tokenSettings, request.AccessToken);
            if (principal == null || principal.FindFirst("UserName")?.Value == null)
            {
                return new AppResponse<UserRefreshTokenResponse>().SetErrorResponse("email", "User not found");
            }
          
            var user = await _userManager.FindByNameAsync(principal.FindFirst("UserName")?.Value ?? "");
            if (user == null)
            {
                return new AppResponse<UserRefreshTokenResponse>().SetErrorResponse("email", "User not found");
            }
         
            if (!await _userManager.VerifyUserTokenAsync(user, "REFRESHTOKENPROVIDER", "RefreshToken", request.RefreshToken))
            {
                return new AppResponse<UserRefreshTokenResponse>().SetErrorResponse("token", "Refresh token expired");
            }
            
            
            var token = await GenerateUserToken(user);
            return new AppResponse<UserRefreshTokenResponse>().SetSuccessResponse(new UserRefreshTokenResponse() { AccessToken = token.AccessToken, RefreshToken = token.RefreshToken });
            
           
        }
    }
}
