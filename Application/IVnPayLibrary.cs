
namespace Application
{
    public interface IVnPayLibrary
    {
        string CreatePaymentUrl(decimal amount, string orderDesc, string orderId);
        bool VerifySignature(IDictionary<string, string> queryParams);
    }
}
