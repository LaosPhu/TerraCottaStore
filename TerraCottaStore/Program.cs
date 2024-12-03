using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TerraCottaStore.Models;
using TerraCottaStore.Repository;

var builder = WebApplication.CreateBuilder(args);
// connect DB
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration["ConnectionStrings:ConnedtedDb"]);
});
// Add services to the container.
builder.Services.AddControllersWithViews();
// sessioon builder
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(
    option =>
    {
        option.IdleTimeout = TimeSpan.FromMinutes(30);
        option.Cookie.IsEssential = true;
    }
    );


builder.Services.AddIdentity<AppUserModel,IdentityRole>()
	.AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();
//dbcontext= datacontext

builder.Services.Configure<IdentityOptions>(options =>
{
	// Password settings.
	options.Password.RequireDigit = true;
	options.Password.RequireLowercase = true;
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequireUppercase = false;
	options.Password.RequiredLength = 4;
	

	
	options.User.RequireUniqueEmail = true;
});



var app = builder.Build();
//404
//app.UseStatusCodePagesWithRedirects("/Home/Error?statuscode={0}");
//
app.UseSession();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
//Admin area
app.MapControllerRoute(
    name: "Areas",
    pattern: "{area:exists}/{controller=Product}/{action=Index}/{id?}");
//default
app.MapControllerRoute(
    name: "category",
    pattern: "/category/{Slug?}",
    defaults: new { controller = "Category", action = "Index" });
app.MapControllerRoute(
    name: "brand",
    pattern: "/brand/{slug?}",
    defaults: new { controller = "Brand", action = "Index" });
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//Seeding data
var context = app.Services.CreateScope().ServiceProvider.GetRequiredService<DataContext>(); 
    SeedData.SeedingData(context);
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<AppUserModel>>();

        // Seed dữ liệu
        await SeedData.SeedRolesAsync(roleManager);
        await SeedData.SeedUsersAsync(userManager, roleManager);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}
app.Run();
