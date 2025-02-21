using Application;
using Application.IRepos;

namespace Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        public readonly AppDbContext _context;
        public readonly IAccountRepo _accountRepo;
        public readonly IAccountMembershipRepo _accountMembershipRepo;
        public readonly IMembershipPlanRepo _membershipPlanRepo;
        public readonly IPaymentMethodRepo _paymentMethodRepo;
        public readonly IBlogBookmarkRepo _blogBookmarkRepo;
        public readonly IBlogLikeRepo _blogLikeRepo;
        public readonly IBlogPostRepo _postRepo;
        public readonly ICommentRepo _commentRepo;
        public readonly IFetusRepo _fetusRepo;
        public readonly IFetusRecordRepo _fetusRecordRepo;
        public readonly IMediaRepo _mediaRepo;
        public readonly IPregnancyRepo _regnancyRepo;
        public readonly IPregnancyStandardRepo _standardRepo;
        public readonly IScheduleTemplateRepo _scheduleTemplateRepo;
        public readonly IScheduleUserRepo _scheduleUserRepo;


        public IAccountRepo AccountRepo => _accountRepo;

        public IAccountMembershipRepo AccountMembershipRepo => _accountMembershipRepo;

        public IMembershipPlanRepo MembershipPlanRepo => _membershipPlanRepo;

        public IPaymentMethodRepo PaymentMethodRepo => _paymentMethodRepo;

        public IBlogBookmarkRepo BlogBookmarkRepo => _blogBookmarkRepo;

        public IBlogLikeRepo BlogLikeRepo => _blogLikeRepo;

        public IBlogPostRepo PostRepo => _postRepo;

        public ICommentRepo CommentRepo => _commentRepo;

        public IFetusRepo FetusRepo => _fetusRepo;

        public IFetusRecordRepo FetusRecordRepo => _fetusRecordRepo;

        public IMediaRepo MediaRepo => _mediaRepo;

        public IPregnancyRepo RegnancyRepo => _regnancyRepo;

        public IPregnancyStandardRepo StandardRepo => _standardRepo;

        public IScheduleTemplateRepo ScheduleTemplateRepo => _scheduleTemplateRepo;

        public IScheduleUserRepo ScheduleUserRepo => _scheduleUserRepo;

        public UnitOfWork(AppDbContext context, IAccountRepo accountRepo, IAccountMembershipRepo accountMembershipRepo, IMembershipPlanRepo membershipPlanRepo, IPaymentMethodRepo paymentMethodRepo, IBlogBookmarkRepo blogBookmarkRepo, IBlogLikeRepo blogLikeRepo, IBlogPostRepo postRepo, ICommentRepo commentRepo, IFetusRepo fetusRepo, IFetusRecordRepo fetusRecordRepo, IMediaRepo mediaRepo, IPregnancyRepo regnancyRepo, IPregnancyStandardRepo standardRepo, IScheduleTemplateRepo scheduleTemplateRepo, IScheduleUserRepo scheduleUserRepo)
        {
            _context = context;
            _accountRepo = accountRepo;
            _accountMembershipRepo = accountMembershipRepo;
            _membershipPlanRepo = membershipPlanRepo;
            _paymentMethodRepo = paymentMethodRepo;
            _blogBookmarkRepo = blogBookmarkRepo;
            _blogLikeRepo = blogLikeRepo;
            _postRepo = postRepo;
            _commentRepo = commentRepo;
            _fetusRepo = fetusRepo;
            _fetusRecordRepo = fetusRecordRepo;
            _mediaRepo = mediaRepo;
            _regnancyRepo = regnancyRepo;
            _standardRepo = standardRepo;
            _scheduleTemplateRepo = scheduleTemplateRepo;
            _scheduleUserRepo = scheduleUserRepo;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
