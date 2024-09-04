using Domain;

namespace Repository;

public interface IUserDetailRepository
{
    public Task AddUserDetailAsync(UserDetail userDetail);

    public Task<UserDetail> findByUserIdAsync(string userId);
    
    public Task UpdateUserDetailAsync(UserDetail userDetail);
    
    public Task<UserDetail> GetUserDetailAsync(string userId);
}