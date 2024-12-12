using Microsoft.AspNetCore.Mvc;
using OnlineStore.Api.Services.DTO;
using OnlineStore.Core.Domain;
using OnlineStore.Services.Contract;

namespace OnlineStore.Api.Endpoints;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this WebApplication app)
    {
        app.MapGet("/products", GetProducts)
            .WithName("GetProducts")
            .Produces<PagedResult<ProductDto>>()
            .Produces(StatusCodes.Status400BadRequest)
            .WithTags("Products");
    }

    private static async Task<IResult> GetProducts(
        IProductService productService, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
    {
        if (pageIndex < 1 || pageSize < 1)
        {
            return Results.BadRequest("PageIndex and PageSize must be greater than 0.");
        }

        var pagedResult = await productService.GetProductsAsync(pageIndex, pageSize);

        return Results.Ok(pagedResult);
    }
}