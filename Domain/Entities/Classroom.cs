using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Classroom : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }

        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
    }

    public enum ApproveEnum
    {
        Pending,
        Approved,
        Rejected
    }
}
