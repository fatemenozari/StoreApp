using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using OnlineStore.Api.Services.DTO;
using OnlineStore.Data.DbContext;
using OnlineStore.Services;
using OnlineStore.Core.Domain;
using OnlineStore.Services.Contract;

namespace OnlineStoreTests;

public class ProductServiceTests
{
    private readonly ApplicationDbContext _context;
    private readonly Mock<IDiscountStrategy> _mockDiscountStrategy;
    private readonly Mock<IProductDecorator> _mockProductDecorator;
    private readonly ProductService _productService;
    private readonly Mock<ILogger<ProductService>> _mockLogger;
    private readonly Mock<IMapper> _mockMapper;

    public ProductServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("TestDatabase") 
            .Options;

        _context = new ApplicationDbContext(options); 

        _mockDiscountStrategy = new Mock<IDiscountStrategy>();
        _mockProductDecorator = new Mock<IProductDecorator>();
        _mockLogger = new Mock<ILogger<ProductService>>();
        _mockMapper = new Mock<IMapper>();

        _productService = new ProductService(
            _context, 
            _mockDiscountStrategy.Object, 
            _mockProductDecorator.Object,
            _mockLogger.Object,_mockMapper.Object);  
        _mockMapper.Setup(m => m.Map<List<ProductDto>>(It.IsAny<List<Product>>()))
            .Returns((List<Product> products) =>
            {
                return products.Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Stock = p.Stock,
                    CategoryId = p.CategoryId,
                    Description = p.Description,
                    Category = new CategoryDto
                    {
                        Id = p.Category.Id,
                        Name = p.Category.Name,
                        Discount = p.Category.Discount
                    }
                }).ToList();
            });
    }

    [Fact]
    public async Task GetProductsAsync_ShouldReturnPagedProducts_WhenProductsExist()
    {
        // Arrange
        var products = new List<Product>
        {
            new() { Id = 1, Price = 100, Stock = 10, Category = new Category { Name = "Mobile" , Discount = 7.5m}},
            new() { Id = 2, Price = 200, Stock = 5, Category = new Category { Name = "Laptop", Discount = 0}},
            new() { Id = 3, Price = 150, Stock = 0, Category = new Category { Name = "Mobile" , Discount = 0}},
            new() { Id = 4, Price = 300, Stock = 3, Category = new Category { Name = "Mobile", Discount = 0 }}
        };

        _context.Products.AddRange(products);
        await _context.SaveChangesAsync();

        // Act
        var result = await _productService.GetProductsAsync(pageIndex: 1, pageSize: 2);

        // Assert
        Assert.NotNull(result); 
        Assert.NotEmpty(result.Items); 
        Assert.Equal(2, result.Items.Count());  
    }

    [Fact]
    public async Task GetProductsAsync_ShouldApplyDiscount_WhenConditionsMet()
    {
        // Arrange
        var products = new List<Product>
        {
            new() { Id = 1, Price = 100, Stock = 10, Category = new Category { Name = "Mobile" , Discount = 7.5m}},
            new() { Id = 2, Price = 200, Stock = 1, Category = new Category { Name = "Laptop", Discount = 0}}
        };

        _context.Products.AddRange(products);
        await _context.SaveChangesAsync();

        _mockDiscountStrategy.Setup(ds => ds.ApplyDiscount(It.IsAny<Product>()))
            .Callback<Product>((product) =>
            {
                product.Price -= (product.Price * (product.Category.Discount / 100m));
            });

        // Act
        var result = await _productService.GetProductsAsync();

        // Assert
        Assert.Equal(92.5m, result.Items.First(p => p.Id == 1).Price); // Mobile
        Assert.Equal(200m, result.Items.First(p => p.Id == 2).Price); // Laptop
    }

    [Fact]
    public async Task GetProductsAsync_ShouldNotApplyDiscount_WhenConditionsNotMet()
    {
        // Arrange
        var products = new List<Product>
        {
            new() { Id = 1, Price = 100, Stock = 1, Category = new Category { Name = "Mobile" , Discount = 7.5m}},
            new() { Id = 2, Price = 200, Stock = 5, Category = new Category { Name = "Laptop", Discount = 0 }}
        };

        _context.Products.AddRange(products);
        await _context.SaveChangesAsync();
        
        _mockDiscountStrategy.Setup(ds => ds.ApplyDiscount(It.IsAny<Product>()))
            .Callback<Product>((product) =>
            {
                product.Price -= (product.Price * (product.Category.Discount / 100m));
            });
        
        // Act
        var result = await _productService.GetProductsAsync();

        // Assert
        Assert.Equal(100, result.Items.First(p => p.Id == 1).Price);
        Assert.Equal(200, result.Items.First(p => p.Id == 2).Price);
    }

    [Fact]
    public async Task GetProductsAsync_ShouldDecorateProducts_WhenConditionsMet()
    {
        // Arrange
        var products = new List<Product>
        {
            new() { Id = 1, Price = 100, Stock = 10, Category = new Category { Name = "Mobile" , Discount = 7.5m}},
            new() { Id = 2, Price = 200, Stock = 5, Category = new Category { Name = "Laptop" , Discount = 0}}
        };

        _context.Products.AddRange(products);
        await _context.SaveChangesAsync();

        _mockDiscountStrategy.Setup(ds => ds.ApplyDiscount(It.IsAny<Product>()))
            .Callback<Product>((product) =>
            {
                product.Price -= (product.Price * (product.Category.Discount / 100m));
            });
        
        _mockProductDecorator.Setup(pd => pd.Decorate(It.IsAny<Product>(), It.IsAny<decimal>()))
            .Callback<Product, decimal>((product, originalPrice) =>
            {
                if (product.Price != originalPrice)
                    product.Description = $"{product.Description} - Discount Applied";
            });

        // Act
        var result = await _productService.GetProductsAsync();

        // Assert
        Assert.Equal(" - Discount Applied",  result.Items.First(p => p.Id == 1).Description);
    }

    [Fact]
    public async Task GetProductsAsync_ShouldReturnOnlyProductsInStock()
    {
        // Arrange
        var products = new List<Product>
        {
            new() { Id = 1, Price = 100, Stock = 0, Category = new Category { Name = "Mobile" , Discount = 7.5m}},
            new() { Id = 2, Price = 200, Stock = 5, Category = new Category { Name = "Laptop",Discount = 0}}
        };

        _context.Products.AddRange(products);
        await _context.SaveChangesAsync();

        // Act
        var result = await _productService.GetProductsAsync();

        // Assert
        Assert.Single(result.Items);
        Assert.Equal(200, result.Items.First().Price);
    }
}
