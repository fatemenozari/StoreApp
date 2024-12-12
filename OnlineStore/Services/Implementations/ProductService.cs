using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Api.Services.DTO;
using OnlineStore.Core.Domain;
using OnlineStore.Data.DbContext;
using OnlineStore.Services.Contract;

namespace OnlineStore.Services;

public class ProductService(
    ApplicationDbContext context,
    IDiscountStrategy discountStrategy,
    IProductDecorator productDecorator,
    ILogger<ProductService> logger,
    IMapper mapper)
    : IProductService
{
    public async Task<PagedResult<ProductDto>> GetProductsAsync(int pageIndex = 1, int pageSize = 10)
    {
        var totalCount = await context.Products.CountAsync(p => p.Stock > 0);
        var products = await GetAvailableProductsAsync(pageIndex, pageSize);

        ApplyDiscountAndDecorate(products);

        var productDto = mapper.Map<List<ProductDto>>(products);

        return new PagedResult<ProductDto>(productDto, totalCount, pageIndex, pageSize);
    }

    private async Task<List<Product>> GetAvailableProductsAsync(int pageIndex, int pageSize)
    {
        return await context.Products
            .Where(p => p.Stock > 0)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .Include(p => p.Category)
            .ToListAsync(CancellationToken.None);
    }

    private void ApplyDiscountAndDecorate(List<Product> products)
    {
        foreach (var product in products)
        {
            var originalPrice = product.Price;

            if (product.Category.Name == "Mobile" && product.Stock >= 2 && product.Category.Discount > 0)
            {
                discountStrategy.ApplyDiscount(product);
                logger.LogInformation(
                    "Discount applied to product {ProductId}: {OriginalPrice} -> {NewPrice} ({DiscountPercentage}%).",
                    product.Id, originalPrice, product.Price, product.Category.Discount);
                productDecorator.Decorate(product, originalPrice);
            }
        }
    }
}