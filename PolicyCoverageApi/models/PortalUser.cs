using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PolicyCoverageApi.models
{
    public class PortalUser
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        [JsonIgnore]
        public ICollection<UserPolicyList>? Policies { get; set; }
    }
}
