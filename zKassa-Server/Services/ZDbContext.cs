using System.Collections.ObjectModel;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using zKassa_Server.Models;

namespace zKassa_Server.Services;

public class ZDbContext : IdentityDbContext<Employee>
{
    public override DbSet<Employee> Users { get; set; }
    public DbSet<ExtraPermission> CustomPermissions { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<EanCode> EanCodes { get; set; }
    public DbSet<ProductStatus> ProductStatuses { get; set; }
    public DbSet<PriceLog> PriceLogs { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Shop> Shops { get; set; }
    public DbSet<DistributionCenter> DistributionCenters { get; set; }
    public string DbPath { get; }

    public ZDbContext(bool createMigration = false, bool seedDatabase = false)
    {
        Environment.SpecialFolder folder = Environment.SpecialFolder.LocalApplicationData;
        string path = Path.Join(Environment.GetFolderPath(folder), "zKassa");
        DbPath = Path.Join(path, "Database.db");
        if (createMigration)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            if (!File.Exists(DbPath))
                base.Database.Migrate();
        }
        else if (seedDatabase)
        {
            using (ZDbContext dbContext = new())
            {
                dbContext.Database.Migrate();

                Guid distCenterId = Guid.NewGuid();
                dbContext.DistributionCenters.Add(new(distCenterId, "Zuid-Holland"));
                Guid shopId = Guid.NewGuid();
                dbContext.Shops.Add(new(shopId, "Hellevoetsluis", distCenterId));
                Category Fruit = new("Fruit & Veg");
                Category Drinks = new("Drinks");
                dbContext.Categories.Add(Fruit);
                dbContext.Categories.Add(Drinks);

                Guid guid1 = Guid.NewGuid();
                dbContext.Products.Add(new(guid1, "Tomato", 0.15m, 12, 0m, 0m, 0.12m, Fruit.Name));
                dbContext.EanCodes.Add(new(guid1, "0"));
                dbContext.PriceLogs.Add(new(guid1, 0.15m, DateTime.UtcNow));

                Guid guid2 = Guid.NewGuid();
                dbContext.Products.Add(new(guid2, "Potato", 0.15m, 12, 0m, 0m, 0.12m, Fruit.Name));
                dbContext.EanCodes.Add(new(guid2, "01"));
                dbContext.PriceLogs.Add(new(guid2, 0.15m, DateTime.UtcNow));

                Guid guid3 = Guid.NewGuid();
                dbContext.Products.Add(
                    new(guid3, "Cucumber", 0.15m, 12, 0m, 0m, 0.12m, Fruit.Name)
                );
                dbContext.EanCodes.Add(new(guid3, "012"));
                dbContext.PriceLogs.Add(new(guid3, 0.15m, DateTime.UtcNow));

                Guid guid4 = Guid.NewGuid();
                dbContext.Products.Add(
                    new(guid4, "Monster Energy", 0.99m, 24, 0.15m, 0m, 0.24m, Drinks.Name)
                );
                dbContext.EanCodes.Add(new(guid4, "0123"));
                dbContext.PriceLogs.Add(new(guid4, 0.99m, DateTime.UtcNow));

                Guid guid5 = Guid.NewGuid();
                dbContext.Products.Add(
                    new(guid5, "Fake Monster", 0.69m, 12, 0.15m, 0.01m, 0.24m, Drinks.Name)
                );
                dbContext.EanCodes.Add(new(guid5, "01234"));
                dbContext.PriceLogs.Add(new(guid5, 0.69m, DateTime.UtcNow));

                dbContext.SaveChanges();
            }
            return;
        }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={DbPath}").UseLazyLoadingProxies();
        //base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Employee>()
            .HasMany(employee => employee.ExtraPermissions)
            .WithOne(permission => permission.Employee)
            .HasPrincipalKey(nameof(Employee.Id))
            .HasForeignKey(nameof(ExtraPermission.EmployeeId));

        modelBuilder
            .Entity<Product>()
            .HasMany(product => product.EanCodes)
            .WithOne(code => code.Product)
            .HasPrincipalKey(product => product.Id)
            .HasForeignKey(code => code.ProductId);
        modelBuilder
            .Entity<Product>()
            .HasMany(product => product.PriceHistory)
            .WithOne(log => log.Product)
            .HasPrincipalKey(product => product.Id)
            .HasForeignKey(log => log.ProductId);
        modelBuilder
            .Entity<Product>()
            .HasMany(product => product.ProductStatuses)
            .WithOne(code => code.Product)
            .HasPrincipalKey(product => product.Id)
            .HasForeignKey(code => code.ProductId);
        modelBuilder
            .Entity<Product>()
            .HasOne(product => product.Category)
            .WithMany(category => category.Products)
            .HasPrincipalKey(category => category.Name)
            .HasForeignKey(product => product.CategoryName);

        modelBuilder
            .Entity<DistributionCenter>()
            .HasMany(center => center.Shops)
            .WithOne(shop => shop.DistributionCenter)
            .HasPrincipalKey(center => center.Id)
            .HasForeignKey(shop => shop.DistributionId);
        modelBuilder
            .Entity<DistributionCenter>()
            .HasMany(center => center.ProductStatuses)
            .WithOne(status => status.DistributionCenter)
            .HasPrincipalKey(center => center.Id)
            .HasForeignKey(status => status.DistributionCenterId);

        modelBuilder
            .Entity<Shop>()
            .HasMany(shop => shop.Employees)
            .WithOne(employee => employee.Shop)
            .HasPrincipalKey(shop => shop.Id)
            .HasForeignKey(employee => employee.ShopId);

        base.OnModelCreating(modelBuilder);
    }
}
