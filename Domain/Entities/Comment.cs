namespace Domain.Entities;

public partial class Comment : BaseEntity
{
    public int? AccountId { get; set; }

    public int? BlogPostId { get; set; }

    public int? ReplyId { get; set; }

    public string? Status { get; set; }

    public string? Content { get; set; }

    public virtual Account? Account { get; set; }

    public virtual BlogPost? BlogPost { get; set; }
}
