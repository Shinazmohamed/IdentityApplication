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
                .ForMember(e =>
                    e.Id, e => e.MapFrom(src => src.Id));

            CreateMap<ApplicationUser, ListUsersModel>()
                .ForMember(e =>
                    e.Id, e => e.MapFrom(src => src.Id))
                .ForMember(e =>
                    e.Email, e => e.MapFrom(src => src.Email))
                .ForMember(e =>
                    e.Location, e => e.MapFrom(src => src.LocationId));
        }
    }
}
