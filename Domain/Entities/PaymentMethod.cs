namespace Domain.Entities;

public partial class PaymentMethod :BaseEntity
{
    public string? Method { get; set; }

    public string? Via { get; set; }

    public List<AccountMembership> AccountMembership { get; set; }
}
