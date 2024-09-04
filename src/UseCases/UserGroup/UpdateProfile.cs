using System.Security.Claims;
using Domain;

namespace UseCases.UserGroup;

public partial class UserService
{
    public async Task<AppResponse<bool>> UpdateProfileAsync(ClaimsPrincipal User, UserProfile request)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return new AppResponse<bool>().SetErrorResponse("user", "User not found."); 
        }

      
        user.UserName = request.Email; // Updating email

        // Check if the email has changed and update it
        if (user.Email != request.Email)
        {
            var emailResult = await _userManager.SetEmailAsync(user, request.Email);
            if (!emailResult.Succeeded)
            {
                return new AppResponse<bool>().SetErrorResponse("email", "Failed to update email."); 
            }
        }

        
        // Save changes to the user manager and repository
        var identityResult = await _userManager.UpdateAsync(user);
        if (!identityResult.Succeeded)
        {
            return new AppResponse<bool>().SetErrorResponse("user profile", "Failed to update user profile."); 
        }

        
        // Update other user properties
        var userDetail = await _userDetailRepository.GetUserDetailAsync(user.Id);
     
            userDetail.FirstName = request.FirstName;
            userDetail.LastName = request.LastName;
            userDetail.Address = request.Address;
            _userDetailRepository.UpdateUserDetailAsync(userDetail);
        

        //await _userRepository.SaveChangesAsync();

        return new AppResponse<bool>().SetSuccessResponse(true);

    }
}