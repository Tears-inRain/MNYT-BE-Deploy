using Domain.Enums;
namespace Application.ViewModels.Post
{
    public class UpdatePostDTO
    {
        public string? Category { get; set; }
        public string? Title { get; set; }
        public PostType? TypeEnum { get; set; }
        public string? Description { get; set; }
        public int? Period { get; set; }
    }
}
