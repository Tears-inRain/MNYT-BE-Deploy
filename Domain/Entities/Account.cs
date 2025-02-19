namespace Domain.Entities;

public partial class Account : BaseEntity
{
    public string UserName { get; set; } = null!;

    public string? FullName { get; set; }

    public string Email { get; set; } = null!;

    public string? Password { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Role { get; set; }

    public string? Status { get; set; }

    public bool? IsExternal { get; set; }

    public string? ExternalProvider { get; set; }

    public virtual ICollection<AccountMembership> AccountMemberships { get; set; } = new List<AccountMembership>();

    public virtual ICollection<BlogBookmark> BlogBookmarks { get; set; } = new List<BlogBookmark>();

    public virtual ICollection<BlogLike> BlogLikes { get; set; } = new List<BlogLike>();

    public virtual ICollection<BlogPost> BlogPosts { get; set; } = new List<BlogPost>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Pregnancy> Pregnancies { get; set; } = new List<Pregnancy>();
}
