﻿using IdentityApplication.Business.Contracts;
using IdentityApplication.Core;
using IdentityApplication.Core.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApplication.Controllers
{
    [Authorize]
    public class AuditController : Controller
    {
        private readonly IAuditBusiness _business;

        public AuditController(IAuditBusiness business)
        {
            _business = business;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = $"{Constants.Roles.Administrator}")]
        [HttpPost]
        public async Task<IActionResult> GetList([FromBody] PaginationFilter filter)
        {
            var response = await _business.GetAllWithFilters(filter);
            var jsonD = new
            {
                filter.draw,
                recordsTotal = response.TotalCount,
                recordsFiltered = response.TotalCount,
                data = response.Data
            };
            return Json(jsonD);
        }
    }
}