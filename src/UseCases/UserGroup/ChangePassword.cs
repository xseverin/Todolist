using System.Security.Claims;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UseCases.UserGroup;

public partial class UserService
{
    public async Task<AppResponse<bool>> ChangePasswordAsync(ClaimsPrincipal User, ChangePasswordRequest request)
    {
        var user = await _userManager.FindByIdAsync(User.FindFirst("Id").Value);
        if (user == null)
        {
            return new AppResponse<bool>().SetErrorResponse("User", "User not found.");
        }

        var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        if (result.Succeeded)
        {
            return new AppResponse<bool>().SetSuccessResponse(true);
        }

        return new AppResponse<bool>().SetErrorResponse("Password", "Failed to change password.");
    }
}