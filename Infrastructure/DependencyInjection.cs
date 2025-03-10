using Application;
using Application.IRepos;
using Application.IServices;
using Application.IServices.Authentication;
using Application.PaymentProviders.VnPay;
using Application.Services;
using Application.Services.Authentication;
using Application.Utils.Implementation;
using Application.Utils.Interfaces;
using Microsoft.Data.SqlClient;
using Infrastructure.Repos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Application.IServices.CronJob;
using Application.Services.CronJob;
using Application.Settings;

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
            services.AddScoped<IPregnancyService, PregnancyService>();
            services.AddScoped<IVnPayLibrary, VnPayLibrary>();
            services.AddScoped<IMembershipPlanService, MembershipPlanService>();
            services.AddScoped<IPaymentMethodService, PaymentMethodService>();
            services.AddScoped<IAccountMembershipService, AccountMembershipService>();
            services.AddScoped<IVnPayService, VnPayService>();
            services.AddScoped<IFetusService, FetusService>();
            services.AddScoped<IFetusRecordService, FetusRecordService>();
            services.AddScoped<IScheduleUserService, ScheduleUserService>();
            services.AddScoped<IScheduleJobService, ScheduleJobService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IPregnancyStandardService, PregnancyStandardService>();
            services.AddScoped<IBlogPostService, BlogPostService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IInteractionService, InteractionService>();
            services.AddScoped<IMediaService, MediaService>();
            services.AddScoped<IScheduleTemplateService, ScheduleTemplateService>();
            #endregion

            services.Configure<VnPaySettings>(options => config.GetSection("Vnpay").Bind(options));

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
