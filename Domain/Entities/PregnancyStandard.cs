using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class PregnancyStandard
{
    public int PregnancyStandardId { get; set; }

    public string? Type { get; set; }

    public int? Period { get; set; }

    public decimal? Minimum { get; set; }

    public decimal? Maximum { get; set; }

    public string? Unit { get; set; }
}
