using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using NRZ.Web.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using NRZ.Shared.Localization;
using NRZ.Web.Services;
using NRZ.Models.Identity;
using NRZ.Data;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using NRZ.Services.Interfaces;
using NRZ.Services;
using NRZ.Shared;
using NRZ.Services.Notifications;
using NRZ.Models.Settings;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using NRZ.Services.Integration;
using NRZ.Services.Auctions;
using NRZ.Web.Hubs;
using NRZ.Services.EPayments;
using NRZ.Web.AutoTasks;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.SqlServer;
using NRZ.Services.Notifications.Job;

namespace NRZ.Web
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
            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                //This settings shrtens reset password token
                options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
                options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;

                // User settings.
                options.User.AllowedUserNameCharacters =
                    "‡·‚„‰ÂÊÁËÈÍÎÏÌÓÔÒÚÛÙıˆ˜¯˘˙¸˛ˇ¿¡∆√ƒ≈∆«»… ÀÃÕŒœ–—“”‘’÷◊ÿŸ⁄‹ﬁﬂabcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._";
                options.User.RequireUniqueEmail = false;
            })
               .AddEntityFrameworkStores<ApplicationDbContext>()
               .AddDefaultTokenProviders();

            services.AddDbContext<NRZContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddMemoryCache();
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddSignalR();

            //Hangfire
            //By default, ONLY LOCAL requests are allowed to access the Dashboard. Please
            //see the `Configuring Dashboard authorization` section in Hangfire documentation:
            //https://docs.hangfire.io/en/latest/configuration/using-dashboard.html#configuring-authorization
            services.AddHangfire(config =>
            {
                var options = new SqlServerStorageOptions
                {
                    PrepareSchemaIfNecessary = true,
                    QueuePollInterval = TimeSpan.FromMinutes(5)
                };

                config.UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection"), options);
            });
            services.AddHangfireServer();
            services.AddSignalR();

            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                        factory.Create(typeof(SharedResources));
                });

            services.Configure<RequestLocalizationOptions>(opts => { });

            InjectOptions(services);

            var tokenConfig = Configuration.GetSection("TokenConfig");


            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true; //Disable Automatic Model State Validation in ASP.NET Core 2.1
            });

            services
                .AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = tokenConfig["Audience"],
                        ValidIssuer = tokenConfig["Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfig["Secret"]))
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            // If the request is for our hub...
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                (path.StartsWithSegments("/auctions/hub")))
                            {
                                // Read the token out of the query string
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization();

            services.Configure<HttpsConnectionAdapterOptions>(options =>
            {
                options.ClientCertificateMode = ClientCertificateMode.RequireCertificate;
                options.CheckCertificateRevocation = false;
                options.ClientCertificateValidation = (certificate2, chain, policyErrors) =>
                {
                    // accept any cert (testing purposes only)
                    return true;
                };
            });

            ConfigureInjection(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Add this line; you'll need `using Serilog;` up the top, too
            app.UseSerilogRequestLogging();

            var supportedCultures = new string[] { "bg", "en" };
            app.UseRequestLocalization(options =>
                options
                    .AddSupportedCultures(supportedCultures)
                    .AddSupportedUICultures(supportedCultures)
                    .SetDefaultCulture("bg")
                    .RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(context =>
                    {
                        string userLangs = context.Request.Headers["Accept-Language"].ToString();
                        string firstLang = userLangs.Split(',').FirstOrDefault();
                        string defaultLang = string.IsNullOrEmpty(firstLang) ? "bg" : firstLang;
                        return Task.FromResult(new ProviderCultureResult(defaultLang, defaultLang));
                    }))
            );

            //Hangfire
            app.UseHangfireServer(new BackgroundJobServerOptions()
            {
                WorkerCount = 1
            });
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new IDashboardAuthorizationFilter[]
                {
                    new HangfireAuthorizationFilter()
                }
            });

            app.UseCors(options =>
            {
                options.AllowAnyHeader();
                options.AllowAnyMethod();
                options.AllowCredentials();
                options.WithOrigins(new string[]
                    {
                        "http://localhost:8080",
                        "http://localhost:8080/#/",
                        "http://localhost:8082",
                        "http://localhost:8082/#"
                    });
            });

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
                endpoints.MapRazorPages();
                endpoints.MapHub<AuctionHub>("/auctions/hub");
            });

            CreateRoles(serviceProvider);

            GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 1 });
            HangfireJobScheduler.ScheduleRecurringJobs(app.ApplicationServices);
        }

        private void InjectOptions(IServiceCollection services)
        {
            services.AddOptions();
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.Configure<TokenConfig>(Configuration.GetSection("TokenConfig"));
            services.Configure<RegiXCertificateSettings>(Configuration.GetSection("RegiXCertificateSettings"));
            services.Configure<ApplicationSettings>(Configuration.GetSection("Application"));
            services.Configure<IntegrationSettings>(Configuration.GetSection("IntegrationSettings"));
            services.Configure<EPaymentSettings>(Configuration.GetSection("EPaymentSettings"));
            services.Configure<EAuthSettings>(Configuration.GetSection("EAuthSettings"));
            services.Configure<HangFireJobSettings>(Configuration.GetSection("HangFireJobSettings"));
        }

        private void ConfigureInjection(IServiceCollection services)
        {
            //services.AddTransient<CheckTokenInBlacklistMiddleware>();
            services.AddTransient<IPersonService, PersonService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<INomenclatureService, NomenclatureService>();
            services.AddTransient<IPropertyService, PropertyService>();
            services.AddTransient<IIntegrationService, IntegrationService>();
            services.AddTransient<ILogService, LogService>();
            services.AddTransient<IDistraintService, DistraintService>();
            services.AddTransient<IAnnouncementsService, AnnouncementsService>();
            services.AddTransient<IAddressService, AddressService>();
            services.AddSingleton<ConfigurationService>();
            services.AddTransient<ISeizedPropertyAvailabilityRequestService, SeizedPropertyAvailabilityRequestService>();
            services.AddTransient<IRequestForCertificateOfDistraintOfPropertyService, RequestForCertificateOfDistraintOfPropertyService>();
            services.AddTransient<IAgriculturalMachineryService, AgriculturalMachineryService>();
            services.AddTransient<ICompanyService, CompanyService>();
            services.AddTransient<IAuctionRegisterService, AuctionRegisterService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<ITimestampService, TimestampService>();
            services.AddTransient<IEServicesSettingsService, EServicesSettingsService>();
            services.AddTransient<IEPaymentService, EPaymentService>();
            services.AddTransient<IEPaymentJobService, EPaymentJobService>();            
            services.AddTransient<AuthService>();

            //Hangfire jobs
            services.AddScoped<IPaymentRequestSendingJob, PaymentRequestSendingJob>();
            services.AddScoped<IPaymentRequestStatusCheckJob, PaymentRequestStatusCheckJob>();

            services.AddSingleton<IApplicationStoreService, ApplicationStoreService>();
            services.AddTransient<IAuctionService, AuctionService>();
        }

        private void CreateRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            string email = "admin@kontrax.bg";

            string[] roles = { Constants.Role_SysAdmin, Constants.Role_AuctionParticipant, Constants.Role_AuctionOrgaziner, Constants.Role_MJAdmin };

            foreach (var role in roles)
            {
                //Check that there is an Administrator role and create if not
                Task<bool> hasRole = roleManager.RoleExistsAsync(role);
                hasRole.Wait();

                if (!hasRole.Result)
                {
                    Task<IdentityResult> roleResult = roleManager.CreateAsync(new IdentityRole(role));
                    roleResult.Wait();
                }
            }

            //Check if the admin user exists and create it if not
            //Add to the Administrator role
            Task<ApplicationUser> testUser = userManager.FindByNameAsync(Constants.Role_SysAdmin);
            testUser.Wait();

            if (testUser.Result == null)
            {
                ApplicationUser administrator = new ApplicationUser();
                administrator.Email = email;
                administrator.EmailConfirmed = true;
                administrator.UserName = Constants.Role_SysAdmin;
                administrator.Deleted = false;
                administrator.ConfirmedByAdmin = true;
                administrator.AuthType = "USER";
                administrator.CreatedOn = DateTime.UtcNow;

                Task<IdentityResult> newUser = userManager.CreateAsync(administrator, Constants.DefaultPass);
                newUser.Wait();

                if (newUser.Result.Succeeded)
                {
                    Task<IdentityResult> newUserRole = userManager.AddToRoleAsync(administrator, Constants.Role_SysAdmin);
                    newUserRole.Wait();
                }
            }
        }
    }
}
