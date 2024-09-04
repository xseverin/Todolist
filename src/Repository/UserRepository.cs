using Repository;

namespace Repository;

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApplicationUser> GetUserByIdAsync(string userId)
    {
        return await _context.Users
            .Include(u => u.UserDetail)
            .SingleOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<ApplicationUser> GetUserByEmailAsync(string email)
    {
        return await _context.Users
            .Include(u => u.UserDetail)
            .SingleOrDefaultAsync(u => u.Email == email);
    }

    public async Task UpdateUserAsync(ApplicationUser user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
}