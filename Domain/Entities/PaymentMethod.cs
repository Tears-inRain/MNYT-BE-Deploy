namespace Domain.Entities;

public partial class PaymentMethod : BaseEntity
{
    public string? Via { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public List<AccountMembership> AccountMembership { get; set; }
}
