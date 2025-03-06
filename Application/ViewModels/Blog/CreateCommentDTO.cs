using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Blog
{
    public class CreateCommentDTO
    {
        public int BlogPostId { get; set; }
        public int? ReplyId { get; set; }
        public string Content { get; set; } = null!;
    }
}
