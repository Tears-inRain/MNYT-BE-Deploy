
namespace Domain.Entities
{
    public partial class Fetus : BaseEntity
    {
        public string? Name { get; set; }

        public string? Gender { get; set; }

        public int? PregnancyId { get; set; }

        public virtual Pregnancy? Pregnancy { get; set; }

        public virtual ICollection<FetusRecord> FetusRecords { get; set; } = new List<FetusRecord>();
    }
}
