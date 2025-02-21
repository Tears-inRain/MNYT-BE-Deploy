using Application.IRepos;
using Domain.Entities;

namespace Infrastructure.Repos
{
    public class PaymentMethodRepo : GenericRepo<PaymentMethod>, IPaymentMethodRepo
    {
        private readonly AppDbContext _appDbContext;
        public PaymentMethodRepo(AppDbContext dbContext) : base(dbContext)
        {
            _appDbContext = dbContext;
        }
    }
}
