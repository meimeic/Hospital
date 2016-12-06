using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using WebFramework.Identity;
using WebFramework.Services;
namespace WebFramework
{
    public class Startup
    {
        private readonly IHostingEnvironment _env;
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)//通用设置
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);//指定环境设置项

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
            _env = env;
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddIdentityWithMongoStoresUsingCustomTypes<ApplicationUser, ApplicationRole>(connectionString)
                .AddDefaultTokenProviders();

            //认证参数设置
            services.Configure<IdentityOptions>(options =>
            {
                // 密码参数
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;

                // 账户锁定参数
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // Cookies 参数
                options.Cookies.ApplicationCookie.ExpireTimeSpan = TimeSpan.FromDays(150);
                options.Cookies.ApplicationCookie.LoginPath = "/Account/LogIn";
                options.Cookies.ApplicationCookie.LogoutPath = "/Account/LogOff";

                // User settings
                options.User.RequireUniqueEmail = true;
                //var dataProtectionPath = Path.Combine(_env.WebRootPath, "identity-artifacts");
                //options.Cookies.ApplicationCookie.DataProtectionProvider = DataProtectionProvider.Create(dataProtectionPath);
                //options.Cookies.ApplicationCookie.AuthenticationScheme = "ApplicationCookie";
            });

            services.AddSingleton<RoleManager<ApplicationRole>, ApplicationRoleManager>();
            services.AddSingleton<UserManager<ApplicationUser>, ApplicationUserManager>();
            services.AddScoped<SignInManager<ApplicationUser>, ApplicationSignInManager>();

            services.AddMvc();

            //注册Email服务和短信服务
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.AddTransient<IEmailSender, AuthMessageSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseIdentity();
            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715

            //路由配置
            app.UseMvc(routes =>
            {
                //MVC 路由
                routes.MapRoute(
                    name: "mvc",
                    template: "{controller=Home}/{action=Index}/{id?}");
                //API 路由
                routes.MapRoute(
                name: "api",
                template: "api/{controller}/{id?}");
            });
        }
    }
}
