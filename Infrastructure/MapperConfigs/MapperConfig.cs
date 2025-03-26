using Application.ViewModels.Authentication;
using Application.ViewModels.MembershipPlan;
using Application.ViewModels.Payment;
using Application.ViewModels.Fetus;
using Application.ViewModels.Pregnancy;
using Application.ViewModels.FetusRecord;
using AutoMapper;
using Domain.Entities;
using Application.ViewModels.ScheduleUser;
using Application.ViewModels.Accounts;
using Application.ViewModels.PregnancyStandard;
using Application.ViewModels.Blog;
using Application.ViewModels.Media;
using Application.ViewModels.ScheduleTemplate;
using Application.ViewModels.AccountMembership;

namespace Infrastructure.MapperConfigs
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            MappingAccount();
            MappingMembershipPlan();
            MappingPaymentMethod();
            MappingAccountMembership();
            MappingPregnancy();
            MappingFetus();
            MappingFetusRecord();
            MappingScheduleUser();
            MappingPregnancyStandard();
            MappingBlog();
            MappingMedia();
            MappingScheduleTemplate();
        }

        private void MappingPregnancyStandard()
        {
            CreateMap<PregnancyStandardVM, PregnancyStandard>().ReverseMap();
            CreateMap<PregnacyStandardAddVM, PregnancyStandard>().ReverseMap();
        }

        public void MappingAccount()
        {
            CreateMap<AccountRegistrationDTO, Account>()
                    .ForMember(dest => dest.Password, opt => opt.Ignore())
                    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Active"))
                    .ForMember(dest => dest.ExternalProvider, opt => opt.Condition(src => src.IsExternal));
            CreateMap<Account, AccountDTO>();
            //CreateMap<CreateAccountDTO, Account>().ReverseMap();
        }

        public void MappingMembershipPlan()
        {
            CreateMap<CreateMembershipPlanDTO, MembershipPlan>();
            CreateMap<MembershipPlanDTO, MembershipPlan>();
            CreateMap<MembershipPlan, MembershipPlanDTO>();
        }

        public void MappingAccountMembership()
        {
            CreateMap<AccountMembership, ReadAccountMembershipDTO>();
        }

        public void MappingPaymentMethod()
        {
            CreateMap<PaymentMethod, PaymentMethodDTO>().ReverseMap();
            CreateMap<TogglePaymentMethodDTO, PaymentMethod>();
        }
        public void MappingPregnancy()
        {
            CreateMap<PregnancyAddVM, Pregnancy>().ReverseMap();
            CreateMap<PregnancyVM, Pregnancy>().ReverseMap();
            CreateMap<Pregnancy, ReadPregnancyDTO>();

        }
        public void MappingFetus()
        {
            CreateMap<FetusAddVM, Fetus>().ReverseMap();
            CreateMap<FetusVM, Fetus>().ReverseMap();
            CreateMap<Fetus, ReadFetusDTO>();
        }
        public void MappingFetusRecord()
        {
            CreateMap<FetusRecordAddVM, FetusRecord>().ReverseMap();
            CreateMap<FetusRecordVM, FetusRecord>().ReverseMap();
        }
        public void MappingScheduleUser()
        {
            CreateMap<ScheduleUserAddVM, ScheduleUser>().ReverseMap();
            CreateMap<ScheduleUserVM, ScheduleUser>().ReverseMap();
        }

        public void MappingBlog()
        {
            CreateMap<CreatePostDTO, BlogPost>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Draft"));

            CreateMap<BlogPost, ReadPostDTO>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author != null ? src.Author.UserName : null))
                .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(src => src.BlogLikes.Count))
                .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(src => src.Comments.Count))
                .ForMember(dest => dest.BookmarkCount, opt => opt.MapFrom(src => src.BlogBookmarks.Count));
            CreateMap<UpdatePostDTO, BlogPost>();

            CreateMap<CreateCommentDTO, Comment>();
            CreateMap<Comment, ReadCommentDTO>()
                .ForMember(dest => dest.AccountUserName, opt => opt.MapFrom(src => src.Account != null ? src.Account.UserName : null));
        }
        public void MappingMedia()
        {
            CreateMap<CreateMediaDTO, Media>();
            CreateMap<UpdateMediaDTO, Media>();
            CreateMap<Media, ReadMediaDetailDTO>();
            CreateMap<Media, ReadMediaDTO>();
        }
        public void MappingScheduleTemplate()
        {
            CreateMap<ScheduleTemplateAddVM, ScheduleTemplate>().ReverseMap();
            CreateMap<ScheduleTemplateVM, ScheduleTemplate>().ReverseMap();
        }
    }
}
