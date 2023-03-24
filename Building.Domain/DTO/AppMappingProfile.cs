using AutoMapper;
using Building.Domain.Entity;
using Building.Domain.Response;
using Building.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Building.Domain.DTO
{
    public class AppMappingProfile:Profile
    {
        //public AppMappingProfile()
        //{
        //    CreateMap<Employee,EmployeeDTO>().ReverseMap();
        //    CreateMap<Employee,ProfileVIewModel>().ReverseMap();
        //    CreateMap<Query, QueryDTO>()
        //        .ForMember(c => c.Name, opt => opt.MapFrom(src => src.Material.Name))
        //        .ForMember(c => c.Responsible, opt => opt.MapFrom(src => $"{src.Snab.SecondName} {src.Snab.Name}"));
        //}
    }
}
