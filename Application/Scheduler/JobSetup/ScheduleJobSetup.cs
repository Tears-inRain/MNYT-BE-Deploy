using Application.Scheduler.Jobs;
using Application.Scheduler.JobSetup.Interfaces;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Scheduler.JobSetup
{
    public class ScheduleJobSetup : IScheduleJobSetup
    {
        public void ConfigureQuartz(IServiceCollectionQuartzConfigurator quartz)
        {
            var jobKey = new JobKey("SendEmailJob");

            quartz.AddJob<SendEmailJob>(opts => opts.WithIdentity(jobKey));

            quartz.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("SendEmailJob-trigger")
                .WithCronSchedule("0 0 * * *")); // Chạy mỗi ngày lúc 12:00
        }
    }
}
