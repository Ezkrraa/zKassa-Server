using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace zKassa_Server.Models;

[PrimaryKey(nameof(Id))]
public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }

    [AllowNull]
    public virtual ICollection<EanCode> EanCodes { get; set; }

    public Product(Guid id, string name, decimal price)
    {
        Id = id;
        Name = name;
        Price = price;
    }

    public Product(Guid id)
    {
        Id = id;
    }

    public Product() { }
}
