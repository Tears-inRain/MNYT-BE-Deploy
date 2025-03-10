using Application.IServices.CronJob;
using Application.Jobs;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.CronJob
{
    public class ScheduleJobService : IScheduleJobService
    {

        public void ConfigureQuartz(IServiceCollectionQuartzConfigurator quartz)
        {
            var jobKey = new JobKey("SendEmailJob");

            quartz.AddJob<SendEmailJob>(opts => opts.WithIdentity(jobKey));

            quartz.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("SendEmailJob-trigger")
                .WithCronSchedule("0 * * ? * *")); // Chạy mỗi phút

            var checkMembershipJobKey = new JobKey("CheckAccountMembershipJob");
            quartz.AddJob<Jobs.CheckAccountMembershipJob>(opts => opts.WithIdentity(checkMembershipJobKey));

            quartz.AddTrigger(opts => opts
                .ForJob(checkMembershipJobKey)
                .WithIdentity("CheckAccountMembership_Trigger")
                .StartNow()
                .WithCronSchedule("0 0 0 * * ?"));
        }
    }
}
