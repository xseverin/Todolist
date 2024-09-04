using System.ComponentModel.DataAnnotations;
using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Domain;
using Repository;
using UseCases;
using UseCases.UserGroup;
using UseCases.UserGroup;
namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController(UserService userService) : ControllerBase
    {
        private readonly UserService _userService = userService;

        [HttpPost]
        public async Task<AppResponse<bool>> Register(UserRegisterRequest req)
        {
            return await _userService.UserRegisterAsync(req);
        }

        [HttpPost]
        public async Task<AppResponse<UserLoginResponse>> Login(UserLoginRequest req)
        {
            return await _userService.UserLoginAsync(req);
        }

        [HttpPost]
        public async Task<AppResponse<UserRefreshTokenResponse>> RefreshToken(UserRefreshTokenRequest req)
        {
            return await _userService.UserRefreshTokenAsync(req);
        }

        [HttpPost]
        public async Task<AppResponse<bool>> Logout()
        {
            return await _userService.UserLogoutAsync(User);
        }
        
        [HttpPost]
        [Authorize]
        public async  Task<AppResponse<bool>> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            return await _userService.ChangePasswordAsync(User, request);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            return Ok(await  _userService.GetUserProfileAsync(User));
        }
        
        
        [HttpPut]
        [Authorize]
        public async Task<AppResponse<bool>> UpdateProfile(UserProfile request)
        {
            return await _userService.UpdateProfileAsync(User, request);
        }
        
        [HttpPost]
        public async Task<AppResponse<bool>> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            return await _userService.ForgotPassword(request);
        }

        [HttpPut]
        public async Task<AppResponse<bool>> ResetPassword(string token, [FromBody] ResetPasswordRequest request)
        {
            Console.Write("adsadsad");
            return await _userService.ResetPassword(token, request);
        }
    }

}