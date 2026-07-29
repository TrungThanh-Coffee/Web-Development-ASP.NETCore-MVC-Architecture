using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using project_cuoiky.Data;
using project_cuoiky.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration
    .GetConnectionString("project_cuoikyContextConnection")
    ?? throw new InvalidOperationException(
        "Connection string 'project_cuoikyContextConnection' not found."
    );

// Đăng ký DbContext, chỉ cần một lần
builder.Services.AddDbContext<project_cuoikyContext>(options =>
    options.UseSqlServer(connectionString)
);

// Đăng ký ASP.NET Core Identity
builder.Services
    .AddDefaultIdentity<AppUser>(options =>
    {
        // Đăng ký xong có thể đăng nhập ngay
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddEntityFrameworkStores<project_cuoikyContext>();

// Đăng ký MVC và Razor Pages
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Authentication phải đứng trước Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

// Cần thiết cho các trang Identity: Login, Register, Logout
app.MapRazorPages();

app.Run();