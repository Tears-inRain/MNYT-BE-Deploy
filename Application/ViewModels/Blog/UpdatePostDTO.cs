using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Blog
{
    public class UpdatePostDTO
    {
        public string? Category { get; set; }
        public string? Title { get; set; }
        public PostType? TypeEnum { get; set; }
        public string? Description { get; set; }
        public int? Period { get; set; }
        public int? ImageId { get; set; }
        public string? ImageUrl { get; set; }
    }
}
