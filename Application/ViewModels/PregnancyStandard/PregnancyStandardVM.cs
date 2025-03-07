using Domain;

namespace Application.ViewModels.PregnancyStandard;

public partial class PregnancyStandardVM : BaseEntity
{
    public string? PregnancyType { get; set; }

    public string? Type { get; set; }

    public int? Period { get; set; }

    public decimal? Minimum { get; set; }

    public decimal? Maximum { get; set; }

    public string? Unit { get; set; }
}