using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Payment
{
    public class PaymentMethodDTO
    {
        public int Id { get; set; }
        public string? Via { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}
