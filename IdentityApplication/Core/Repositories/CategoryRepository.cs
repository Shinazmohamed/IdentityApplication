using IdentityApplication.Areas.Identity.Data;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.Entities;

namespace IdentityApplication.Core.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IList<Category> GetCategories()
        {
            return _context.Category.ToList();
        }
        public Category GetCategoryById(Guid Id)
        {
            return _context.Category.FirstOrDefault(l => l.CategoryId == Id);
        }
        public Category GetCategoryByName(string Name)
        {
            return _context.Category.FirstOrDefault(l => l.CategoryName == Name);
        }
    }
}
