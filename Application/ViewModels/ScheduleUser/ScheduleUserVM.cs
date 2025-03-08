using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.ScheduleUser
{
    public class ScheduleUserVM : BaseEntity
    {
        public string? Title { get; set; }

        public string? Status { get; set; }

        public string? Type { get; set; }

        public DateOnly? Date { get; set; }

        public string? Note { get; set; }

        public int? PregnancyId { get; set; }
    }
}
