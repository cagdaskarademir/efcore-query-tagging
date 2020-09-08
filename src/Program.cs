using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using EfCoreTagging.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EfCoreTagging
{
    class Program
    {
        static readonly IConfiguration Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true, true)
            .Build();

        static async Task Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddDbContext<DataContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("SqlServer"))
                        .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole())))
                .BuildServiceProvider();

            await InitializeDatabase(serviceProvider);

            await RunSample1(serviceProvider).ConfigureAwait(false);

            Console.WriteLine("Hello Query Tagging!");
            Console.ReadLine();
        }

        private static Task RunSample1(IServiceProvider serviceProvider)
        {
            using var serviceScope = serviceProvider.CreateScope();
            var dataContext = serviceScope.ServiceProvider.GetRequiredService<DataContext>();

            var first50Item = dataContext
                .Products
                .TagWith("Get First 50 Item")
                .OrderBy(p => p.CreateDate)
                .Take(50).ToList();

            return Task.CompletedTask;
        }

        private static async Task InitializeDatabase(ServiceProvider serviceProvider)
        {
            // wait for database ready
            while (true)
            {
                try
                {
                    // Create Database Columns
                    if (serviceProvider.GetService(typeof(DataContext)) is DataContext context)
                    {
                        await context.Database.EnsureCreatedAsync();

                        for (int i = 0; i < 100; i++)
                        {
                            var product = new Faker<Product>("en")
                                .RuleFor(p => p.Id, Guid.NewGuid)
                                .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                                .RuleFor(p => p.CreateDate, f => f.Date.Past());

                            await context.Products.AddAsync(product);
                        }

                        await context.SaveChangesAsync();
                    }

                    return;
                }
                catch(Exception e)
                {
                    Console.WriteLine($"Wait 10 sec, and try again!! - {e.Message}");
                    Thread.Sleep(10000);
                }
            }
        }
    }
}
