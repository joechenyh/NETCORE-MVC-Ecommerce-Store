using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NETCORE_CA_8A.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;
using NETCORE_CA_8A.DB;

namespace NETCORE_CA_8A
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(30000);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddDbContext<StoreDbContext>(opt =>
               opt.UseLazyLoadingProxies()
               .UseSqlServer(Configuration.GetConnectionString("DbConn")));

            services.AddSession();
            services.AddHttpContextAccessor();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,StoreDbContext dbcontext)
        {
            if (env.IsDevelopment())
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

            app.UseAuthorization();
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                  name: "default",
                  pattern: "{controller=Gallery}/{action=Gallery}/{username?}");

                endpoints.MapControllerRoute(
                    name: "add",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
               
                endpoints.MapControllerRoute(
                   name: "add",
                   pattern: "{controller=Cart}/{action=AddtoCart}/{productId?}/{fromProdDetail?}");
                endpoints.MapControllerRoute(
                  name: "add1",
                  pattern: "{controller=Product}/{action=View}/{product?}");

            });
            dbcontext.Database.EnsureDeleted();
            dbcontext.Database.EnsureCreated();

            new DBSeeder(dbcontext);
            new DBTester(dbcontext);
        }
    }
}
