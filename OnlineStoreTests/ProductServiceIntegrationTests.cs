using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnlineStore.Api.Services.DTO;
using OnlineStore.Core.Domain;
using OnlineStore.Data.DbContext;
using OnlineStore.Services;
using OnlineStore.Services.Contract;
using Moq;

namespace OnlineStoreTests;

    public class ProductServiceIntegrationTests
    {
        private readonly ApplicationDbContext _context;
        private readonly Mock<IDiscountStrategy> _mockDiscountStrategy;
        private readonly Mock<IProductDecorator> _mockProductDecorator;
        private readonly ProductService _productService;
        private readonly Mock<ILogger<ProductService>> _mockLogger;
        private readonly IMapper _mapper;

        public ProductServiceIntegrationTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("IntegrationTestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);
            _mockDiscountStrategy = new Mock<IDiscountStrategy>();
            _mockProductDecorator = new Mock<IProductDecorator>();
            _mockLogger = new Mock<ILogger<ProductService>>();

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, ProductDto>()
                    .ForMember(dest => dest.Category, opt => opt.MapFrom(src => new CategoryDto
                    {
                        Id = src.Category.Id,
                        Name = src.Category.Name,
                        Discount = src.Category.Discount
                    }));
            });
            _mapper = mockMapper.CreateMapper();

            _productService = new ProductService(
                _context,
                _mockDiscountStrategy.Object,
                _mockProductDecorator.Object,
                _mockLogger.Object,
                _mapper);
        }

        [Fact]
        public async Task GetProductsAsync_ShouldReturnPagedProductsAndApplyDiscount_WhenConditionsMet()
        {
            // Arrange
            var products = new List<Product>
            {
                new()
                { 
                    Id = 1, 
                    Price = 100, 
                    Stock = 5, 
                    Category = new Category { Name = "Mobile", Discount = 10 }
                },
                new()
                { 
                    Id = 2, 
                    Price = 200, 
                    Stock = 3, 
                    Category = new Category { Name = "Laptop", Discount = 0 }
                },
                new()
                { 
                    Id = 3, 
                    Price = 150, 
                    Stock = 0, 
                    Category = new Category { Name = "Mobile", Discount = 10 }
                },
                new()
                { 
                    Id = 4, 
                    Price = 300, 
                    Stock = 7, 
                    Category = new Category { Name = "Mobile", Discount = 5 }
                }
            };

            await _context.Products.AddRangeAsync(products);
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
                    {
                        product.Description = $"{product.Description} - Discount Applied";
                    }
                });

            // Act
            var result = await _productService.GetProductsAsync(pageIndex: 1, pageSize: 3);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Items.Count()); 
            Assert.Equal(90, result.Items.First(p => p.Id == 1).Price);
            Assert.Equal(200, result.Items.First(p => p.Id == 2).Price); 
            Assert.Equal(" - Discount Applied", result.Items.First(p => p.Id == 1).Description); 
            Assert.Equal(285, result.Items.First(p => p.Id == 4).Price); 
        }
    }

