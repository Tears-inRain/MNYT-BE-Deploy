using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Blog
{
    public class UpdateBlogPostDTO
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? Period { get; set; }
    }
}
