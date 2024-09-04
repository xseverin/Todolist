// UseCases/UserGroup/UserService.cs
using System.Security.Claims;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Identity;
using Repository;

namespace UseCases.UserGroup
{
    public partial class UserService
    {
        public async Task<AppResponse<bool>> UserLogoutAsync(ClaimsPrincipal user)
        {
            if (user.Identity?.IsAuthenticated ?? false)
            {
                var username = user.Claims.First(x => x.Type == "UserName").Value;
                var appUser = await _userManager.FindByNameAsync(username);
                if (appUser != null)
                {
                    await _userManager.UpdateSecurityStampAsync(appUser);
                }
                return new AppResponse<bool>().SetSuccessResponse(true);
            }
            return new AppResponse<bool>().SetSuccessResponse(true);
        }
    }
}