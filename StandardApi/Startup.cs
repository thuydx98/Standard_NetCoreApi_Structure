using System;
using System.Text;
using DryIoc;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StandardApi.Common.Mail;
using StandardApi.Constants;
using StandardApi.Data.Services;
using StandardApi.Framework.Authentication;
using StandardApi.Framework.Authorization;
using StandardApi.Framework.Configuration;
using StandardApi.Framework.Configuration.Options;
using StandardApi.Framework.Configuration.Swagger;
using StandardApi.Framework.IoCRegistrar;

namespace StandardApi
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _env;

        public Startup(IHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            _env = env;
            _configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            services.Configure<MailServerSetting>(_configuration.GetSection("MailServerSetting"));

            var jwtSettingOptions = _configuration.GetSection("JwtOptions");
            services.Configure<JwtOptions>(options =>
            {
                options.Issuer = jwtSettingOptions["Issuer"];
                options.Audience = jwtSettingOptions["Audience"];
                options.SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettingOptions["SecretKey"])), SecurityAlgorithms.HmacSha256);
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "StandardApi API", Version = "v1" });
                c.OperationFilter<AuthorizationHeaderParameterOperationFilter>();
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddResponseCaching();

            services.AddCors();
            //services.AddCors(options =>
            //{
            //    options.AddDefaultPolicy(
            //        builder =>
            //        {
            //            builder.WithOrigins(_configuration.GetAllowOrigins())
            //                .AllowAnyHeader()
            //                .AllowAnyMethod();
            //        });
            //});

            services.AddControllers(option =>
            {
                option.EnableEndpointRouting = false;
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                option.Filters.Add(new AuthorizeFilter(policy));
            })
            .AddControllersAsServices()
            .AddNewtonsoftJson()
            .ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddSingleton(ctx => _configuration);
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(_configuration.GetDbConnectionString(_env?.EnvironmentName)));

            services.AddAuthentication(options => options.DefaultScheme = "StandardApi")
                .AddCookie("StandardApi.CookieSchema", options =>
                {
                    options.LoginPath = _configuration.GetLoginPath();
                    options.LogoutPath = _configuration.GetLogoutPath();
                    options.ExpireTimeSpan = TimeSpan.FromHours(JwtValidTime.NOT_HAVE_REMEMBER_ME_BY_HOURS);
                    options.SlidingExpiration = true;
                })
                .UseAuthValidation("StandardApi", validateOptions =>
                {
                    validateOptions = new AuthValidationOptions
                    {
                        LoginPath = _configuration.GetLoginPath(),
                    };
                });

            services.ConfigurePermissions();

            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(_configuration.GetHangfireConnectionString(_env?.EnvironmentName), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));

            services.AddHangfireServer();
        }

        public static IContainer CreateConfiguredContainer() =>
            new Container(rules =>
                rules.With(propertiesAndFields: request =>
                    request.ServiceType.Name.EndsWith("Controller") ? PropertiesAndFields.Properties()(request) : null)
            );

        public void ConfigureContainer(IContainer container)
        {
            CompositionRoot.Register(container);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseResponseCaching();
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "StandardApi V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseHangfireDashboard("/hangfire");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
