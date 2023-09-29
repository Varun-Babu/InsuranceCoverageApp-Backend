using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PolicyCoverageApi.Models;

public partial class Policycoverage
{
    public string PolicyCoverageId { get; set; } 

    public string? PolicyId { get; set; }

    public int? CoverageId { get; set; }

    public int? Limit { get; set; }

    [JsonIgnore]
    public virtual Coverage? Coverage { get; set; }

    [JsonIgnore]
    public virtual Policy? Policy { get; set; }
}
