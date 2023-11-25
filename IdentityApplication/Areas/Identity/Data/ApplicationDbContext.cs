using IdentityApplication.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace IdentityApplication.Areas.Identity.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<Employee> Employee => Set<Employee>();
    public DbSet<Location> Location => Set<Location>();
    public DbSet<Department> Department => Set<Department>();
    public DbSet<Category> Category => Set<Category>();
    public DbSet<SubCategory> SubCategory => Set<SubCategory>();
    public DbSet<CategoryMapping> CategoryMapping => Set<CategoryMapping>();

    private readonly IConfiguration _configuration;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        string employeeTableName = _configuration.GetSection("AppSettings")["TagEmployeeTableName"];
        if (!string.IsNullOrEmpty(employeeTableName))
        {
            builder.Entity<Employee>().ToTable(employeeTableName);
        }

        builder.Entity<CategoryMapping>()
            .HasKey(cs => new { cs.CategoryId, cs.SubCategoryId });

        builder.Entity<CategoryMapping>()
            .HasOne(cs => cs.Category)
            .WithMany(c => c.CategorySubcategories)
            .HasForeignKey(cs => cs.CategoryId);

        builder.Entity<CategoryMapping>()
            .HasOne(cs => cs.SubCategory)
            .WithMany(s => s.CategorySubcategories)
            .HasForeignKey(cs => cs.SubCategoryId);
    }
}
