using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models;

public partial class PaymentMethod
{
    public int PaymentMethodId { get; set; }[Key]

    public string? Method { get; set; }

    public string? Via { get; set; }

    public string? TransactionCode { get; set; }

    public int? AccountMembershipId { get; set; }

    public virtual AccountMembership? AccountMembership { get; set; }
}
