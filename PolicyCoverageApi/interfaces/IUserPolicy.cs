using Microsoft.AspNetCore.Mvc;
using PolicyCoverageApi.models;
using PolicyCoverageApi.Models;

namespace PolicyCoverageApi.interfaces
{
    public interface IUserPolicy
    {
        #region Get coverage by Policy No
        Task<Policy> GetPolicyByPolicyNumberAsync(long policyNumber);
        Task<List<Coverage>> GetCoverageByPolicyIdAsync(string policyId);

        #endregion end here

        #region delete User Policy List by Policy No
        Task<UserPolicyList> GetUserPolicyByPolicyNumberAsync(long policyNumber);
        Task DeleteUserPolicyAsync(UserPolicyList userPolicy);

        #endregion end here

        #region User Policy List

        Task<List<UserPolicyList>> GetAllPolicyNumbersAsync(int userid);

        #endregion

        #region Add Policy Number unused
        //Task<Policy> GetPolicyByPolicyNumberAsync(int policyNumber);
        //Task<Vehicle> GetVehicleByPolicyIdAsync(string policyId);
        //Task<UserPolicyList> AddUserPolicyAsync(UserPolicyList userPolicy);

        #endregion

        #region Policy Number ADD 
        public Task<Policy> GetPolicyAsync(int policyNumber);
        public Task<Policyvehicle> GetPolicyvehicleAsync(int policyNumber);
        public Task<Vehicle> validateChasisNumber(string vehicleId, string chasisNumber);
        public Task<UserPolicyList> AddPolicyNumberAsync(int userId, int policyNumber);

        #endregion
    }
}
