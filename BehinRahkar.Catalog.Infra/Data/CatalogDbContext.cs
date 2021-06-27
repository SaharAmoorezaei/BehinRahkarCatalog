

using BehinRahkar.Application.Contracts;
using BehinRahkar.Catalog.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BehinRahkar.Catalog.Infra.Data
{
    public class CatalogDbContext : DbContext, ICatalogDbContext
    {
        public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options) { }

        public virtual DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductCofiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
