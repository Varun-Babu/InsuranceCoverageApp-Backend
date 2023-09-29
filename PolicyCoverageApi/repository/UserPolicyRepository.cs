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
        public async Task<Policy> GetPolicyByPolicyNumberAsync(long policyNumber)
        {
            return await policyDbContext.Policies.FirstOrDefaultAsync(pn => pn.PolicyNumber == policyNumber);
        }

        public async Task<List<Coverage>> GetCoverageByPolicyIdAsync(string policyId)
        {
            var policyCov = await policyDbContext.Policycoverages.Where(pc => pc.PolicyId == policyId).ToListAsync();

            if (policyCov == null || policyCov.Count == 0)
            {
                return null;
            }

            var coverageIds = policyCov.Select(pc => pc.CoverageId).ToList();

            var coverages = await policyDbContext.Coverages.Where(c => coverageIds.Contains(c.CoverageId)).ToListAsync();
            return coverages;
        }

        #endregion

        #region Delete policy by policy no

        public async Task<UserPolicyList> GetUserPolicyByPolicyNumberAsync(long policyNumber)
        {
            return await userDbContext.userPolicyLists.FirstOrDefaultAsync(x => x.PolicyNumber == policyNumber);
        }

        public async Task DeleteUserPolicyAsync(UserPolicyList userPolicy)
        {
            userDbContext.userPolicyLists.Remove(userPolicy);
            await userDbContext.SaveChangesAsync();
        }

        #endregion

        #region Get User Policy List
        public async Task<List<UserPolicyList>> GetAllPolicyNumbersAsync(int userid)
        {
            return await userDbContext.userPolicyLists.Where(pl => pl.UserId == userid).ToListAsync();
        }
        #endregion

        #region Add Policy Number Unused
        /*
        public async Task<Policy> GetPolicyByPolicyNumberAsync(int policyNumber)
        {
            return await policyDbContext.Policies.FirstOrDefaultAsync(p => p.PolicyNumber == policyNumber);
        }

        public async Task<Vehicle> GetVehicleByPolicyIdAsync(string policyId)
        {
            var policyVehicle = await policyDbContext.Policyvehicles.FirstOrDefaultAsync(pv => pv.PolicyId == policyId);
            return await policyDbContext.Vehicles.FirstOrDefaultAsync(v => v.VehicleId == policyVehicle.VehicleId);
        }

        public async Task<UserPolicyList> AddUserPolicyAsync(UserPolicyList userPolicy)
        {
            userDbContext.userPolicyLists.Add(userPolicy);
            await userDbContext.SaveChangesAsync();
            return userPolicy;
            
        } */
        #endregion add policy no ends here


        #region Add PolicyNo
        public async Task<Policy> GetPolicyAsync(int policyNumber)
        {
            var policy = await policyDbContext.Policies.FirstOrDefaultAsync(p => p.PolicyNumber == policyNumber);
            if (policy == null)
            {
                return null;
            }
            return policy;
        }

        public async Task<Policyvehicle> GetPolicyvehicleAsync(int policyNumber)
        {
            var policy = await policyDbContext.Policies.FirstOrDefaultAsync(p => p.PolicyNumber == policyNumber);
            var policyVehicleRecord = await policyDbContext.Policyvehicles.FirstOrDefaultAsync(v => v.PolicyId == policy.PolicyId);
            if (policyVehicleRecord == null)
            {
                return null;
            }
            return policyVehicleRecord;
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

        public async Task<UserPolicyList> AddPolicyNumberAsync(int userId, int policyNumber)
        {
            UserPolicyList userPloicyList = new UserPolicyList
            {
               
                UserId = userId,
                PolicyNumber = policyNumber
            };
            await userDbContext.userPolicyLists.AddAsync(userPloicyList);
            await userDbContext.SaveChangesAsync();
            return userPloicyList;
        }

        #endregion


    }
}

