
using Application;
using Application.IServices.CronJob;
using Application.Jobs;
using Application.PaymentProviders.VnPay;
using Application.Services.CronJob;
using Application.Settings;
using Infrastructure;
using Quartz;
using WebAPI.Middlewares;
namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add CORS policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy => policy.AllowAnyOrigin()
                                    .AllowAnyMethod()
                                    .AllowAnyHeader());
            });

            // register Job in DI
            builder.Services.AddTransient<SendEmailJob>();

            // Add services to the container.
            builder.Configuration.AddEnvironmentVariables();

            builder.Services.AddHttpContextAccessor();

            builder.Services.Configure<VnPaySettings>(builder.Configuration.GetSection("Vnpay"));
            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

            builder.Services.AddControllers();

            builder.Services.AddInfrastructureServicesAsync(builder.Configuration);
            var schedulerService = builder.Services.BuildServiceProvider().GetRequiredService<IScheduleJobService>();
            // config Quartz 
            builder.Services.AddQuartz(q => schedulerService.ConfigureQuartz(q));
            builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSingleton<GlobalExceptionMiddleware>();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseCors("AllowAll");

            if (app.Environment.IsDevelopment())
            {
            }

            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseMiddleware<GlobalExceptionMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();

        }
    }
}
