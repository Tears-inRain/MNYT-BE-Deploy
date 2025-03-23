using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Blog
{
    public class CreatePostDTO
    {
        public string? Category { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public int? ImageId { get; set; }

        public string? ImageUrl { get; set; }

        public int? Period { get; set; }

        public string? Status { get; set; }

        public DateOnly? PublishedDay { get; set; }
    }
}
