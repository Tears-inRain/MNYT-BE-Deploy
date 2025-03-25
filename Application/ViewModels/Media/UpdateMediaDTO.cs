using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Media
{
    public class UpdateMediaDTO
    {
        public string? EntityType { get; set; }

        public int? EntityId { get; set; }

        public string? Type { get; set; }

        public string? Url { get; set; }
    }
}
