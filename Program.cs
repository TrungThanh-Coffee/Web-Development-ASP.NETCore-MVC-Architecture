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



builder.Services.AddDbContext<project_cuoikyContext>(options =>
    options.UseSqlServer(connectionString)
);



builder.Services
    .AddDefaultIdentity<AppUser>(options =>
    {
     
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddEntityFrameworkStores<project_cuoikyContext>();



builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);

    options.Cookie.HttpOnly = true;

    options.Cookie.IsEssential = true;

    options.Cookie.Name = ".PerfumeShop.Cart";
});


builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();


var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}


app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();


app.UseSession();


app.UseAuthentication();

app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);


app.MapRazorPages();


app.Run();