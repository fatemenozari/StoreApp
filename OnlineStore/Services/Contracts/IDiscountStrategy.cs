using OnlineStore.Core.Domain;

namespace OnlineStore.Services.Contract;


public interface IDiscountStrategy
{
    decimal ApplyDiscount(Product product);
}