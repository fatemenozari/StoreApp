using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OnlineStore.Data.DbContext;
using OnlineStore.Services;
using OnlineStore.Api.Endpoints;
using OnlineStore.Data;
using OnlineStore.Services.Contract;

namespace OnlineStore.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Online Store API", Version = "v1" });
        });

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                "Server=sqlserver,1433;Database=OnlineStore;User ID=sa;Password=YourStrongPassword123;TrustServerCertificate=true;"));

        builder.Services.AddScoped<IProductDecorator, FeaturedProductDecorator>();
        builder.Services.AddScoped<IDiscountStrategy, MobileDiscountStrategy>();
        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
        
        var app = builder.Build();

        var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.EnsureCreatedAsync();
        await SeedData.InitializeAsync(dbContext);

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Online Store API v1");
                c.RoutePrefix = string.Empty;
            });
        }

        app.MapProductEndpoints();

        app.Run();
    }
}