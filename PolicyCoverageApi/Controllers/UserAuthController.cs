using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PolicyCoverageApi.data;
using PolicyCoverageApi.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using PolicyCoverageApi.interfaces;
using PolicyCoverageApi.repository;

namespace PolicyCoverageApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAuthController : ControllerBase
    {
        private readonly IUserAuth _repo;
        private readonly UserDbContext userDbContext;

        public UserAuthController(IUserAuth userAuth,UserDbContext userDbContext)
        {
            _repo = userAuth;
            this.userDbContext = userDbContext;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromBody] PortalUser user)
        {
            if (user == null)
                return BadRequest();

            var userExist = await _repo.AuthenticateAsync(user.UserName, user.Password);

            if (userExist == null)
            {
               
                return NotFound(new { Message = "User Not Found" });
            }
            else
            {
                return Ok(new { message = "login success" });
            }
        }


    }
}
