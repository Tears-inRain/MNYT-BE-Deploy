using Application.ViewModels.Subject;
using AutoMapper;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.MapperConfigs
{
    public class MapperConfig : Profile
    {
        public MapperConfig() 
        {
            MappingSubject();
        }

        public void MappingSubject()
        {
            CreateMap<SubjectAddVM, Account>().ReverseMap();
            CreateMap<SubjectVM, Account>().ReverseMap();
        }
    }
}
