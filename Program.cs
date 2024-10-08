using CSVFile.Data;
using CSVFile.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Read connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Add context with SQL Server
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

// Add application identity
builder.Services.AddIdentity<IdentityUser<Guid>, IdentityRole<Guid>>(options => options.SignIn.RequireConfirmedAccount = false)
	.AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddTransient<ICSVDataService, CSVDataService>();


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

app.Run();
