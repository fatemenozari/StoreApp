using Microsoft.EntityFrameworkCore;
using OnlineStore.Core.Domain;
using OnlineStore.Data.DbContext;

namespace OnlineStore.Data;

public static class SeedData
{
    public static async Task InitializeAsync(ApplicationDbContext context)
    {
        if (!await context.Categories.AnyAsync())
        {
            var categories = new List<Category>
            {
                new() { Name = "Mobile", Discount = 7.5m },
                new() { Name = "Laptop", Discount = 0 },
                new() { Name = "Fruits and Vegetables", Discount = 0 },
                new() { Name = "Accessories", Discount = 0 }
            };

            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();
        }

        if (!await context.Products.AnyAsync())
        {
            var products = new List<Product>
            {
                new() { Name = "IPhone16", CategoryId = 1 , Stock = 8 , Price = 1000000},
                new() { Name = "IPhone13", CategoryId = 1, Stock = 1, Price = 200000},
                new() { Name = "Apple", CategoryId = 3 , Stock = 300, Price = 43000},
                new() { Name = "cherry", CategoryId = 3 , Stock = 100, Price = 12800},
                new() { Name = "Sunglasses", CategoryId = 4, Stock = 20, Price = 33000},
                new() { Name = "Hat", CategoryId = 4, Stock = 0},
                new() { Name = "Asus Zenbook flip 13", CategoryId = 2, Stock = 12, Price = 3400000},
                new() { Name = "Asus Zenbook 14X", CategoryId = 2, Stock = 11, Price = 12000000}
            };

            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();
        }
    }
}