
using Building.BLL.Services.Implementations;
using Building.BLL.Services.Interfaces;
using Building.DAL;
using Building.DAL.Interfaces;
using Building.DAL.Repositories;
using Building.Domain.DTO;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string? cnn = builder.Configuration.GetConnectionString("Building");
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<BuildingContext>(opt => opt.UseSqlServer(cnn));

builder.Services.AddScoped<IQueryRepository, QueryRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IMaterialRepository, MaterialRepository>();
builder.Services.AddScoped<IQueryDetailsRepository, QueryDetailsRepository>();
builder.Services.AddScoped<IBuildingSiteRepository, BuildingSiteRepository>();
builder.Services.AddScoped<ICatatalogRepository, CatalogRepository>();
builder.Services.AddScoped<IQueryService, QueryService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IMaterialService, MaterialService>();
builder.Services.AddScoped<IQueryDetailsService, QueryDetailsService>();
builder.Services.AddScoped<IBuildingSiteService, BuildingSiteService>();
builder.Services.AddScoped<ICatalogService, CatalogService>();


builder.Services.AddAutoMapper(typeof(AppMappingProfile));


builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", conf =>
    {
        conf.LoginPath = "/Authentication/Index";
        conf.AccessDeniedPath = "/Login/Denied";
    });

builder.Services.AddAuthorization();

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
    pattern: "{controller=Authentication}/{action=Index}/{id?}");
app.Run();
