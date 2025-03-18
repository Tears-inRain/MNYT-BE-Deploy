using Application.Scheduler.Jobs;
using Application.Scheduler.JobSetup.Interfaces;
using Quartz;

namespace Application.Scheduler.JobSetup
{
    public class CheckAccountMembershipJobSetup : ICheckAccountMembershipJobSetup
    {
        public void ConfigureQuartz(IServiceCollectionQuartzConfigurator q)
        {
            var checkMembershipJobKey = new JobKey("CheckAccountMembershipJob");

            q.AddJob<CheckAccountMembershipJob>(opts => opts.WithIdentity(checkMembershipJobKey));

            q.AddTrigger(opts => opts
                .ForJob(checkMembershipJobKey)
                .WithIdentity("CheckAccountMembership_Trigger")
                .StartNow()
                .WithCronSchedule("0 0 0 * * ?"));
        }
    }
}
