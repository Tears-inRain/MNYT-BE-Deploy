using Application.ViewModels.Media;
using Domain.Enums;

namespace Application.ViewModels.Blog
{
    public class ReadPostDTO
    {
        public int Id { get; set; }
        public string? Category { get; set; }
        public bool IsAnonymous { get; set; }
        public string Title { get; set; } = null!;
        public PostType? TypeEnum { get; set; }
        public string? Description { get; set; }
        public int? AuthorId { get; set; }
        public string? AuthorName { get; set; }
        public int? Period { get; set; }
        public string? Status { get; set; }
        public DateOnly? PublishedDay { get; set; }
        public List<ReadMediaDTO>? Images { get; set; }
        public DateTime CreateDate { get; set; }

        // For illustration: 
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
        public int BookmarkCount { get; set; }
    }
}
