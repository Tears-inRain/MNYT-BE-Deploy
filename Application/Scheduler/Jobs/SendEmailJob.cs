using Application.IRepos;
using Application.Scheduler.CronJob;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Scheduler.Jobs
{
    public class SendEmailJob : IJob
    {
        private readonly IScheduleUserRepo _scheduleUserRepo;
        private readonly IEmailService _emailService;
        private readonly ILogger<SendEmailJob> _logger;
        public SendEmailJob(IScheduleUserRepo scheduleUserRepo, IEmailService emailService, ILogger<SendEmailJob> logger)
        {
            _scheduleUserRepo = scheduleUserRepo;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow.Date);
            var schedules = await _scheduleUserRepo.GetSchedulesByDateAsync(today);

            foreach (var schedule in schedules)
            {
                if (schedule.Pregnancy?.Account?.Email is string email)
                {
                    string subject = "Lịch hẹn của bạn";
                    string body = $"Bạn có lịch hẹn \"{schedule.Title}\" vào ngày {schedule.Date:dd/MM/yyyy}. Vui lòng kiểm tra chi tiết";

                    await _emailService.SendEmailAsync(email, subject, body);
                    _logger.LogInformation($"Sent email to {email} at {DateTime.Now}");
                }
            }
        }
    }
}
