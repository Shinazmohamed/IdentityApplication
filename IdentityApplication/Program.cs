using Microsoft.EntityFrameworkCore;
using IdentityApplication.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.Repositories;
using IdentityApplication.Business.Contracts;
using IdentityApplication.Business;
using Serilog;
using IdentityApplication.Core.PermissionHelper;
using Microsoft.AspNetCore.Authorization;
using IdentityApplication.Middlewares;
using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString, sqlServerOptions => sqlServerOptions.CommandTimeout(300)));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();


builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

AddScoped();

//Add support to logging with SERILOG
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddNotyf(config => { config.DurationInSeconds = 3; config.IsDismissable = true; config.Position = NotyfPosition.TopRight; });

var app = builder.Build();

//Add support to logging request with SERILOG
app.UseSerilogRequestLogging();

using (var serviceScope = app.Services.CreateScope())
{
    var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseMiddleware<RequireLogoutMiddleware>();

app.UseNotyf();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();

void AddScoped()
{
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IRoleRepository, RoleRepository>();
    builder.Services.AddScoped<ILocationRepository, LocationRepository>();
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    builder.Services.AddScoped<IEmployeeBusiness, EmployeeBusiness>();
    builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

    builder.Services.AddScoped<ICategorySubCategoryRepository, CategorySubCategoryRepository>();
    builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
    builder.Services.AddScoped<ICategorySubCategoryBusiness, CategorySubCategoryBusiness>();
    builder.Services.AddScoped<ICategoryBusiness, CategoryBusiness>();
    builder.Services.AddScoped<ISubCategoryBusiness, SubCategoryBusiness>();
    builder.Services.AddScoped<ISubCategoryRepository, SubCategoryRepository>();
    builder.Services.AddScoped<IMenuBusiness, MenuBusiness>();
    builder.Services.AddScoped<IMenuRepository, MenuRepository>();

    builder.Services.AddScoped<ISubMenuBusiness, SubMenuBusiness>();
    builder.Services.AddScoped<ISubMenuRepository, SubMenuRepository>();

    builder.Services.AddScoped<IAuditBusiness, AuditBusiness>();
    builder.Services.AddScoped<IAuditRepository, AuditRepository>();

    builder.Services.AddScoped<IDepartmentBusiness, DepartmentBusiness>();
    builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();

    builder.Services.AddScoped<ICategoryDepartmentMappingBusiness, CategoryDepartmentMappingBusiness>();
    builder.Services.AddScoped<ICategoryDepartmentMappingRepository, CategoryDepartmentMappingRepository>();

    builder.Services.AddScoped<IPermissionBusiness, PermissionBusiness>();
    builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();

    builder.Services.AddScoped<IEntityBusiness, EntityBusiness>();
    builder.Services.AddScoped<IEntityRepository, EntityRepository>();

    builder.Services.AddScoped<IRoleBusiness, RoleBusiness>();
    builder.Services.AddScoped<IUserBusiness, UserBusiness>();

    builder.Services.AddScoped<IPreviousMonthEmployeeRepository, PreviousMonthEmployeeRepository>();
}