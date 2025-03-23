using Domain.Enums;

namespace Domain.Entities;

public partial class BlogPost : BaseEntity
{
    public string? Category { get; set; }

    public string Title { get; set; } = null!;

    public PostType TypeEnum { get; set; } = PostType.Forum;

    public string? Description { get; set; }

    public int? ImageId { get; set; }

    public string? ImageUrl { get; set; }

    public int? AuthorId { get; set; }

    public int? Period { get; set; }

    public string? Status { get; set; }

    public DateOnly? PublishedDay { get; set; }

    public virtual Account? Author { get; set; }

    public virtual ICollection<BlogBookmark> BlogBookmarks { get; set; } = new List<BlogBookmark>();

    public virtual ICollection<BlogLike> BlogLikes { get; set; } = new List<BlogLike>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
