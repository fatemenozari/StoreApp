using OnlineStore.Api.Services.DTO;
using OnlineStore.Core.Domain;

namespace OnlineStore.Services.Contract;

public interface IProductService
{
    Task<PagedResult<ProductDto>> GetProductsAsync(int pageIndex = 1, int pageSize = 10);
}