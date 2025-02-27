namespace Domain.Entities;

public partial class AccountMembership : BaseEntity
{
    public int? AccountId { get; set; }

    public int? MembershipPlanId { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public decimal? Amount { get; set; }

    public string? Status { get; set; }

    public virtual Account? Account { get; set; }

    public virtual MembershipPlan? MembershipPlan { get; set; }

    public int? PaymentMethodId { get; set; }
    public virtual PaymentMethod PaymentMethods { get; set; }
}
