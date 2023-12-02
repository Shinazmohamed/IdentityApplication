﻿using IdentityApplication.Core.ViewModel;

namespace IdentityApplication.Business.Contracts
{
    public interface IMenuBusiness
    {
        List<MenuViewModel> GetMenus(Guid? roleId);
    }
}