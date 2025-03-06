using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.PregnancyStandard
{
    public class PregnacyStandardAddVM
    {
        public string? PregnancyType { get; set; }

        public string? Type { get; set; }

        public int? Period { get; set; }

        public decimal? Minimum { get; set; }

        public decimal? Maximum { get; set; }

        public string? Unit { get; set; }
    }
}
