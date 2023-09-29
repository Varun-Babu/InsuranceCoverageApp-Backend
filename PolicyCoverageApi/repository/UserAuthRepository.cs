using Microsoft.EntityFrameworkCore;
using PolicyCoverageApi.data;
using PolicyCoverageApi.interfaces;
using PolicyCoverageApi.models;

namespace PolicyCoverageApi.repository
{
    public class UserAuthRepository : IUserAuth
    {
        private readonly UserDbContext _context;

        public UserAuthRepository(UserDbContext userDbContext)
        {
            _context = userDbContext;
        }
        public async Task<PortalUser> AuthenticateAsync(string userName, string password)
        {
            return await _context.portalUsers.FirstOrDefaultAsync(x => x.UserName == userName && x.Password == password);
        }
    }
}
