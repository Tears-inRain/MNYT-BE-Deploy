using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.AccountMembership
{
    public class ReadAccountMembershipDTO
    {
        public int Id { get; set; }
        public int? AccountId { get; set; }
        public int? MembershipPlanId { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public decimal? Amount { get; set; }
        public string? Status { get; set; }
        public string? PaymentStatus { get; set; }
        public int? PaymentMethodId { get; set; }
    }
}
