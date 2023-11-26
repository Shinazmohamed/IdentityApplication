using IdentityApplication.Areas.Identity.Data;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace IdentityApplication.Core.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CategorySubCategoryRepository> _logger;
        public CategoryRepository(ApplicationDbContext context, ILogger<CategorySubCategoryRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public IList<Category> GetCategories()
        {
            return _context.Category.ToList();
        }

        public Category GetCategoryById(Guid Id)
        {
            return _context.Category
                .Include(e => e.SubCategories)
                . FirstOrDefault(l => l.CategoryId == Id);
        }

        public Category GetCategoryByName(string Name)
        {
            return _context.Category.FirstOrDefault(l => l.CategoryName == Name);
        }
    }
}
