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


        private readonly UserDbContext userDbContext;
        private readonly IUserPolicy userPolicy;

        public UserPolicyController(UserDbContext userDbContext, IUserPolicy userPolicy)
        {
            this.userDbContext = userDbContext;
            this.userPolicy = userPolicy;
        }


        #region add policy number unused
        /*

        [HttpPost("validate/{userid}/{policyNumber}/{chassisNumber}")]
        public async Task<IActionResult> ValidatePolicyAndChassis(int userid,long policyNumber, string chassisNumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(chassisNumber) || policyNumber <= 0)
                {
                  
                    return BadRequest(new { message = "Invalid request data" });
                }

                var policy = await userPolicy.GetPolicyByPolicyNumberAsync(policyNumber);

                if (policy == null)
                {
                    
                    return NotFound(new { message = "Policy not found" });
                }

                var vehicle = await userPolicy.GetVehicleByPolicyIdAsync(policy.PolicyId);

                if (vehicle == null || vehicle.ChasisNumber != chassisNumber)
                {
                   
                    return NotFound(new{ message = "ChassisNumber does not match" });
                }

                var userPolicyList = new UserPolicyList
                {
                    UserId = userid,
                    PolicyNumber = policyNumber
                };

               var userPolicyAdd = await userPolicy.AddUserPolicyAsync(userPolicyList);
                return Ok(new { message = "Policy Number Added" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        } */
        #endregion add policy no end here

        #region add policy number used

        [HttpPost("validate/{userId}/{policyNumber}/{chasisNumber}")]
        public async Task<IActionResult> AddPloicyNumber(int userId, int policyNumber, string chasisNumber)
        {
            try
            {
                var policy = await userPolicy.GetPolicyAsync(policyNumber);
                if (policy != null)
                {
                    var policyVehicleRecord = await userPolicy.GetPolicyvehicleAsync(policyNumber);
                    if (policyVehicleRecord != null)
                    {
                        var vehicle = await userPolicy.validateChasisNumber(policyVehicleRecord.VehicleId, chasisNumber);
                        if (vehicle != null)
                        {
                            var userPolicyToAdd = await userPolicy.AddPolicyNumberAsync(userId, policyNumber);
                            if (userPolicyToAdd != null)
                            {
                                return Ok(userPolicyToAdd);
                            }

                        }
                        return NotFound("ChasisNumber doesnt exist");
                    }
                    return NotFound("No Vehicle Found For Corresponding PolicyNumber");
                }
                return NotFound("Policy Number doesnt exist");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        #endregion

        #region Policy Number List


        [HttpGet("{userid}")]
        
        public async Task<IActionResult> GetPolicyNumbers([FromRoute] int userid)
        {
            try
            {
                var policyNumbers = await userPolicy.GetAllPolicyNumbersAsync(userid);

                if (policyNumbers == null || policyNumbers.Count == 0)
                {
                    return NoContent();
                }
                else
                {
                    return Ok(policyNumbers);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion end user policy list

        #region Delete Policy No

        [HttpDelete("{policyNo}")]
        public async Task<IActionResult> DeleteForm([FromRoute] long policyNo)
        {
            try
            {
                var exist = await userPolicy.GetUserPolicyByPolicyNumberAsync(policyNo);

                if (exist == null)
                {
                    
                    return NotFound(new { message = "Policy not found" });
                }

                await userPolicy.DeleteUserPolicyAsync(exist);
                

                return Ok(new { message = "Policy Deleted" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion delete end here

        #region Get Coverage By Policy No

        [HttpGet("GetCoveragesByPolicyNumber/{policyNumber}")]
        public async Task<ActionResult> GetCoveragesByPolicyNumber(long policyNumber)
        {
            try
            {
                    var policy = await userPolicy.GetPolicyByPolicyNumberAsync(policyNumber);

                    if (policy == null)
                    {
                        return NotFound("Policy not found.");
                    }

                    var coverage = await userPolicy.GetCoverageByPolicyIdAsync(policy.PolicyId);

                    if (coverage == null)
                    {
                        return NotFound("Coverage not found");
                    }

                    return Ok(coverage);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion get coverage ends here

    }
}










