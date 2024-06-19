using Demo.BLl.Interfacies;
using Demo.BLl.Repositories;
using Demo.DAL.Data;
using Demo.DAL.Models;
using Demo.PL.Extetions;
using Demo.PL.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region  ConfigureServices
            builder.Services.AddControllersWithViews();


            //defualt is addScoped
            builder.Services.AddDbContext<AppDbConText>(options =>
            {
                options/*.UseLazyLoadingProxies()*/.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            }
            , ServiceLifetime.Scoped);

            builder.Services.AddApplicationServices();
            builder.Services.AddAutoMapper(m => m.AddProfile(new MappingProfile()));
            builder.Services.AddScoped<IunitOfWork, UnitOfWork>();

            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(config =>
            {
                config.Password.RequiredUniqueChars = 0;
                config.Password.RequireUppercase = false;
                config.Password.RequireLowercase = false;
                config.Password.RequiredLength = 3;
                config.Password.RequireNonAlphanumeric = false;
                config.User.RequireUniqueEmail = true;
                
            }).AddEntityFrameworkStores<AppDbConText>().AddDefaultTokenProviders();
            builder.Services.ConfigureApplicationCookie(Config =>
            {
                Config.LoginPath = "/Account/LogIn";
                Config.AccessDeniedPath = "/Home";
               
            });

            #endregion
           
            var app = builder.Build();

            #region Configure
            if (builder.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
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


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            #endregion

            app.Run();

        }

       
    }
}
