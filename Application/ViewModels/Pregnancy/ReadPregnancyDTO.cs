using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Pregnancy
{
    public class ReadPregnancyDTO
    {
        public int Id { get; set; }

        public int? AccountId { get; set; }

        public string? Type { get; set; }

        public string? Status { get; set; }

        public DateOnly? StartDate { get; set; }
    }
}
