using Domain;
using Microsoft.AspNetCore.Identity;
using Repository;

namespace UseCases.UserGroup
{

    public partial class UserService
    {

        public async Task<Result<bool>> UserRegisterAsync(UserRegisterRequest request)
        {
            var user = new ApplicationUser()
            {
                UserName = request.Email,
                Email = request.Email
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                var userDetail = new UserDetail
                {
                    Id = Guid.NewGuid(),
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    //Address = request.Address,
                    UserId = user.Id
                };

                await _userDetailRepository.AddUserDetailAsync(userDetail);

                return new Result<bool>().SetSuccess(true);
            }
            else
            {
                return new Result<bool>().SetError(GetRegisterErrors(result));
            }
        }

        private Dictionary<string, string[]> GetRegisterErrors(IdentityResult result)
        {
            var errorDictionary = new Dictionary<string, string[]>(1);

            foreach (var error in result.Errors)
            {
                string[] newDescriptions;

                if (errorDictionary.TryGetValue(error.Code, out var descriptions))
                {
                    newDescriptions = new string[descriptions.Length + 1];
                    Array.Copy(descriptions, newDescriptions, descriptions.Length);
                    newDescriptions[descriptions.Length] = error.Description;
                }
                else
                {
                    newDescriptions = [error.Description];
                }

                errorDictionary[error.Code] = newDescriptions;
            }

            return errorDictionary;
        }
    }
}
