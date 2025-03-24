using AutoMapper;
using BookTrader.Data;
using BookTrader.Mapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    // Puedes personalizar las opciones de Identity (ej: requisitos de contraseña)
    options.SignIn.RequireConfirmedAccount = false;
})
.AddRoles<IdentityRole>() // Habilita roles, ej: Admin y User
.AddEntityFrameworkStores<ApplicationDbContext>();



builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

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
