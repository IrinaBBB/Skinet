﻿using System.Text.Json;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data;

public class StoreContextSeed
{
    public static async Task SeedAsync(StoreContext context, ILoggerFactory loggerFactory)
    {
        try
        {
            if (!context.ProductBrands.Any())
            {
                var brandsData
                    = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                if (brands != null)
                    foreach (var brand in brands)
                    {
                        context.ProductBrands.Add(brand);
                    }
                await context.Database.OpenConnectionAsync();
                await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT ProductBrands ON");
                await context.SaveChangesAsync();
                await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT ProductBrands OFF");
                await context.Database.CloseConnectionAsync();
            }

            if (!context.ProductTypes.Any())
            {
                var typesData
                    = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/types.json");
                var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                if (types != null)
                    foreach (var type in types)
                    {
                        context.ProductTypes.Add(type);
                    }
                await context.Database.OpenConnectionAsync();
                await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT ProductTypes ON");
                await context.SaveChangesAsync();
                await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT ProductTypes OFF");
                await context.Database.CloseConnectionAsync();
            }

            if (!context.Products.Any())
            {
                var productsData
                    = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                if (products != null)
                    foreach (var product in products)
                    {
                        context.Products.Add(product);
                    }
                await context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            var logger = loggerFactory.CreateLogger<StoreContextSeed>();
            logger.LogError(ex.Message);
        }
    }
}