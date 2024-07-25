using C_DiscApp.Data;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using C_DiscApp.Models;
using Microsoft.AspNetCore.Identity;

namespace C_DiscApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            //var configuration = builder.Configuration;

            builder.Services.AddMemoryCache();
            builder.Services.AddSession();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<DiscContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
            })
            .AddEntityFrameworkStores<DiscContext>()
            .AddDefaultTokenProviders();

            // Register IHttpClientFactory
            builder.Services.AddHttpClient();

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
            app.UseAuthentication();

            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
