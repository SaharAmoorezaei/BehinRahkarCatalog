
using BehinRahkar.Catalog.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace BehinRahkar.Catalog.Infra.Data
{
    public class ProductCofiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(e => e.Code).IsRequired();
            builder.Property(e => e.Name).IsRequired();
            builder.Property(e => e.LastUpdated).IsRequired();
            builder.Property(e => e.RowVersion).IsRowVersion();
            builder.Property(e => e.Price).HasColumnType("decimal(18,4)");
            builder.HasIndex(e => e.Code).IsUnique();


            //builder.HasData(new Product() { Code = "1233", Name = "Product1", LastUpdated = DateTime.Now, IsConfirmed = true, Price = 500 });
            //builder.HasData(new Product() { Code = "1234", Name = "Product2", LastUpdated = DateTime.Now, IsConfirmed = true, Price = 1000 });
            //builder.HasData(new Product() { Code = "1235", Name = "Product3", LastUpdated = DateTime.Now, IsConfirmed = false, Price = 2000 });
        }
    }
}
