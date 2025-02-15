using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class AccountMembership
{
    public int MembershipId { get; set; }

    public int? AccountId { get; set; }

    public int? MembershipPlanId { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public decimal? PaymentAmount { get; set; }

    public string? PaymentStatus { get; set; }

    public string? PaymentMethod { get; set; }

    public string? TransactionCode { get; set; }

    public virtual Account? Account { get; set; }

    public virtual MembershipPlan? MembershipPlan { get; set; }
}
