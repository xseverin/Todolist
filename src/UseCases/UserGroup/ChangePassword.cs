using System.Security.Claims;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UseCases.UserGroup;

public partial class UserService
{
    public async Task<Result<bool>> ChangePasswordAsync(ClaimsPrincipal User, ChangePasswordRequest request)
    {
        var user = await _userManager.FindByIdAsync(User.FindFirst("Id").Value);
        if (user == null)
        {
            return new Result<bool>().SetError("User", "User not found.");
        }

        var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        if (result.Succeeded)
        {
            return new Result<bool>().SetSuccess(true);
        }

        return new Result<bool>().SetError("Password", "Failed to change password.");
    }
}