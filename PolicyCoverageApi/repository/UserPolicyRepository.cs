using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PolicyCoverageApi.data;
using PolicyCoverageApi.models;
using PolicyCoverageApi.interfaces;
using PolicyCoverageApi.Models;

namespace PolicyCoverageApi.repository
{
    public class UserPolicyRepository : IUserPolicy
    {
        #region dependency injection

        private readonly UserDbContext userDbContext;
        private readonly PolicyDbContext policyDbContext;

        public UserPolicyRepository(UserDbContext userDbContext, PolicyDbContext policyDbContext)
        {

            this.userDbContext = userDbContext;
            this.policyDbContext = policyDbContext;
        }
        #endregion

        #region Get Coverage By Policy No
        public async Task<ActionResult<List<Coverage>>> GetCoveragesByPolicyNumber(int policyNumber)
        {
            var policy = await policyDbContext.Policies.FirstOrDefaultAsync(pn => pn.PolicyNumber == policyNumber);

            if (policy == null)
            {
                return new NotFoundObjectResult("Policy not found.");
            }

            var policyCov = await policyDbContext.Policycoverages.Where(pc => pc.PolicyId == policy.PolicyId).ToListAsync();

            if (policyCov == null || policyCov.Count == 0)
            {
                return new NotFoundObjectResult("Coverage not found");
            }

            var coverageIds = policyCov.Select(pc => pc.CoverageId).ToList();

            var coverages = await policyDbContext.Coverages.Where(c => coverageIds.Contains(c.CoverageId)).ToListAsync();

            return coverages;
        }


        #endregion

        #region Delete policy by policy no

        public async Task<IActionResult> DeleteUserPolicy(int policyNumber)
        {
            var policyToDelete = await userDbContext.userPolicyLists.FirstOrDefaultAsync(x => x.PolicyNumber == policyNumber);

            if (policyToDelete != null)
            {
                userDbContext.userPolicyLists.Remove(policyToDelete);
                await userDbContext.SaveChangesAsync();
                return new OkObjectResult(new { message = "Policy Deleted" });
            }

            return new NotFoundObjectResult(new { message = "Policy not found" });
        }


        #endregion

        #region Add PolicyNo
        public async Task<Policy> GetPolicy(int policyNumber)
        {
            var policy = await policyDbContext.Policies.FirstOrDefaultAsync(p => p.PolicyNumber == policyNumber);
            if (policy == null)
            {
                return null;
            }
            return policy;
        }

        public async Task<Vehicle> validateChasisNumber(string vehicleId, string chasisNumber)
        {
            var vehicle = await policyDbContext.Vehicles.FirstOrDefaultAsync(v => v.VehicleId == vehicleId && v.ChasisNumber == chasisNumber);
            if (vehicle == null)
            {
                return null;
            }
            return vehicle;

        }
        public async Task<Policyvehicle> GetPolicyvehicle(string policyId)
        {
            var policyVehicleRecord = await policyDbContext.Policyvehicles.FirstOrDefaultAsync(v => v.PolicyId == policyId);
            if (policyVehicleRecord == null)
            {
                return null;
            }
            return policyVehicleRecord;
        }

        public async Task<IActionResult> AddPolicyNumberAndValidate(UserPolicyList policyList, string chasisNumber)
        {

            int userId = policyList.UserId;
            int policyNumber = policyList.PolicyNumber;
        
            var existingUserPolicy = await userDbContext.userPolicyLists
                .FirstOrDefaultAsync(up => up.UserId == userId && up.PolicyNumber == policyNumber);

            if (existingUserPolicy != null)
            {
                return new BadRequestObjectResult( new { message = "Policy Number already exists for the user." });
            }

            var policy = await GetPolicy(policyNumber);
            var policyVehicleRecord = policy != null ? await GetPolicyvehicle(policy.PolicyId) : null;
            var vehicle = policyVehicleRecord != null ? await validateChasisNumber(policyVehicleRecord.VehicleId, chasisNumber) : null;

            if (policy == null || policyVehicleRecord == null || vehicle == null)
            {
                return new NotFoundObjectResult(new { message = "Invalid Policy or ChasisNumber" });
            }

            var userPolicyList = new UserPolicyList
            {
                UserId = userId,
                PolicyNumber = policyNumber
            };

            await userDbContext.userPolicyLists.AddAsync(userPolicyList);
            await userDbContext.SaveChangesAsync();

            return new OkObjectResult(new {message ="ok"});
        }

        #endregion

        #region get policy number

        public async Task<ActionResult<List<UserPolicyList>>> GetAllPolicyNumbers(int userid)
        {
            var policyNumbers = await userDbContext.userPolicyLists.Where(pl => pl.UserId == userid).ToListAsync();

            if (policyNumbers == null || policyNumbers.Count == 0)
            {
                return null;
            }

            return policyNumbers;
        }

        #endregion



    }
}

