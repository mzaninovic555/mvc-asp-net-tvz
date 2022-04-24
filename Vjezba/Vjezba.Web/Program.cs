using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation();

builder.Services.AddDbContext<Vjezba.DAL.ClientManagerDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("ClientManagerDbContext"),
        opt => opt.MigrationsAssembly("Vjezba.DAL")));

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

app.MapControllerRoute(
    name: "contact_ruta",
    pattern: "/kontakt-forma",
    new { controller = "Home", action = "Contact" });

app.MapControllerRoute(
    name: "privacy_ruta",
    pattern: "o-aplikaciji/{lang}",
    new { controller="Home", action="Privacy" },
    constraints: new { lang = @"[a-zA-Z]{2}"}
    );


//MockClientRepository.Instance.Initialize(Path.Combine(app.Environment.WebRootPath, "data"));
//MockCityRepository.Instance.Initialize(Path.Combine(app.Environment.WebRootPath, "data"));

app.Run();
