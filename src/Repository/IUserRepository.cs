namespace Repository;

using System.Threading.Tasks;
using Domain;

public interface IUserRepository
{
    Task<ApplicationUser> GetUserByIdAsync(string userId);
    Task<ApplicationUser> GetUserByEmailAsync(string email);
    Task UpdateUserAsync(ApplicationUser user);
}