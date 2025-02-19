namespace Domain.Entities;

public partial class Pregnancy : BaseEntity
{
    public int? AccountId { get; set; }

    public string? Type { get; set; }

    public string? Status { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public virtual Account? Account { get; set; }

    public virtual ICollection<ScheduleUser> ScheduleUser { get; set; } = new List<ScheduleUser>();

    public virtual ICollection<Fetus> Fetus { get; set; } = new List<Fetus>();
}
