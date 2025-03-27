using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Post
{
    public class TopAuthorDTO
    {
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public int PostCount { get; set; }
    }
}
