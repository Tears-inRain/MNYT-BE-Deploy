using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class Comment
{
    public int CommentId { get; set; }

    public int? AccountId { get; set; }

    public int? BlogPostId { get; set; }

    public int? ReplyId { get; set; }

    public string? Status { get; set; }

    public string? Content { get; set; }

    public virtual Account? Account { get; set; }

    public virtual BlogPost? BlogPost { get; set; }
}
