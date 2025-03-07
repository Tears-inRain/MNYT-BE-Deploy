using Domain;

namespace Application.ViewModels.Pregnancy
{
    public class PregnancyVM : BaseEntity
    {
        public int? AccountId { get; set; }

        public string? Type { get; set; }

        public string? Status { get; set; }

        public DateOnly? EndDate { get; set; }

    }
}
