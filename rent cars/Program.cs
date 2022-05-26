using Microsoft.EntityFrameworkCore;
using veggga.Persistence;
using AutoMapper;
using veggga.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.-----------------------------------------------------------------------------------


// service
builder.Services.AddDbContext<VegaDbContext>(opions => opions.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllersWithViews();

//repositry interface
builder.Services.AddScoped<IVehicleRepositry, VehicleRepositry>();


//------------------------cors
builder.Services.AddCors(); //
//------------------------cors

    //Auth0
    builder.Services.AddMvc();

    // 1. Add Authentication Services
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.Authority = "https://dev-ukc9qf8b.us.auth0.com/";
        options.Audience = "https://mraish/mohammed";
    });


var app = builder.Build();

//  services.-----------------------------------------------------------------------------------




// Configure the HTTP request pipeline.
/*if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.

}*/


//cors-------------------------------
app.UseCors(builder => builder     //
     .AllowAnyOrigin()             //
     .AllowAnyMethod()             // 
     .AllowAnyHeader());           //
//cors-------------------------------

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();
