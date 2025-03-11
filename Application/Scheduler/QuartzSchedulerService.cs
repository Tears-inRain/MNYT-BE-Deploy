using Application.Scheduler.JobSetup.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Scheduler
{
    public static class QuartzSchedulerService
    {
        public static IServiceCollection AddQuartzJobs(this IServiceCollection services)
        {
            services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionJobFactory();

                using (var serviceProvider = services.BuildServiceProvider())
                {
                    var schedulerService = serviceProvider.GetRequiredService<IScheduleJobSetup>();
                    var setup = serviceProvider.GetRequiredService<ICheckAccountMembershipJobSetup>();

                    setup.ConfigureQuartz(q);
                    schedulerService.ConfigureQuartz(q);
                }
            });

            services.AddQuartzHostedService(options =>
            {
                options.WaitForJobsToComplete = true;
            });

            return services;
        }
    }
}