using OnlineStore.Core.Domain;
using OnlineStore.Services.Contract;

namespace OnlineStore.Services;

public class MobileDiscountStrategy : IDiscountStrategy
{
    public decimal ApplyDiscount(Product product)
    {
        return product.Price - (product.Price * (product.Category.Discount / 100.0m));
    }
}