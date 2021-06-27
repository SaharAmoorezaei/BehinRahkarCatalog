

using BehinRahkar.Catalog.Infra.Data;
using BehinRahkar.Catalog.Domain.Models;
using System.Linq;
using System.Threading.Tasks;

namespace BehinRahkar.Infra.Data
{
    public static class CatalogDbContextSeed
    {
        public static async Task SeedSampleDataAsync(CatalogDbContext context)
        {
            // Seed, if necessary
            if (context.Products.Any()) { return; }
            context.Products.Add(new Product("1234", "Product1",null, 500));
            context.Products.Add(new Product("1235", "Product1",null, 999));
            context.Products.Add(new Product("1236", "Product1",null, 2000));

            await context.SaveChangesAsync();
        }
    }
}
