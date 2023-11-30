﻿using IdentityApplication.Core.Entities;

namespace IdentityApplication.Core.Contracts
{
    public interface IMenuRepository
    {
        IList<Menu> GetMenus();
    }
}
