using Microsoft.AspNetCore.Mvc;
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

        public async Task<IActionResult> AuthenticateAsync(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                return new NotFoundObjectResult(new { Message = "User Not Found" });
            }

            var userExist = await _context.portalUsers
                .FirstOrDefaultAsync(x => x.UserName == userName && x.Password == password);

            if (userExist == null)
            {
                return new NotFoundObjectResult(new { Message = "User Not Found" });
            }
            else
            {
                return new OkObjectResult(userExist);
            }
        }
    }
}
