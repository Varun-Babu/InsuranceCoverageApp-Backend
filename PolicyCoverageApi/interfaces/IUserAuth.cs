using PolicyCoverageApi.models;

namespace PolicyCoverageApi.interfaces
{
    public interface IUserAuth 
    {
        Task<PortalUser> AuthenticateAsync(string userName, string password);
    }
}
