using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.ScheduleTemplate
{
    public class ScheduleTemplateAddVM
    {
        public int Period { get; set; }

        public string? Type { get; set; }

        public string Tag { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }
    }
}
