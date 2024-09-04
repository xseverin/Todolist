using Domain;
using Microsoft.EntityFrameworkCore;

namespace Repository;


public class UserDetailRepository : IUserDetailRepository
{
    private readonly ApplicationDbContext _context;

    public UserDetailRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddUserDetailAsync(UserDetail userDetail)
    {
        await _context.UserDetails.AddAsync(userDetail);
        await _context.SaveChangesAsync();
    }
    
    public async Task<UserDetail> findByUserIdAsync(string userId)
    {
        return await _context.UserDetails.FirstOrDefaultAsync(x => x.UserId == userId);
    }
    
    public async Task UpdateUserDetailAsync(UserDetail userDetail)
    {
        _context.UserDetails.Update(userDetail);
        await _context.SaveChangesAsync();
    }
   
    public async Task<UserDetail> GetUserDetailAsync(string userId)
    {
        return await _context.UserDetails.SingleOrDefaultAsync(ud => ud.UserId == userId);
    }
}
