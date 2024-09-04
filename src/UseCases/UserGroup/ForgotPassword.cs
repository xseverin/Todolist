using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using Azure.Core;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace UseCases.UserGroup;
public partial class UserService
{
   

    
    public async Task<Result<bool>> ForgotPassword([FromBody] Domain.ForgotPasswordRequest request)
    {
        var user = await _userRepository.GetUserByEmailAsync(request.Email);
        if (user == null)
        {
            return new Result<bool>().SetError("User", "User not found");
        }

        var resetToken = _tokenUtil.GenerateResetToken();
        user.ResetPasswordToken = _tokenUtil.HashToken(resetToken);
        user.ResetPasswordExpire = DateTime.UtcNow.AddMinutes(10);
        await _context.SaveChangesAsync();

        var resetUrl = $"http://localhost:1000/ResetPassword/{resetToken}";
        var message = $"You are receiving this email because you (or someone else) have requested the reset of a password. Please make a request to <a href=\"{resetUrl}\">this link</a>";

        _emailService.SendEmail("todolistemaily@gmail.com", "Todo App", user.Email, user.UserName, "Password reset token", message);

        return new Result<bool>().SetSuccess(true);
    }
}