using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Vjezba.DAL;
using Vjezba.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation();

builder.Services.AddDbContext<ClientManagerDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("ClientManagerDbContext"),
            opt => opt.MigrationsAssembly("Vjezba.DAL")));

builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ClientManagerDbContext>();

builder.Services.AddScoped<UserManager<AppUser>>();

builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
    googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
});

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

app.UseAuthentication();
app.UseAuthorization();

var supportedCultures = new[]
{
    new CultureInfo("hr"), new CultureInfo("en-US")
};

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("hr"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
    endpoints.MapRazorPages();
});

app.MapControllerRoute(
    name: "kontakt-forma",
    pattern: "kontakt-forma",
    defaults: new { controller = "Home", action = "Contact" });

app.MapControllerRoute(
    name: "o-aplikaciji",
    pattern: "o-aplikaciji/{lang:alpha:length(2)}",
    defaults: new { controller = "Home", action = "Privacy" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
