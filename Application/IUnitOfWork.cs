using Application.IRepos;
using Microsoft.EntityFrameworkCore.Storage;

namespace Application
{
    public interface IUnitOfWork
    {
        IAccountRepo AccountRepo { get; }

        IAccountMembershipRepo AccountMembershipRepo { get; }

        IMembershipPlanRepo MembershipPlanRepo { get; }

        IPaymentMethodRepo PaymentMethodRepo { get; }

        IBlogBookmarkRepo BlogBookmarkRepo { get; }

        IBlogLikeRepo BlogLikeRepo { get; }

        IBlogPostRepo PostRepo { get; }

        ICommentRepo CommentRepo { get; }

        IFetusRepo FetusRepo { get; }

        IFetusRecordRepo FetusRecordRepo { get; }

        IMediaRepo MediaRepo { get; }

        IPregnancyRepo RegnancyRepo { get; }

        IPregnancyStandardRepo StandardRepo { get; }

        IScheduleTemplateRepo ScheduleTemplateRepo { get; }

        IScheduleUserRepo ScheduleUserRepo { get; }

        public Task<int> SaveChangesAsync();
        IDbContextTransaction BeginTransaction();
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
