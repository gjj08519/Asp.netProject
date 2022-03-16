using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PizzaWebsite.Data.Seeder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaWebsite
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            await RunSeeding(host);

            host.Run();
        }

        private static async Task RunSeeding(IHost host)
        {
            var scopeFactory = host.Services.GetService<IServiceScopeFactory>();

            using var scope = scopeFactory.CreateScope();

            var services = scope.ServiceProvider;
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();

            try
            {
                var userIdentityDataSeeder = services.GetService<UserIdentityDataSeeder>();
                await userIdentityDataSeeder.SeedAsync();

                var pizzaWebsiteDataSeeder = services.GetService<PizzaWebsiteDataSeeder>();
                pizzaWebsiteDataSeeder.Seed();
            }
            catch (Exception e)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(e, "An error occurred seeding the database.");
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}