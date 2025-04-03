using AutoMapper;
using BookTrader.Data;
using BookTrader.Mapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using BookTrader.Models;
using BookTrader.Services;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



builder.Services.AddIdentity<Users, IdentityRole>(options =>
{
    options.Password.RequireNonAlphanumeric = false; 
    options.Password.RequiredLength = 8;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedAccount = false; 
    options.SignIn.RequireConfirmedEmail = false;   
    options.SignIn.RequireConfirmedPhoneNumber = false; 
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";  // Redirige si no está autenticado
    options.AccessDeniedPath = "/Account/AccessDenied"; // Redirige si no tiene permisos
});

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

var app = builder.Build();

await SeedService.SeedDatabase(app.Services);


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
 
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


//async Task CrearRoles(IServiceProvider serviceProvider)
//{
//    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
//    var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

//    string[] roles = { "Admin", "User" };

//    foreach (var role in roles)
//    {
//        if (!await roleManager.RoleExistsAsync(role))
//        {
//            await roleManager.CreateAsync(new IdentityRole(role));
//        }
//    }

//    // Crea un usuario administrador predeterminado, si no existe
//    string adminEmail = "admin@booktrader.com";
//    string adminPassword = "Admin123!";

//    var adminUser = await userManager.FindByEmailAsync(adminEmail);
//    if (adminUser == null)
//    {
//        var newAdmin = new IdentityUser { UserName = adminEmail, Email = adminEmail };
//        await userManager.CreateAsync(newAdmin, adminPassword);
//        await userManager.AddToRoleAsync(newAdmin, "Admin");
//    }
//}

//// Llama a la inicialización de roles en un scope antes de app.Run()
//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    await CrearRoles(services);
//}


app.Run();
