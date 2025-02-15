using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class ScheduleTemplate
{
    public int ScheduleTemplateId { get; set; }

    public int Period { get; set; }

    public string? Type { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }
}
