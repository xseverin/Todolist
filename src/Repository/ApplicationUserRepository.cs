using Domain;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public interface IApplicationUserRepository
    {
        Task<ApplicationUser> FindIdByGmailAsync(string mail);
    }
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ApplicationUserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ApplicationUser> FindIdByGmailAsync(string mail)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Email == mail);


            return user;
        }

}
}