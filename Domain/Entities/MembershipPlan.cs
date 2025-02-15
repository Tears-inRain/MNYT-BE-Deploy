using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class MembershipPlan
{
    public int MembershipPlanId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public virtual ICollection<AccountMembership> AccountMemberships { get; set; } = new List<AccountMembership>();
}
