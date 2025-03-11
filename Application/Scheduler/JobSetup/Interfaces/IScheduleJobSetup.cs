using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Scheduler.JobSetup.Interfaces
{
    public interface IScheduleJobSetup
    {
        void ConfigureQuartz(IServiceCollectionQuartzConfigurator quartz);
    }
}
