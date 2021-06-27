using BehinRahkar.Catalog.Api;
using BehinRahkar.Catalog.Infra.Data;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BehinRahkar.Catalog.Application.IntegrationTests.Config
{
    public class Testing<T>: IDisposable,IClassFixture<T> where T:class 
    {
        private static IServiceScopeFactory _scopeFactory;
        private static IConfigurationRoot _configuration;
        //private static Checkpoint _checkpoint;
        public Testing()
        {
            var builder = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", true, true)
           .AddEnvironmentVariables();



            _configuration = builder.Build();

            var startup = new Startup(_configuration);

            var services = new ServiceCollection();

            services.AddSingleton(Mock.Of<IWebHostEnvironment>(w =>
                w.EnvironmentName == "Development" &&
                w.ApplicationName == "BehinRahkar.Catalog.Api"));

            services.AddLogging();
            //var serviceCollection = new ServiceCollection()
            services.AddEntityFrameworkInMemoryDatabase();
                

            services.AddDbContext<CatalogDbContext>(options =>
            {
                options.UseInMemoryDatabase(databaseName: new Guid().ToString());
            });
            startup.ConfigureServices(services);

            _scopeFactory = services.BuildServiceProvider().GetService<IServiceScopeFactory>();

            //_checkpoint = new Checkpoint
            //{
            //    TablesToIgnore = new[] { "__EFMigrationsHistory" }
            //};

            ClearData();

            //EnsureDatabase();
        }

        protected void ClearData()
        {
            using var scope = _scopeFactory.CreateScope();

            var dbContext = scope.ServiceProvider.GetService<CatalogDbContext>();
            dbContext.Products.RemoveRange(dbContext.Products.ToList());
            dbContext.SaveChanges();
        }

        private static void EnsureDatabase()
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetService<CatalogDbContext>();

            context.Database.Migrate();
        }
        public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            using var scope = _scopeFactory.CreateScope();

            var mediator = scope.ServiceProvider.GetService<ISender>();

            return await mediator.Send(request);
        }

        public void Dispose()
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetService<CatalogDbContext>();
            if(context != null)
            {
                context.Dispose();
            }
        }
    }
}
