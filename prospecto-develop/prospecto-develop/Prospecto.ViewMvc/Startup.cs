using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Prospecto.Data;
using Prospecto.Filters;
using Prospecto.IRespository;
using Prospecto.IService;
using Prospecto.Models.Constants;
using Prospecto.Repository;
using Prospecto.Repository.Interface;
using Prospecto.Service;
using Prospecto.Service.Interface;
using Prospecto.ViewMvc.Extensions;
using System;
using System.Globalization;

namespace Prospecto.ViewMvc
{
    public class Startup
    {
        private readonly IWebHostEnvironment _currentEnvironment;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _currentEnvironment = env;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureOptions(Configuration);

            services.AddControllersWithViews()
                    .AddNewtonsoftJson();

            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<ISystemSettingService, SystemSettingService>();
            services.AddScoped<ISystemSettingRepository, SystemSettingRepository>();

            services.AddScoped<System.Data.IDbConnection>(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var connectionString = configuration.GetConnectionString(ProjectVariableConstants.ProspectoConnectionString);
                return new MySql.Data.MySqlClient.MySqlConnection(connectionString);
            });

            services.AddDbContextPool<ProspectoContext>(options =>
            {
                options.UseMySql(
                    Configuration.GetConnectionString(ProjectVariableConstants.ProspectoConnectionString),
                    new MySqlServerVersion(new Version(8, 0, 17))
                );
            });

            services.AddDependencyInjection(_currentEnvironment, Configuration);

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 4;
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => { options.LoginPath = "/Login"; });

            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
                options.Filters.Add<ClaimsRequirementFilter>();
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var supportedCultures = new[] { new CultureInfo("pt-BR") };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("pt-BR", "pt-BR"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<ProspectoContext>();
                dbContext.Database.Migrate();
            }

            // Desativar HTTPS se estiver em desenvolvimento para evitar conflitos com o app mobile
            // Comentado para aceitar acesso por HTTP no app mobile
            // app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseCors("AllowAllOrigins");
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); // Para rotas como [Route("api/attendance")]
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Login}/{id?}");
            });
        }
    }
}
