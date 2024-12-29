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
    public string DbPath { get; }

    public ZDbContext()
    {
        Environment.SpecialFolder folder = Environment.SpecialFolder.LocalApplicationData;
        string path = Path.Join(Environment.GetFolderPath(folder), "zKassa");
        DbPath = Path.Join(path, "Database.db");
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        if (!File.Exists(DbPath))
            base.Database.Migrate();
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

        base.OnModelCreating(modelBuilder);
    }
}
