using Microsoft.EntityFrameworkCore;
using System;
using zKassa_Server.Models;
using zKassa_Server.Services;

namespace zKassa_Server
{
    public static class Seeder
    {
        public static void CreateDatabase()
        {
            ZDbContext dbContext = new ZDbContext();
            dbContext.Database.Migrate();
        }

#if DEBUG
        public static void SeedDatabase()
        {
            ZDbContext dbContext = new();

            dbContext.Database.Migrate();

            DistributionCenter distCenter = new(Guid.NewGuid(), "Zuid-Holland");
            Shop shop = new(Guid.NewGuid(), "Hellevoetsluis", distCenter.Id);

            dbContext.DistributionCenters.Add(distCenter);
            dbContext.Shops.Add(shop);

            Category Fruit = new("Fruit & Veg");
            Category Drinks = new("Drinks");
            dbContext.Categories.Add(Fruit);
            dbContext.Categories.Add(Drinks);

            List<Product> products =
            [
                new(Guid.NewGuid(), "Tomato", 0.15m, 12, 0m, 0m, 0.12m, Fruit.Name),
                new(Guid.NewGuid(), "Potato", 0.15m, 12, 0m, 0m, 0.12m, Fruit.Name),
                new(Guid.NewGuid(), "Cucumber", 0.15m, 12, 0m, 0m, 0.12m, Fruit.Name),
                new(Guid.NewGuid(), "Raw Salad", 0.69m, 12, 0m, 0.15m, 0.12m, Drinks.Name),
                new(Guid.NewGuid(), "Monster Energy", 0.99m, 24, 0.15m, 0m, 0.24m, Drinks.Name),
                new(Guid.NewGuid(), "Fake Monster", 0.69m, 12, 0.15m, 0.01m, 0.24m, Drinks.Name),
                new(Guid.NewGuid(), "Power drink 1x6", 3.59m, 4, 0.90m, 0.00m, 0.24m, Drinks.Name),
                new(Guid.NewGuid(), "Fake Monster", 0.69m, 12, 0.15m, 0.00m, 0.24m, Drinks.Name),
            ];
            for (int i = 0; i < products.Count; i++)
            {
                dbContext.Products.Add(products[i]);
                dbContext.EanCodes.Add(new(products[i].Id, i.ToString()));
                dbContext.PriceLogs.Add(new(products[i].Id, products[i].Price));
            }
            dbContext.SaveChanges();
        }
    }
#endif
}
