﻿using IdentityApplication.Core.Entities;
using IdentityApplication.Core.ViewModel;

namespace IdentityApplication.Business.Contracts
{
    public interface IEmployeeBusiness
    {
        void Create(InsertEmployeeRequest request);
        Task<PaginationResponse<Employee>> GetAll(PaginationFilter filter);
        Task<Employee?> GetById(string id, string month);
        Task<ListEmployeeRequest> Update(ListEmployeeRequest request, bool isAdmin);
        Task Delete(string id, string month);
    }
}
