using Application;
using Application.IRepos;
using Application.IServices;
using Application.IServices.Authentication;
using Application.PaymentProviders.VnPay;
using Application.Services;
using Application.Services.Authentication;
using Application.Utils.Implementation;
using Application.Utils.Interfaces;
using Infrastructure.Repos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            #region repo config
            services.AddScoped<IAccountRepo, AccountRepo>();
            services.AddScoped<IAccountMembershipRepo, AccountMembershipRepo>();
            services.AddScoped<IMembershipPlanRepo, MembershipPlanRepo>();
            services.AddScoped<IPaymentMethodRepo,PaymentMethodRepo>();
            services.AddScoped<IBlogBookmarkRepo, BlogBookmarkRepo>();
            services.AddScoped<IBlogLikeRepo, BlogLikeRepo>();
            services.AddScoped<IBlogPostRepo, BlogPostRepo>();
            services.AddScoped<ICommentRepo, CommentRepo>();
            services.AddScoped<IFetusRepo, FetusRepo>();
            services.AddScoped<IFetusRecordRepo, FetusRecordRepo>();
            services.AddScoped<IMediaRepo, MediaRepo>();
            services.AddScoped<IPregnancyRepo, PregnancyRepo>();
            services.AddScoped<IPregnancyStandardRepo, PregnancyStandardRepo>();
            services.AddScoped<IScheduleTemplateRepo, ScheduleTemplateRepo>();
            services.AddScoped<IScheduleUserRepo, ScheduleUserRepo>();
            #endregion

            #region service config
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IDateTimeProvider, DateTimeProvider>();
            services.AddScoped<IVnPayService, VnPayService>();
            services.AddScoped<IMembershipPlanService, MembershipPlanService>();
            services.AddScoped<IPaymentMethodService, PaymentMethodService>();
            services.AddScoped<IAccountMembershipService, AccountMembershipService>();
            #endregion
            services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(config.GetConnectionString("MNYT_DB")));

            return services;

        }
    }
}
