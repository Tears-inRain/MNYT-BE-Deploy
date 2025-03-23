using Application;
using Application.IRepos;
using Application.Services;
using Application.Utils.Interfaces;
using Infrastructure.Repos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Application.Settings;
using Application.Authentication;
using Application.Authentication.Interface;
using Application.Scheduler.JobSetup;
using Application.Scheduler.JobSetup.Interfaces;
using Application.Scheduler.CronJob;
using Application.Services.IServices;
using Application.Utils;
using Application.PaymentProviders.VnPay;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static async Task<IServiceCollection> AddInfrastructureServicesAsync(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(config.GetConnectionString("MNYT_DB")));
            services.AddTransient<IEmailService, EmailService>();
            services.Configure<EmailSettings>(config.GetSection("EmailSettings"));
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

            services.AddScoped<IAccountService, AccountService>();

            services.AddScoped<IPregnancyService, PregnancyService>();
            services.AddScoped<IFetusService, FetusService>();
            services.AddScoped<IFetusRecordService, FetusRecordService>();

            services.AddScoped<IScheduleUserService, ScheduleUserService>();

            services.AddScoped<IPregnancyStandardService, PregnancyStandardService>();
            services.AddScoped<IScheduleTemplateService, ScheduleTemplateService>();

            services.AddScoped<IMembershipPlanService, MembershipPlanService>();
            services.AddScoped<IPaymentMethodService, PaymentMethodService>();
            services.AddScoped<IAccountMembershipService, AccountMembershipService>();
            services.AddScoped<IVnPayService, VnPayService>();
            services.AddScoped<ICashPaymentService, CashPaymentService>();

            services.AddScoped<IPostService, PostService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IInteractionService, InteractionService>();
            services.AddScoped<IMediaService, MediaService>();

            services.AddScoped<IScheduleJobSetup, ScheduleJobSetup>();
            services.AddScoped<ICheckAccountMembershipJobSetup, CheckAccountMembershipJobSetup>();
            #endregion

            #region quartz config
            var quartzBuilder = Host.CreateDefaultBuilder()
                .ConfigureServices((cxt, services) =>
                {
                    services.AddQuartz();
                    services.AddQuartzHostedService(opt =>
                    {
                        opt.WaitForJobsToComplete = true;
                    });
                }).Build();

            // will block until the last running job completes
            await quartzBuilder.RunAsync();
            return services;
            #endregion

        }
    }
}
