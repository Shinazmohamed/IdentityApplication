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
            CreateMap<Employee, InsertEmployeeRequest>()
                .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.Id));

            CreateMap<InsertEmployeeRequest, Employee>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.EmployeeId));

            CreateMap<Employee, ViewEmployeeModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

            CreateMap<ApplicationUser, ListUsersModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location.LocationName));

            CreateMap<CreateSubCategoryRequest, CategoryMapping>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.SelectedCategory))
                .ForMember(dest => dest.SubCategoryId, opt => opt.MapFrom(src => src.SelectedSubCategory));

            CreateMap<CategoryMapping, ListCategoryMappingModel>()
                .ForMember(dest => dest.SelectedCategory, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.SelectedSubCategory, opt => opt.MapFrom(src => src.SubCategoryId));
        }

    }
}
