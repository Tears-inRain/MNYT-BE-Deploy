
namespace Domain.Entities;

public partial class Media : BaseEntity
{
    public string? Type { get; set; }

    public string? EntityType { get; set; }

    public string? Url { get; set; }
}
