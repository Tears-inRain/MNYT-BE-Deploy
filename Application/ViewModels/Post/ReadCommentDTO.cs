using Application.ViewModels.Media;

namespace Application.ViewModels.Post
{
    public class ReadCommentDTO
    {
        public int Id { get; set; }
        public int? AccountId { get; set; }
        public string? AccountUserName { get; set; }
        public int? BlogPostId { get; set; }
        public int? ReplyId { get; set; }
        public string? Content { get; set; }
        //public string? Status { get; set; }
        public DateTime CreateDate { get; set; }
        public List<ReadMediaDTO>? Images { get; set; }
    }
}
