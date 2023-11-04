using Microsoft.EntityFrameworkCore;
using IdentityApplication.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using IdentityApplication.Core;
using IdentityApplication.Core.Contracts;
using IdentityApplication.Core.Repositories;
using IdentityApplication.Business.Contracts;
using IdentityApplication.Business;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

#region Authorization
AddAuthorizationPolicies(builder.Services);
#endregion

AddScoped();

var app = builder.Build();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();

void AddAuthorizationPolicies(IServiceCollection services)
{
    services.AddAuthorization(options =>
    {
        options.AddPolicy(Constants.Policies.RequireAdmin, policy => policy.RequireClaim(Constants.Roles.Administrator));
    });
    services.AddAuthorization(options =>
    {
        options.AddPolicy(Constants.Policies.RequireUser, policy => policy.RequireClaim(Constants.Roles.User));
    });
}

void AddScoped()
{
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IRoleRepository, RoleRepository>();
    builder.Services.AddScoped<ILocationRepository, LocationRepository>();
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); 
    builder.Services.AddScoped<IEmployeeBusiness, EmployeeBusiness>();
    builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
}