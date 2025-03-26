using Domain.Entities;
using Domain;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services.IServices;

namespace Application.Services
{
    public class CashPaymentService : ICashPaymentService
    {
        private readonly IAccountMembershipService _membershipService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CashPaymentService> _logger;

        public CashPaymentService(
            IAccountMembershipService membershipService,
            IUnitOfWork unitOfWork,
            ILogger<CashPaymentService> logger)
        {
            _membershipService = membershipService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<AccountMembership> CreateCashPaymentAsync(int accountId, int membershipPlanId)
        {
            _logger.LogInformation("CashPayment: Checking membership for account #{AccountId}, plan #{PlanId}",
                                   accountId, membershipPlanId);

            var currentActive = await _membershipService.GetActiveMembershipAsync(accountId);

            AccountMembership newMembership;

            if (currentActive == null)
            {
                _logger.LogInformation("No active membership => create new membership for Account #{AccountId}", accountId);
                newMembership = await _membershipService.CreateNewMembershipAsync(accountId, membershipPlanId);
            }
            else
            {
                _logger.LogInformation("Active membership #{MembershipId} found => upgrade to plan #{PlanId}.",
                    currentActive.Id, membershipPlanId);
                newMembership = await _membershipService.UpgradeMembershipAsync(accountId, membershipPlanId);
            }

            var plan = await _unitOfWork.MembershipPlanRepo.GetByIdAsync(newMembership.MembershipPlanId ?? 0);
            if (plan == null)
                throw new Exception("Membership plan not found.");

            newMembership.PaymentStatus = "Paid";
            newMembership.Status = "Active";
            newMembership.StartDate = DateOnly.FromDateTime(DateTime.Now);
            newMembership.EndDate = newMembership.StartDate.Value.AddDays(plan.Duration);

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("CashPayment done. Membership #{NewId} => Active, PaymentStatus=Paid.", newMembership.Id);

            return newMembership;
        }
    }
}