using AutoMapper;
using BookTrader.Data;
using BookTrader.Mapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using BookTrader.Models;
using BookTrader.Services;
using Microsoft.AspNetCore.HttpOverrides;
using AspNetCoreRateLimit;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddTransient<EmailSender>();

builder.Services.AddHttpClient<GoogleBookService>();

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

    // Políticas de seguridad
    options.Cookie.HttpOnly = true;                        
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;    
    options.Cookie.SameSite = SameSiteMode.Strict;            

    // -------------------------
    // Expiración y renovación
    options.ExpireTimeSpan = TimeSpan.FromDays(14);        
    options.SlidingExpiration = true;
});



builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddMemoryCache();

builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));


builder.Services.AddInMemoryRateLimiting();

builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

var app = builder.Build();

await SeedService.SeedDatabase(app.Services);


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedProto
});

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseIpRateLimiting();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
 
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");




app.Run();
