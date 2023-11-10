using IdentityApplication.Areas.Identity.Data;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.Entities;

namespace IdentityApplication.Core.Repositories
{
    public class SubCategoryRepository : ISubCategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public SubCategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IList<SubCategory> GetSubCategories()
        {
            return _context.SubCategory.ToList();
        }
        public SubCategory GetSubCategoryById(Guid Id)
        {
            return _context.SubCategory.FirstOrDefault(l => l.SubCategoryId == Id);
        }
        public SubCategory GetSubCategoryByName(string Name)
        {
            return _context.SubCategory.FirstOrDefault(l => l.SubCategoryName == Name);
        }
    }
}
