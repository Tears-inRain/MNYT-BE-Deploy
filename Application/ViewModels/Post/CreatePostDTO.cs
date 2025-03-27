using Application.ViewModels.Media;

namespace Application.ViewModels.Post
{
    public class CreatePostDTO
    {
        public string? Category { get; set; }

        public bool IsAnonymous { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public List<CreateMediaDTO>? Images { get; set; }

        public int? Period { get; set; }

        public string? Status { get; set; }

        public DateOnly? PublishedDay { get; set; }
    }
}
