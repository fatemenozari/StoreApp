using OnlineStore.Core.Domain;
using OnlineStore.Services.Contract;

namespace OnlineStore.Services;

public class FeaturedProductDecorator : IProductDecorator
{
    public void Decorate(Product product, decimal originalPrice)
    {
        if (originalPrice != product.Price) 
        {
            product.Description += " - Discount Applied";
        }
    }
}