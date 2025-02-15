using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class BlogLike
{
    public int BlogLikeId { get; set; }

    public int? AccountId { get; set; }

    public int? PostId { get; set; }

    public virtual Account? Account { get; set; }

    public virtual BlogPost? Post { get; set; }
}
