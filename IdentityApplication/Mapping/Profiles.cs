using AutoMapper;
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
        }
    }
}
