using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class Pregnancy
{
    public int PregnancyId { get; set; }

    public int? AccountId { get; set; }

    public string? Type { get; set; }

    public string? Status { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public virtual Account? Account { get; set; }

    public virtual ICollection<ScheduleUser> ScheduleUser { get; set; } = new List<ScheduleUser>();

    public virtual ICollection<Fetus> Fetus { get; set; } = new List<Fetus>();
}
