using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI.Relational;
using Org.BouncyCastle.Asn1.Ocsp;
using PolicyCoverageApi.data;
using PolicyCoverageApi.models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;
using System.Security.Policy;
using PolicyCoverageApi.Models;
using PolicyCoverageApi.interfaces;
using System.Text.RegularExpressions;
using System.Reflection.Metadata.Ecma335;
using Microsoft.Win32;
using PolicyCoverageApi.repository;
using System.Threading.Channels;

namespace PolicyCoverageApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserPolicyController : ControllerBase
    {

        private readonly IUserPolicy userPolicy;

        public UserPolicyController( IUserPolicy userPolicy)
        {
            this.userPolicy = userPolicy;
        }


        #region Delete Policy No

        [HttpDelete("{policyNo}")]
        public async Task<IActionResult> DeletePolicyNumber([FromRoute] int policyNo)
        {
            try
            {
                var result = await userPolicy.DeleteUserPolicy(policyNo);
                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        #endregion delete end here

        #region Get Coverage By Policy No

        [HttpGet("GetCoveragesByPolicyNumber/{policyNumber}")]
        public async Task<ActionResult<List<Coverage>>> GetCoveragesByPolicyNumber([FromRoute]int policyNumber)
        {
            try
            {
                var result = await userPolicy.GetCoveragesByPolicyNumber(policyNumber);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        #endregion get coverage ends here

        #region new add policy number
        [HttpPost("validate/{chasisNumber}")]
        public async Task<IActionResult> AddPolicyNumberAndValidate([FromBody] UserPolicyList policyList,[FromRoute] string chasisNumber)
        {
            try
            {
                var result = await userPolicy.AddPolicyNumberAndValidate(policyList, chasisNumber);
                return result;
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult("An error occurred while processing the request.");
            }

        }
        #endregion add policy ends here

        #region policy no list

        [HttpGet("GetAllPolicyNumbers/{userid}")]
        public async Task<ActionResult<List<UserPolicyList>>> GetAllPolicyNumbers([FromRoute]int userid)
        {
            try
            {
                var response =  await userPolicy.GetAllPolicyNumbers(userid);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion
    }
}










