using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Blog
{
    public class ReadBlogPostDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int? AuthorId { get; set; }
        public string? AuthorName { get; set; }
        public int? Period { get; set; }
        public string? Status { get; set; }
        public DateOnly? PublishedDay { get; set; }

        // For illustration: 
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
        public int BookmarkCount { get; set; }
    }
}
