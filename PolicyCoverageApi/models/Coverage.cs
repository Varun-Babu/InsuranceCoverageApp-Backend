using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PolicyCoverageApi.Models;

public partial class Coverage
{
    public int CoverageId { get; set; }

    public string? CoverageName { get; set; }

    public string? CovCd { get; set; }

    public DateTime? EffectiveDt { get; set; }

    public DateTime? ExpirationDt { get; set; }

    public int? SortOrder { get; set; }

    public string? Description { get; set; }

    public sbyte? IsActive { get; set; }

    [JsonIgnore] 
    public virtual ICollection<Policycoverage>? Policycoverages { get; set; } 
}
