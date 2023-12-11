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

            CreateMap<CreateCategorySubCategoryRequest, SubCategory>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.SelectedCategory))
                .ForMember(dest => dest.SubCategoryId, opt => opt.MapFrom(src => src.SelectedSubCategory));

            CreateMap<CreateSubCategoryRequest, SubCategory>()
                .ForMember(dest => dest.SubCategoryId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.SubCategoryName, opt => opt.MapFrom(src => src.Name));

            CreateMap<SubCategory, CreateSubCategoryRequest>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.SubCategoryId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.SubCategoryName));

            CreateMap<Category, ListCategoryModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CategoryName));

            CreateMap<CreateCategoryRequest, Category>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Name));

            CreateMap<Category, CreateCategoryRequest>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CategoryName));


            CreateMap<Menu, MenuViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.MenuId))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.DisplayName))
                .ForMember(dest => dest.SubMenu, opt => opt.MapFrom(src => src.SubMenus));

            CreateMap<SubMenu, SubMenuViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.MenuId))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.DisplayName))
                .ForMember(dest => dest.Controller, opt => opt.MapFrom(src => src.Controller))
                .ForMember(dest => dest.Method, opt => opt.MapFrom(src => src.Method));

            CreateMap<CreateMenuRequest, Menu>();
            CreateMap<CreateMenuRequest, SubMenu>()
                .ForMember(dest => dest.MenuId, opt => opt.MapFrom(src => src.SelectedMenu));

            CreateMap<Audit, ListAuditModel>();

            CreateMap<CreateDepartmentViewModel, Department>()
                .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Name));

            CreateMap<Department, ListDepartmentViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.DepartmentId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.DepartmentName));

            CreateMap<CreateCategoryDepartmentMappingViewModel, Category>()
                .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.SelectedDepartment))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.SelectedCategory));

            CreateMap<CreateCategoryDepartmentMappingViewModel, Category>()
                .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.SelectedDepartment))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.SelectedCategory));


            CreateMap<SubCategory, ListSubCategoryModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.SubCategoryId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.SubCategoryName));

            CreateMap<CreatePermission, Permission>();

            CreateMap<Entity, ViewEntityModel>();

            CreateMap<Permission, ViewPermissionModel>();
            
        }

    }
}
