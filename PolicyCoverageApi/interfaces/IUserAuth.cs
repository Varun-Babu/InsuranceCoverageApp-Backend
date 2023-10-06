using Microsoft.AspNetCore.Mvc;
using PolicyCoverageApi.models;

namespace PolicyCoverageApi.interfaces
{
    public interface IUserAuth 
    {
        Task<IActionResult> AuthenticateAsync(string userName, string password);
    }
}
