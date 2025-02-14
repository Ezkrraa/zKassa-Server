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
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<TransactionItem> TransactionItems { get; set; }
    public string DbPath { get; }

    public ZDbContext()
    {
        Environment.SpecialFolder folder = Environment.SpecialFolder.LocalApplicationData;
        string path = Path.Join(Environment.GetFolderPath(folder), "zKassa");
        DbPath = Path.Join(path, "Database.db");
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
        modelBuilder
            .Entity<Shop>()
            .HasMany(shop => shop.Transactions)
            .WithOne(transaction => transaction.Shop)
            .HasPrincipalKey(shop => shop.Id)
            .HasForeignKey(transaction => transaction.ShopId);

        modelBuilder
            .Entity<Transaction>()
            .HasMany(transaction => transaction.TransactionItems)
            .WithOne(item => item.Transaction)
            .HasPrincipalKey(transaction => transaction.Id)
            .HasForeignKey(item => item.TransactionId);

        modelBuilder
            .Entity<TransactionItem>()
            .HasOne(item => item.Product)
            .WithMany(product => product.TransactionItems)
            .HasPrincipalKey(product => product.Id)
            .HasForeignKey(item => item.ProductId);

        base.OnModelCreating(modelBuilder);
    }
}
