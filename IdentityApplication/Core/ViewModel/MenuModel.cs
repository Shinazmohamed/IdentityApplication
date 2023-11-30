namespace IdentityApplication.Core.ViewModel
{
    public class MenuModel
    {
        public List<ViewMenuModel> MenuItems { get; set; }

        public MenuModel()
        {
            MenuItems = new List<ViewMenuModel>
            {
                new ViewMenuModel
                {
                    DisplayName = "Employee",
                    SubMenu = new List<ViewMenuModel>
                    {
                        new ViewMenuModel { DisplayName = "Create Employee", Controller = "Employee", Method = "Create" },
                        new ViewMenuModel { DisplayName = "View Employee", Controller = "Employee", Method = "List" }
                    }
                },
                new ViewMenuModel
                {
                    DisplayName = "User",
                    SubMenu = new List<ViewMenuModel>
                    {
                        new ViewMenuModel { DisplayName = "Register User", Controller = "User", Method = "Register" },
                        new ViewMenuModel { DisplayName = "View Users", Controller = "User", Method = "Index" },
                        new ViewMenuModel { DisplayName = "Profile", Controller = "User", Method = "Profile" }
                    }
                },
                new ViewMenuModel
                {
                    DisplayName = "Sub Category",
                    SubMenu = new List<ViewMenuModel>
                    {
                        new ViewMenuModel { DisplayName = "Create", Controller = "SubCategory", Method = "Index" },
                        new ViewMenuModel { DisplayName = "Mapping", Controller = "CategorySubCategoryMapping", Method = "Index" }
                    }
                },
                new ViewMenuModel
                {
                    DisplayName = "Category",
                    SubMenu = new List<ViewMenuModel>
                    {
                        new ViewMenuModel { DisplayName = "Create", Controller = "Category", Method = "Index" }
                    }
                }
            };
        }
    }

}
