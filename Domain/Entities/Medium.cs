using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class Medium
{
    public int MediaId { get; set; }

    public string? Type { get; set; }

    public string? Url { get; set; }
}
