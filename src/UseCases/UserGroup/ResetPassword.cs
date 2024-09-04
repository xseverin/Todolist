using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using Domain;
using Microsoft.AspNetCore.Identity;

namespace UseCases.UserGroup;

public partial class UserService
{
    public async Task<IdentityResult> ValidatePasswordWithAllValidatorsAsync(
        string password)
    {
        // Список результатов проверки для каждого валидатора
        var results = new List<IdentityResult>();

        // Перебираем все валидаторы пароля, настроенные в UserManager
        foreach (var validator in userManager.PasswordValidators)
        {
            var result = await validator.ValidateAsync(userManager, null, password);
            results.Add(result);

            // Если один из валидаторов возвращает ошибки, мы можем сразу вернуться с ними
            if (!result.Succeeded)
            {
                return result;
            }
        }

        // Если все валидаторы прошли успешно, возвращаем успешный результат
        return IdentityResult.Success;
    }
    
    public async Task<AppResponse<bool>> ResetPassword(string token,  ResetPasswordRequest request)
    {
        // Find user by the reset token (hashed)
        var hashedToken = _tokenUtil.HashToken(token);
        var user = await _context.Users.SingleOrDefaultAsync(u => u.ResetPasswordToken == hashedToken && u.ResetPasswordExpire > DateTime.UtcNow);

        if (user == null)
        {
            return new AppResponse<bool>().SetErrorResponse("token", "Invalid or expired token");
        }

        // Reset the password using UserManager
        var identityUser = await _userManager.FindByIdAsync(user.Id.ToString());
        if (identityUser == null)
        {
            return new AppResponse<bool>().SetErrorResponse("user", "User not found");
        }

        //var token_ = await _userManager.GeneratePasswordResetTokenAsync(user);
        //var resetResult = await _userManager.ResetPasswordAsync(identityUser, token_, request.Password);
       // if (!resetResult.Succeeded)
        //{
         //   return new AppResponse<bool>().SetErrorResponse("password", string.Join(", ", resetResult.Errors.Select(e => e.Description)));
        //}
        
        if (!ValidatePasswordWithAllValidatorsAsync(request.Password).Result.Succeeded)
        {
            return new AppResponse<bool>().SetErrorResponse("password", "Password is not valid");
        }
        
        var hashedPassword = _passwordHasher.HashPassword(user, request.Password);
   
        user.PasswordHash = hashedPassword;

        // Clear the reset token and expiration
        user.ResetPasswordToken = null;
        user.ResetPasswordExpire = null;
        await _context.SaveChangesAsync();

        return new AppResponse<bool>().SetSuccessResponse(true);
    }
    

    
}