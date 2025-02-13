using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class MembershipPlants
    {
        public int Id { get; set; }
        public string MembershipPlantsName { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
    }
}
