using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Blog
{
    public class CreateBlogPostDTO
    {
        public string? Category { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int? Period { get; set; }
    }
}
