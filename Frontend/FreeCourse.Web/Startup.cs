using FreeCourse.Web.Models;
using FreeCourse.Web.Services;
using FreeCourse.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace FreeCourse.Web
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
            services.Configure<ClientSettings>(Configuration.GetSection(nameof(ClientSettings)));
            services.Configure<ServiceApiSettings>(Configuration.GetSection(nameof(ServiceApiSettings)));

            var serviceApiSettings = Configuration.GetSection("ServiceApiSettings").Get<ServiceApiSettings>();

            services.AddHttpClient<IIdentityService,IdentityService>();
            services.AddHttpClient<IUserService, UserService>(opt =>
            {
               opt.BaseAddress = new Uri(serviceApiSettings.IdentityBaseUri);
              
            });
            services.AddHttpContextAccessor();
          
           
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie
                (CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.LoginPath = "/Auth/SignIn";
                    options.ExpireTimeSpan = TimeSpan.FromDays(60);                         // Cookie Base Authentication
                    options.SlidingExpiration = true;
                    options.Cookie.Name = "WebCookie";
                });
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
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
        }
    }
}
