using AutoMapper;
using IdentityApplication.Areas.Identity.Data;
using IdentityApplication.Core.Entities;
using IdentityApplication.Core.ViewModel;

namespace IdentityApplication.Mapping
{
    public class Profiles : Profile
    {
        public Profiles()
        {
            CreateMap<Employee, InsertEmployeeRequest>();
            CreateMap<InsertEmployeeRequest, Employee>();

            CreateMap<Employee, ViewEmployeeModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
                // Map other properties if needed
                // .ForMember(dest => dest.OtherProperty, opt => opt.MapFrom(src => src.SourceProperty));

            CreateMap<ApplicationUser, ListUsersModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location.LocationName));
        }

    }
}
