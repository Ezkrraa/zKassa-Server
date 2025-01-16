using System.Collections.ObjectModel;
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
    public DbSet<Shop> Shops { get; set; }
    public DbSet<DistributionCenter> DistributionCenters { get; set; }
    public string DbPath { get; }

    public ZDbContext(bool createMigration = false)
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
            .WithOne(code => code.Product)
            .HasPrincipalKey(product => product.Id)
            .HasForeignKey(code => code.ProductId);
        modelBuilder.Entity<Product>()
            .HasMany(product => product.ProductStatuses)
            .WithOne(code => code.Product)
            .HasPrincipalKey(product => product.Id)
            .HasForeignKey(code => code.ProductId);

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
            .HasPrincipalKey(shop =>  shop.Id)
            .HasForeignKey(employee => employee.ShopId);

        base.OnModelCreating(modelBuilder);
    }
}
