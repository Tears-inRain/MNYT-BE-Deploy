using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Fetus
{
    public class FetusVM : BaseEntity
    {
        public string? Name { get; set; }

        public string? Gender { get; set; }

        public int? PregnancyId { get; set; }
    }
}
