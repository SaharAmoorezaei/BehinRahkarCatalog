
using BehinRahkar.Catalog.Domain.Models;
using FluentAssertions;
using Xunit;

namespace BehinRahkar.Catalog.Domain.Tests
{
    public class ProductTests
    {
        [Fact(DisplayName = "در صورتی که قیمت بالای 999 باشد باید تایید نشود")]
        public void ProductIsConfirm_IfPriceGreaterThan1000_ShouldBeFalse()
        {
            var sut = new Product("111", "test", null, 2000);
            sut.IsConfirmed.Should().BeFalse();
        }

        [Fact(DisplayName = "در صورتی که قیمت کمتر از 1000 باشد باید تایید شود")]
        public void ProductIsConfirm_IfPriceLessThan1000_ShouldBeTrue()
        {
            var sut = new Product("111", "test", null, 80);
            sut.IsConfirmed.Should().BeTrue();
        }

        [Fact(DisplayName = "در صورتی که قیمت کمتر 999 باشد باید تایید شود")]
        public void ProductIsConfirm_IfPriceIsEqual999_ShouldBeTrue()
        {
            var sut = new Product("111", "test", null, 999);
            sut.IsConfirmed.Should().BeTrue();
        }

    }
}
