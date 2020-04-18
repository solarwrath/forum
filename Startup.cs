using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FORUM_PROJECT.DAL;
using FORUM_PROJECT.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;

namespace FORUM_PROJECT
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews()
                //Enable hot reloading
                .AddRazorRuntimeCompilation();

            services.AddHttpContextAccessor();

            services.AddDbContext<ForumContext>(options =>
                options.UseSqlServer(Configuration["connectionString"])
                );

            //Specify security requirements
            services.AddIdentity<User, IdentityRole>(config =>
                {
                    config.User.RequireUniqueEmail = true;

                    config.Password.RequiredLength = 6;
                    config.Password.RequireDigit = false;
                    config.Password.RequireNonAlphanumeric = false;
                    config.Password.RequireUppercase = true;

                    config.SignIn.RequireConfirmedEmail = false;
                })
                .AddEntityFrameworkStores<ForumContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "ForumIdentity.Cookie";
                config.LoginPath = "/Auth/Login";
            });

            //Need to send confirmation email
            services.AddMailKit(config => config.UseMailKit(Configuration.GetSection("MailKit").Get<MailKitOptions>(), ServiceLifetime.Singleton));

            services.AddScoped<IGenericRepository<Topic>, GenericRepository<Topic>>();
            services.AddScoped<TopicService>();

            services.AddScoped<IGenericRepository<User>, GenericRepository<User>>();
            services.AddScoped<UserService>();

            services.AddScoped<IGenericRepository<Post>, GenericRepository<Post>>();
            services.AddScoped<PostService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
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
        }
    }
}
