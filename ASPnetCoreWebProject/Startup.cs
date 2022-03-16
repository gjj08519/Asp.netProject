using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PizzaWebsite.Data.Repositories;
using PizzaWebsite.Data;
using PizzaWebsite.Services.GoogleMaps;
using PizzaWebsite.Services.reCAPTCHA_v2;
using PizzaWebsite.Services.SendGrid;
using System;
using PizzaWebsite.Data.Seeder;

namespace PizzaWebsite
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
            services.AddDbContext<UserIdentityDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("UserIdentityConnection")));

            // add database context
            services.AddDbContext<PizzaWebsiteDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("PizzaWebsiteConnection"))
                        .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information)
                        .EnableSensitiveDataLogging()
                        // No tracking on entries to avoid recursive inserting child properties of the model
                        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            // add repository
            services.AddScoped<IUserIdentityRepository, UserIdentityRepository>();
            services.AddScoped<IPizzaWebsiteRepository, PizzaWebsiteRepository>();

            // add data seeder
            services.AddTransient<UserIdentityDataSeeder>();
            services.AddTransient<PizzaWebsiteDataSeeder>();

            // add sessions
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(600);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<UserIdentityDbContext>();

            //services.AddIdentity<IdentityUser, IdentityRole>()
            //.AddEntityFrameworkStores<ApplicationDbContext>()
            //.AddDefaultUI()
            //.AddDefaultTokenProviders();

            // add reCAPTCHA verfier to controller
            services.AddTransient<IReCaptchaVerifier, ReCaptchaVerifier>();

            services.Configure<ReCaptchaOptions>(options =>
            {
                options.SiteKey = Configuration["ExternalProviders:reCAPTCHA_v2:SiteKey"];
                options.SecretKey = Configuration["ExternalProviders:reCAPTCHA_v2:SecretKey"];
            });

            // set up Google Maps options
            services.Configure<GoogleMapsOptions>(options =>
            {
                options.ApiKey = Configuration["ExternalProviders:GoogleMaps:ApiKey"];
                options.CompanyAddress = Configuration["ExternalProviders:GoogleMaps:CompanyAddress"];
            });

            services.AddTransient<IGeocoder, Geocoder>();

            // add SendGrid email sender to controller
            services.AddTransient<IEmailSender, SendGridEmailSender>();

            services.Configure<SendGridEmailSenderOptions>(options =>
            {
                options.ApiKey = Configuration["ExternalProviders:SendGrid:ApiKey"];
                options.SenderEmail = Configuration["ExternalProviders:SendGrid:SenderEmail"];
                options.SenderName = Configuration["ExternalProviders:SendGrid:SenderName"];
                options.CompanyEmail = Configuration["ExternalProviders:SendGrid:CompanyEmail"];
            });

            services.AddControllersWithViews();
            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
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
            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
