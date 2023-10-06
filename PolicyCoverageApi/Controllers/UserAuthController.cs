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
using MySqlX.XDevAPI.Common;
using System.Dynamic;
using System.IO;
using ZstdSharp.Unsafe;

namespace PolicyCoverageApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAuthController : ControllerBase
    {
        private readonly IUserAuth _repo;

        public UserAuthController(IUserAuth userAuth)
        {
         
            _repo = userAuth;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromBody] PortalUser user)
        {
            try
            {
                return await _repo.AuthenticateAsync(user.UserName, user.Password);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
   








    }
}
