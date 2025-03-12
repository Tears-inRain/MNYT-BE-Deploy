
namespace Domain.Entities;

public partial class FetusRecord : BaseEntity
{
    public int? Period { get; set; }

    public int? InputPeriod { get; set; }

    public decimal? Weight { get; set; }

    public decimal? Bpd { get; set; }

    public decimal? Length { get; set; }

    public decimal? Hc { get; set; }

    public DateOnly? Date { get; set; } //input date

    public int? FetusId { get; set; }
    public virtual Fetus? Fetus { get; set; }
}
