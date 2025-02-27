using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Payment
{
    public class CreatePaymentDTO
    {
        public int AccountId { get; set; }
        public int MembershipPlanId { get; set; }
    }
}
