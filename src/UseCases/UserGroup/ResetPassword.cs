
using Microsoft.EntityFrameworkCore;
using Domain;
using Microsoft.AspNetCore.Identity;

namespace UseCases.UserGroup;

public partial class UserService
{
    public async Task<IdentityResult> ValidatePasswordWithAllValidatorsAsync(
        string password)
    {
        
        var results = new List<IdentityResult>();

      
        foreach (var validator in userManager.PasswordValidators)
        {
            var result = await validator.ValidateAsync(userManager, null, password);
            results.Add(result);

            if (!result.Succeeded)
            {
                return result;
            }
        }

        return IdentityResult.Success;
    }
    
    public async Task<Result<bool>> ResetPassword(string token,  ResetPasswordRequest request)
    {
        // Find user by the reset token (hashed)
        var hashedToken = _tokenUtil.HashToken(token);
        var user = await _context.Users.SingleOrDefaultAsync(u => u.ResetPasswordToken == hashedToken && u.ResetPasswordExpire > DateTime.UtcNow);

        if (user == null)
        {
            return new Result<bool>().SetError("token", "Invalid or expired token");
        }

        // Reset the password using UserManager
        var identityUser = await _userManager.FindByIdAsync(user.Id.ToString());
        if (identityUser == null)
        {
            return new Result<bool>().SetError("user", "User not found");
        }
        
        if (!ValidatePasswordWithAllValidatorsAsync(request.Password).Result.Succeeded)
        {
            return new Result<bool>().SetError("password", "Password is not valid");
        }
        
        var hashedPassword = _passwordHasher.HashPassword(user, request.Password);
   
        user.PasswordHash = hashedPassword;

        // Clear the reset token and expiration
        user.ResetPasswordToken = null;
        user.ResetPasswordExpire = null;
        await _context.SaveChangesAsync();

        return new Result<bool>().SetSuccess(true);
    }
    

    
}