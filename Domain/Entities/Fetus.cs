using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public partial class Fetus
    {
        public int FetusId { get; set; }

        public string? Name { get; set; }

        public string? gender { get; set; }

        public int? PregnancyId { get; set; }
        public virtual Pregnancy? Pregnancy { get; set; }

        public virtual ICollection<FetusRecord> FetusRecords { get; set; } = new List<FetusRecord>();
    }
}
