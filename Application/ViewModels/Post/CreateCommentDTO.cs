using Application.ViewModels.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Post
{
    public class CreateCommentDTO
    {
        public int BlogPostId { get; set; }
        public int? ReplyId { get; set; }
        public string? Content { get; set; }
        public List<CreateMediaDTO>? Images { get; set; }
    }
}
