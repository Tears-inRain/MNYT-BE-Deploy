using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Post
{
    public class ReadPostLikeDTO
    {
        public int Id { get; set; }
        public int? AccountId { get; set; }
        public string? Account { get; set; }

        public int? PostId { get; set; }
    }
}
