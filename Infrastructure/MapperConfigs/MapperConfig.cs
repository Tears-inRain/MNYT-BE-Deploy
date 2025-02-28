using Application.ViewModels.Authentication;
<<<<<<< HEAD
using Application.ViewModels.MembershipPlan;
using Application.ViewModels.Payment;
=======
using Application.ViewModels.Pregnancy;
>>>>>>> origin/main
using AutoMapper;
using Domain.Entities;

namespace Infrastructure.MapperConfigs
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            MappingAccount();
<<<<<<< HEAD
            MappingMembershipPlan();
            MappingPaymentMethod();
=======
            MappingPregnancy();
>>>>>>> origin/main
        }

        public void MappingAccount()
        {
            CreateMap<AccountRegistrationDTO, Account>()
                    .ForMember(dest => dest.Password, opt => opt.Ignore())
                    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Active"))
                    .ForMember(dest => dest.ExternalProvider, opt => opt.Condition(src => src.IsExternal));
        }
<<<<<<< HEAD

        public void MappingMembershipPlan()
        {
            CreateMap<CreateMembershipPlanDTO, MembershipPlan>();
            CreateMap<MembershipPlanDTO, MembershipPlan>();
            CreateMap<MembershipPlan, MembershipPlanDTO>();
        }

        public void MappingPaymentMethod()
        {
            CreateMap<PaymentMethod, PaymentMethodDTO>().ReverseMap();
            CreateMap<TogglePaymentMethodDTO, PaymentMethod>();
=======
        public void MappingPregnancy()
        {
            CreateMap<PregnancyAddVM, Pregnancy>().ReverseMap();
            CreateMap<PregnancyVM, Pregnancy>().ReverseMap();
>>>>>>> origin/main
        }
    }
}
