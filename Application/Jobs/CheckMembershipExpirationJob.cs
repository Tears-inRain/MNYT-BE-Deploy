using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Application.Jobs
{
    [DisallowConcurrentExecution]
    public class CheckAccountMembershipJob : IJob
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CheckAccountMembershipJob> _logger;

        public CheckAccountMembershipJob(IUnitOfWork unitOfWork,
                                            ILogger<CheckAccountMembershipJob> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Running scheduled job: CheckAccountMembershipJob at {Time}", DateTime.Now);

            bool anyChange = false;

            var activeMemberships = await _unitOfWork.AccountMembershipRepo
                .GetAllQueryable()
                .Where(m => m.Status == "Active")
                .ToListAsync();

            // 2) Kiểm tra gói nào đã hết hạn
            var today = DateOnly.FromDateTime(DateTime.Now);
            foreach (var membership in activeMemberships)
            {
                // Nếu EndDate < hôm nay => đã hết hạn
                if (membership.EndDate != null && membership.EndDate < today)
                {
                    membership.Status = "Expired";
                    _logger.LogInformation("Membership #{MembershipId} for Account #{AccountId} expired.",
                                           membership.Id, membership.AccountId);
                    anyChange = true;
                }
            }

            // 3) Lưu thay đổi
            if (anyChange)
            {
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                _logger.LogInformation("No membership status changed. Skip SaveChanges.");
            }

            _logger.LogInformation("CheckAccountMembershipJob finished at {time}", DateTimeOffset.Now);
        }
    }
}
