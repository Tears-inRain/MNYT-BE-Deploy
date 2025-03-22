using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Media
{
    public class ReadMediaDTO
    {
        public int Id { get; set; }

        public string? Type { get; set; }

        public string? Url { get; set; }
    }
}
