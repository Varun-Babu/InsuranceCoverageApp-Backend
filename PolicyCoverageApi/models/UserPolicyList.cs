using Microsoft.JSInterop;
using System.Text.Json.Serialization;

namespace PolicyCoverageApi.models
{
    public class UserPolicyList
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int PolicyNumber { get; set; }

        [JsonIgnore]
        public PortalUser? User { get; set; }
    }
}
