using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int? AccountId { get; set; }

    public string? Method { get; set; }

    public string? Via { get; set; }

    public string? TransactionCode { get; set; }

    public virtual Account? Account { get; set; }
}
