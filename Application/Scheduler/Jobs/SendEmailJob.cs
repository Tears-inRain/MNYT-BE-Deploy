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
            var today = DateOnly.FromDateTime(DateTime.UtcNow.Date);//thời gian gửi bị trễ 1 ngày, need fix
            var schedules = await _scheduleUserRepo.GetSchedulesByDateAsync(today);

            foreach (var schedule in schedules)
            {
                if (schedule.Pregnancy?.Account?.Email is string email)
                {
                    string subject = "Lịch hẹn của bạn";
                    string body = $@"
    <html>
    <head>
        <style>
            body {{
                font-family: Arial, sans-serif;
                background-color: #f4f4f4;
                padding: 20px;
                text-align: center;
            }}
            .container {{
                max-width: 666px;
                margin: auto;
                background: #fffdf5;
                padding: 20px;
                border-radius: 10px;
                box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1);
            }}
            .title {{
                font-size: 27px;
                font-weight: bold;
                color: #333;
                margin-bottom: 10px;
                margin-top: 1px;
            }}
            .content {{
                background: #55a6c3;
                padding: 13px;
                font-size: 19px;
                border-radius: 5px;
                text-align: left;
                color: #08220b;
            }}
            .footer {{
                margin-top: 10px;
                font-size: 15px;
                color: #666;
                min-height: 100px;
            }}
        </style>
    </head>
    <body>
        <div class='container'>
            <img src='https://i.imgur.com/vVK4Pa5.png' width='150' alt='Logo'>

            <div class='title'>
                <p>Mầm Non Yêu Thương xin kính chào</p>
                <p><strong>{schedule.Pregnancy?.Account?.FullName}</strong></p>
            </div>

            <div class='content'>
                <p>Hiện tại bạn có lịch hẹn <strong>{schedule.Title}</strong>.</p>
                <p style =""color: #08220b;"">Nội dung ghi chú: {schedule.Note}.</p>
                <p>Vào ngày <strong>{schedule.Date:dd/MM/yyyy}</strong>.</p>
            </div>

            <div class='footer'>
                <p>Vui lòng kiểm tra chi tiết lịch hẹn trên trang web.</p>
                <p><strong>Mầm Non Yêu Thương</strong> luôn đồng hành cùng mẹ bầu!</p>
                <p>Xin chúc mẹ và bé có thật nhiều sức khỏe.</p>
            </div>
        </div>
    </body>
    </html>";

                    await _emailService.SendEmailAsync(email, subject, body);
                    _logger.LogInformation($"Sent email to {email} at {DateTime.Now}");
                }
            }
        }
    }
}
