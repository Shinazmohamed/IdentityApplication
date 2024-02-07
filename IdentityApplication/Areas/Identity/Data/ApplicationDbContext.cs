using IdentityApplication.Core;
using IdentityApplication.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Security.Claims;

namespace IdentityApplication.Areas.Identity.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<Employee> Employee => Set<Employee>();
    public DbSet<Location> Location => Set<Location>();
    public DbSet<Department> Department => Set<Department>();
    public DbSet<Category> Category => Set<Category>();
    public DbSet<SubCategory> SubCategory => Set<SubCategory>();
    public DbSet<Menu> Menu => Set<Menu>();
    public DbSet<SubMenu> SubMenu => Set<SubMenu>();
    public DbSet<SubMenuRole> SubMenuRoles => Set<SubMenuRole>();
    public DbSet<Audit> AuditLogs { get; set; }
    public DbSet<Permission> Permission { get; set; }
    public DbSet<Entity> Entity { get; set; }

    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        : base(options)
    {
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        #region Table Definitions
        builder.Entity<ApplicationUser>()
            .ToTable(name: "User")
            .HasOne(u => u.Location)
            .WithMany()
            .HasForeignKey(u => u.LocationId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Entity<IdentityRole>(entity =>
        {
            entity.ToTable(name: "Role");
        });
        builder.Entity<IdentityUserRole<string>>(entity =>
        {
            entity.ToTable("UserRoles");
        });
        builder.Entity<IdentityUserClaim<string>>(entity =>
        {
            entity.ToTable("UserClaims");
        });
        builder.Entity<IdentityUserLogin<string>>(entity =>
        {
            entity.ToTable("UserLogins");
        });
        builder.Entity<IdentityRoleClaim<string>>(entity =>
        {
            entity.ToTable("RoleClaims");
        });
        builder.Entity<IdentityUserToken<string>>(entity =>
        {
            entity.ToTable("UserTokens");
        });

        string employeeTableName = _configuration.GetSection("AppSettings")["TagEmployeeTableName"];
        if (!string.IsNullOrEmpty(employeeTableName))
        {
            builder.Entity<Employee>().ToTable(employeeTableName);
        }
        #endregion

        #region Relationships
        builder.Entity<Category>()
       .HasMany(c => c.SubCategories)
       .WithOne(sc => sc.Category)
       .HasForeignKey(sc => sc.CategoryId)
       .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Menu>()
                .HasMany(c => c.SubMenus)
                .WithOne(sc => sc.Menu)
                .HasForeignKey(sc => sc.MenuId)
                .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<SubMenuRole>()
            .HasKey(sr => new { sr.SubMenuId, sr.Id });

        builder.Entity<SubMenuRole>()
            .HasOne(sr => sr.SubMenu)
            .WithMany(s => s.SubMenuRoles)
            .HasForeignKey(sr => sr.SubMenuId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        builder.Entity<SubMenuRole>()
            .HasOne(sr => sr.Role)
            .WithMany(r => r.SubMenuRoles)
            .HasForeignKey(sr => sr.Id)
            .OnDelete(DeleteBehavior.ClientSetNull);

        builder.Ignore<ApplicationRole>();

        #endregion

        #region Index
        builder.Entity<Audit>()
    .       HasIndex(a => a.DateTime);

        builder.Entity<Employee>()
            .HasIndex(a => new { a.LocationName, a.CategoryName, a.SubCategoryName, a.DepartmentName });
        #endregion
    }

    private void OnBeforeSaveChanges(string userId)
    {
        ChangeTracker.DetectChanges();
        var auditEntries = new List<AuditEntry>();
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is Audit || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                continue;
            var auditEntry = new AuditEntry(entry);
            auditEntry.TableName = entry.Entity.GetType().Name;
            auditEntry.UserId = userId;
            auditEntries.Add(auditEntry);
            foreach (var property in entry.Properties)
            {
                string propertyName = property.Metadata.Name;
                if (property.Metadata.IsPrimaryKey())
                {
                    auditEntry.KeyValues[propertyName] = property.CurrentValue;
                    continue;
                }
                switch (entry.State)
                {
                    case EntityState.Added:
                        auditEntry.AuditType = AuditType.Create;
                        auditEntry.NewValues[propertyName] = property.CurrentValue;
                        break;
                    case EntityState.Deleted:
                        auditEntry.AuditType = AuditType.Delete;
                        auditEntry.OldValues[propertyName] = property.OriginalValue;
                        break;
                    case EntityState.Modified:
                        if (property.IsModified)
                        {
                            auditEntry.ChangedColumns.Add(propertyName);
                            auditEntry.AuditType = AuditType.Update;
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                        }
                        break;
                }
            }
        }
        foreach (var auditEntry in auditEntries)
        {
            AuditLogs.Add(auditEntry.ToAudit());
        }
    }

    public override int SaveChanges()
    {
        OnBeforeSaveChanges(GetCurrentUserEmail());
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        OnBeforeSaveChanges(GetCurrentUserEmail());
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private string GetCurrentUserEmail()
    {
        var userEmail = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
        return userEmail ?? "DefaultUserEmail";
    }

}
