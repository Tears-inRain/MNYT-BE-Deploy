using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class FetusRecord
{
    public int FetusRecordId { get; set; }

    public int? PregnancyId { get; set; }

    public int? FetusId { get; set; }

    public int? Period { get; set; }

    public int? InputPeriod { get; set; }

    public decimal? Weight { get; set; }

    public decimal? Bpd { get; set; }

    public decimal? Length { get; set; }

    public decimal? Hc { get; set; }

    public DateOnly? Date { get; set; }

    public virtual Pregnancy? Pregnancy { get; set; }
}
