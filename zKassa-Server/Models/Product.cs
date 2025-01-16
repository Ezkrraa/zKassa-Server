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
    public uint AmountInBox { get; set; }

    [AllowNull]
    public virtual ICollection<EanCode> EanCodes { get; set; }
    [AllowNull]
    public virtual ICollection<PriceLog> PriceHistory { get; set; }
    [AllowNull]
    public virtual ICollection<ProductStatus> ProductStatuses { get; set; }

    public Product(Guid id, string name, decimal price, uint amountInBox)
    {
        Id = id;
        Name = name;
        Price = price;
        AmountInBox = amountInBox;
    }

    public Product() { }
}
