using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.FetusRecord
{
    public class FetusRecordAddVM
    {
        

        public int? InputPeriod { get; set; }

        public decimal? Weight { get; set; }

        public decimal? Bpd { get; set; }

        public decimal? Length { get; set; }

        public decimal? Hc { get; set; }

        public DateOnly? Date { get; set; } //input date
        public int? FetusId { get; set; }
    }
}
