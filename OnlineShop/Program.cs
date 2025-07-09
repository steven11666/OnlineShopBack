using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models; 
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<OnlineShopContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("OnlineShopContext")
        ?? throw new InvalidOperationException("Connection string 'OnlineShopContext' not found.")));

builder.Services.AddDbContext<OnlineShopUserContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("OnlineShopUserContextConnection")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")));

builder.Services.AddIdentity<OnlineShopUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<OnlineShopUserContext>()
.AddDefaultTokenProviders();



builder.Services.AddRazorPages();
builder.Services.AddSingleton<Microsoft.AspNetCore.Identity.UI.Services.IEmailSender, DummyEmailSender>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); 
app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
