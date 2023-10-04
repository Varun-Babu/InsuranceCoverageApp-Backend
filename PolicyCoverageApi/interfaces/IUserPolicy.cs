using Microsoft.AspNetCore.Mvc;
using PolicyCoverageApi.models;
using PolicyCoverageApi.Models;

namespace PolicyCoverageApi.interfaces
{
    public interface IUserPolicy
    {
        #region Get coverage by Policy No
        Task<ActionResult<List<Coverage>>> GetCoveragesByPolicyNumber(int policyNumber);

        #endregion end here

        #region delete User Policy List by Policy No
        Task<IActionResult> DeleteUserPolicy(int policyNumber);

        #endregion end here

        #region Policy Number ADD 
         Task<Policy> GetPolicy(int policyNumber);
         Task<Policyvehicle> GetPolicyvehicle(string policyId);
         Task<Vehicle> validateChasisNumber(string vehicleId, string chasisNumber);
        Task<IActionResult> AddPolicyNumberAndValidate(UserPolicyList userPolicyList, string chasisNumber);

        #endregion

        #region get policy no

        Task<ActionResult<List<UserPolicyList>>> GetAllPolicyNumbers(int userid);

        #endregion 



    }

}
