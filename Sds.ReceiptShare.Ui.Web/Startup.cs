using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sds.ReceiptShare.Core.ApplicationSettings;
using Sds.ReceiptShare.Data;
using Sds.ReceiptShare.Data.Repository;
using Sds.ReceiptShare.Domain.Entities;
using Sds.ReceiptShare.Logic.Interfaces;
using Sds.ReceiptShare.Logic.Managers;
using Sds.ReceiptShare.Ui.Web.Models;
using Sds.ReceiptShare.Ui.Web.Services;

namespace Sds.ReceiptShare.Ui.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see https://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets<Startup>();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<DataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Database")));

            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<DataContext>();

            // If you want to tweak Identity cookies, they're no longer part of IdentityOptions.
            services.ConfigureApplicationCookie(options => options.LoginPath = "/Account/LogIn");
            services.AddAuthentication()
                    .AddFacebook(options => {
                        options.AppId = Configuration["Authentication:Facebook:AppId"];
                        options.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
                    });

            services.AddScoped<IRepository, Repository>();
            services.AddScoped<IGroupManager, GroupManager>();
            services.AddScoped<ILookupManager, LookupManager>();
            services.AddScoped<ApplicationUserManager>();
            services.AddMvc();

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            services.Configure<StartupSettings>(Configuration.GetSection("Startup"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, DataContext context)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            var recreateDatabase = Configuration.GetSection("Startup").GetValue<bool>("RecreateDatabase");

            DataInitialiser.Initialize(context, recreateDatabase);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
