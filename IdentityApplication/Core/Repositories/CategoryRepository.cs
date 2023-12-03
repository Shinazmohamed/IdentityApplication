﻿using IdentityApplication.Areas.Identity.Data;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.Entities;
using IdentityApplication.Core.ViewModel;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace IdentityApplication.Core.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CategoryRepository> _logger;
        private readonly IMemoryCache _cache;
        public CategoryRepository(ApplicationDbContext context, ILogger<CategoryRepository> logger, IMemoryCache cache)
        {
            _context = context;
            _logger = logger;
            _cache = cache;
        }

        public IList<Category> GetCategories()
        {
            const string cacheKey = "Categories";

            if (_cache.TryGetValue(cacheKey, out IList<Category> cachedCategories))
            {
                return cachedCategories;
            }

            var categories = _context.Category.ToList();

            _cache.Set(cacheKey, categories, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            });

            return categories;
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
        public async Task<PaginationResponse<ListCategoryModel>> GetEntitiesWithFilters(PaginationFilter filter)
        {
            try
            {
                var query = _context.Category.OrderBy(e => e.CategoryId).AsQueryable();

                if (!string.IsNullOrEmpty(filter.category))
                {
                    query = query.Where(e => e.CategoryName == filter.category);
                }

                var totalCount = await query.CountAsync();
                var filteredEntities = await query.Skip(filter.start).Take(filter.length).ToListAsync();

                var resultViewModel = filteredEntities.Select(entity => new ListCategoryModel
                {
                    Id = entity.CategoryId,
                    Name = entity.CategoryName
                }).ToList();

                return new PaginationResponse<ListCategoryModel>(
                    resultViewModel,
                    totalCount,
                    filter.draw,
                    filter.length
                );
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Repo} All function error", typeof(CategoryRepository));
                throw;
            }
        }
        public void Create(Category request)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var existingMapping = _context.Category
                        .FirstOrDefault(e => e.CategoryName == request.CategoryName);

                    if (existingMapping != null)
                    {
                        throw new ArgumentNullException(nameof(existingMapping));
                    }

                    _context.Category.Add(request);
                    _context.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    _logger.LogError(e, "{Repo} All function error", typeof(CategoryRepository));
                    throw;
                }
            }
        }
        public async Task Delete(Guid id)
        {
            try
            {
                var entity = await _context.Category.FirstOrDefaultAsync(e => e.CategoryId == id);
                _context.Category.Remove(entity);
                _context.SaveChanges();

            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Repo} All function error", typeof(CategoryRepository));
                throw;
            }
        }
        public void Update(Category entity)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (entity == null)
                        throw new ArgumentNullException(nameof(entity));

                    var existingMapping = _context.Category
                        .FirstOrDefault(e => e.CategoryId == entity.CategoryId);

                    if (existingMapping == null)
                    {
                        throw new ArgumentNullException(nameof(entity));
                    }

                    existingMapping.CategoryName = entity.CategoryName;
                    _context.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    _logger.LogError(e, "{Repo} All function error", typeof(CategoryRepository));
                    throw;
                }
            }
        }
    }
}
