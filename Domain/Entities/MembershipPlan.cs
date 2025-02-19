using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class MembershipPlan : BaseEntity
{
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public virtual ICollection<AccountMembership> AccountMemberships { get; set; } = new List<AccountMembership>();
}
