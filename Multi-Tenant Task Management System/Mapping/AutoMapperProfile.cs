using Multi_Tenant_Task_Management_System.DTOs;
using Multi_Tenant_Task_Management_System.Models;
using AutoMapper;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Multi_Tenant_Task_Management_System.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // DTO <-> Entity Mappings
            CreateMap<Project, ProjectDto>().ReverseMap();
            CreateMap<TaskEntity, TaskDto>()
             .ForMember(dest => dest.AssignedTo, opt => opt.MapFrom(src => src.AssignedTo.FullName))
             .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Project.Name));
        }
    }
}
