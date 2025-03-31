using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.ScheduleUser
{
    public class TrueScheduleVM
    {
        public string? Title { get; set; }

        public string? Status { get; set; }

        public string? Type { get; set; }

        public string Tag { get; set; }

        public DateTime Date { get; set; }

        public int? Period { get; set; }

        public string? Note { get; set; }

        public int? PregnancyId { get; set; }
    }
}
