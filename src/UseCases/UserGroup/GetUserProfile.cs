using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Domain;
using Microsoft.AspNetCore.Identity;

namespace UseCases.UserGroup;



public partial class UserService
{
    public async Task<UserProfile> GetUserProfileAsync(ClaimsPrincipal user)
    {
        var usr = await _userManager.GetUserAsync(user);
        var email = usr?.Email;
        var userDetail = await _userDetailRepository.findByUserIdAsync(user.FindFirst("Id").Value);

        var userProfile = new UserProfile
        {
            Email = email,
            FirstName = userDetail.FirstName,
            LastName = userDetail.LastName,
            Address = userDetail.Address
        };

        return userProfile;
        
    }
}