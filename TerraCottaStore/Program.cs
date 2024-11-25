using Microsoft.EntityFrameworkCore;
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
var app = builder.Build();

app.UseSession();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
//Seeding data
var context = app.Services.CreateScope().ServiceProvider.GetRequiredService<DataContext>(); 
    SeedData.SeedingData(context);
app.Run();
