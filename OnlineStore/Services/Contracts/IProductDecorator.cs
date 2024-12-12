using OnlineStore.Core.Domain;

namespace OnlineStore.Services.Contract;

public interface IProductDecorator
{
    void Decorate(Product product, decimal originalPrice);
}