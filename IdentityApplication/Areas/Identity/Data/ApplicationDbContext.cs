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

        builder.Entity<Category>()
               .HasMany(c => c.SubCategories)
               .WithOne(sc => sc.Category)
               .HasForeignKey(sc => sc.CategoryId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
