using IdentityApplication.Business.Contracts;
using static IdentityApplication.Core.Constants;

namespace IdentityApplication.Core.ViewModel
{
    public class MenuModel
    {

        private readonly IMenuBusiness _business;

        public List<ViewMenuModel> MenuItems { get; set; }

        public MenuModel(IMenuBusiness business)
        {
            _business = business ?? throw new ArgumentNullException(nameof(business));
            MenuItems = GetMenus();
        }

        private List<ViewMenuModel> GetMenus()
        {
            return _business.GetMenus(Guid.NewGuid());
        }


        //public MenuModel()
        //{
        //    //MenuItems = new List<ViewMenuModel>
        //    //{
        //    //    new ViewMenuModel
        //    //    {
        //    //        DisplayName = "Employee",
        //    //        SubMenu = new List<ViewSubMenuModel>
        //    //        {
        //    //            new ViewSubMenuModel { DisplayName = "Create Employee", Controller = "Employee", Method = "Create" },
        //    //            new ViewSubMenuModel { DisplayName = "View Employee", Controller = "Employee", Method = "List" }
        //    //        }
        //    //    },
        //    //    new ViewMenuModel
        //    //    {
        //    //        DisplayName = "User",
        //    //        SubMenu = new List<ViewSubMenuModel>
        //    //        {
        //    //            new ViewSubMenuModel { DisplayName = "Register User", Controller = "User", Method = "Register" },
        //    //            new ViewSubMenuModel { DisplayName = "View Users", Controller = "User", Method = "Index" },
        //    //            new ViewSubMenuModel { DisplayName = "Profile", Controller = "User", Method = "Profile" }
        //    //        }
        //    //    },
        //    //    new ViewMenuModel
        //    //    {
        //    //        DisplayName = "Sub Category",
        //    //        SubMenu = new List<ViewSubMenuModel>
        //    //        {
        //    //            new ViewSubMenuModel { DisplayName = "Create", Controller = "SubCategory", Method = "Index" },
        //    //            new ViewSubMenuModel { DisplayName = "Mapping", Controller = "CategorySubCategoryMapping", Method = "Index" }
        //    //        }
        //    //    },
        //    //    new ViewMenuModel
        //    //    {
        //    //        DisplayName = "Category",
        //    //        SubMenu = new List<ViewSubMenuModel>
        //    //        {
        //    //            new ViewSubMenuModel { DisplayName = "Create", Controller = "Category", Method = "Index" }
        //    //        }
        //    //    }
        //    //};
        //}
    }

}
