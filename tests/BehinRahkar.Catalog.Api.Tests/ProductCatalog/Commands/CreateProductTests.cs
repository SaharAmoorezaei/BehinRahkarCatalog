
using Alamut.Abstractions.Structure;
using BehinRahkar.Catalog.Application.IntegrationTests.Config;
using BehinRahkar.Catalog.Application.Product.Commands;
using FluentAssertions;
using FluentValidation;
using System.Threading.Tasks;
using Xunit;

namespace BehinRahkar.Catalog.Application.Tests.ProductCatalog.Commands
{
    public class CreateProductTests: Testing<CreateProductTests>
    {
        public CreateProductTests()
        {
            ClearData();
        }

        private async Task<Result> SaveSampleProduct()
        {
            var command = new CreateProductCommand();
            command.Code = "1245";
            command.Name = "product1";
            command.Price = 2;
            return await SendAsync(command);
        }

        [Fact]
        public async Task ProductCreate_WithValidProduct_ShouldReturnSuccess()
        {
            var product = await SaveSampleProduct();
            product.Succeed.Should().BeTrue();
        }

        [Fact]
        public async Task ProductCode_IfExisted_ShouldThrowException()
        {
            var product = await SaveSampleProduct();
            FluentActions.Invoking(async () => await SaveSampleProduct()).Should().Throw<ValidationException>();
        }
    }

   
}
