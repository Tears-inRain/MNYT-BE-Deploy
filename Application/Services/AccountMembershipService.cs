using Application.Services.IServices;
using Application.ViewModels.AccountMembership;
using Application.ViewModels.Blog;
using Application.ViewModels.Media;
using AutoMapper;
using Domain;
using Domain.Entities;
using Domain.Enums;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class AccountMembershipService : IAccountMembershipService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountMembershipService> _logger;

        public AccountMembershipService(IUnitOfWork unitOfWork,
                                        IMapper mapper,
                                        ILogger<AccountMembershipService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ReadAccountMembershipDTO?> GetActiveMembershipAsync(int accountId)
        {
            var allMemberships = await _unitOfWork.AccountMembershipRepo.GetAllAsync();

            var latestActiveMembership = allMemberships
                .Where(m => m.AccountId == accountId && m.Status == "Active")
                .OrderByDescending(m => m.StartDate) // Use correct date field
                .FirstOrDefault();
            return _mapper.Map<ReadAccountMembershipDTO>(latestActiveMembership);
            ;
        }

        public async Task<IEnumerable<ReadAccountMembershipDTO>> GetAllAccountMembershipAsync()
        {
            var entities = await _unitOfWork.AccountMembershipRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<ReadAccountMembershipDTO>>(entities);
        }

        public async Task<AccountMembership> CreateNewMembershipAsync(int accountId, int membershipPlanId)
        {
            var currentActive = await GetActiveMembershipAsync(accountId);
            if (currentActive != null)
            {
                throw new Exception("You already have an active membership. Cannot register a new one.");
            }

            var plan = await _unitOfWork.MembershipPlanRepo.GetByIdAsync(membershipPlanId);
            if (plan == null)
                throw new Exception("MembershipPlan not found.");

            var membership = new AccountMembership
            {
                AccountId = accountId,
                MembershipPlanId = plan.Id,
                Amount = plan.Price,
                Status = "Pending",
                PaymentStatus = "Pending",
                PaymentMethodId = (int)PaymentMethodEnum.Cash,
                StartDate = null,
                EndDate = null,
            };
            await _unitOfWork.AccountMembershipRepo.AddAsync(membership);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Created new membership #{MembershipId} for account #{AccountId}", membership.Id, accountId);
            return membership;
        }

        public async Task<AccountMembership> UpgradeMembershipAsync(int accountId, int newPlanId)
        {
            var current = await GetActiveMembershipAsync(accountId);
            if (current == null)
            {
                throw new Exception("You don't have an active membership to upgrade.");
            }

            var newPlan = await _unitOfWork.MembershipPlanRepo.GetByIdAsync(newPlanId);
            if (newPlan == null)
                throw new Exception("New MembershipPlan not found.");

            if (newPlan.Price <= (current.Amount ?? 0))
            {
                throw new Exception("Cannot downgrade or upgrade to a cheaper or equal plan.");
            }
            var extraCost = newPlan.Price - (current.Amount ?? 0);

            var membership = new AccountMembership
            {
                AccountId = accountId,
                MembershipPlanId = newPlan.Id,
                Amount = extraCost,
                Status = "Pending",
                PaymentStatus = "Pending",
                PaymentMethodId = (int)PaymentMethodEnum.Cash,
                StartDate = null,
                EndDate = null,
            };
            await _unitOfWork.AccountMembershipRepo.AddAsync(membership);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Created membership for upgrade #{NewMemId} from old membership #{OldMemId} for account #{AccountId}",
                membership.Id, current.Id, accountId);

            return membership;
        }
    }
}
