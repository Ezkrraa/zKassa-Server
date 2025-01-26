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

    /// <summary>
    /// The amount of the deposit (statiegeld) for an item, examples include plastic bottles and drink cans. 0.0 for other items.
    /// </summary>
    public decimal Deposit { get; set; }

    /// <summary>
    /// The amount of extra tax for single-use plastic items, in euros. 0.0 for most.
    /// </summary>
    public decimal PlasticTax { get; set; }

    /// <summary>
    /// Sales tax in a fraction. 22% tax = 0.22 for easy multiplication :)
    /// </summary>
    public decimal SalesTax { get; set; }

    public string CategoryName { get; set; }

    [AllowNull]
    public virtual Category Category { get; set; }

    [AllowNull]
    public virtual ICollection<EanCode> EanCodes { get; set; }

    [AllowNull]
    public virtual ICollection<PriceLog> PriceHistory { get; set; }

    [AllowNull]
    public virtual ICollection<ProductStatus> ProductStatuses { get; set; }

    public Product(
        Guid id,
        string name,
        decimal price,
        uint amountInBox,
        decimal deposit,
        decimal plasticTax,
        decimal salesTax,
        string categoryName
    )
    {
        Id = id;
        Name = name;
        Price = price;
        AmountInBox = amountInBox;
        Deposit = deposit;
        PlasticTax = plasticTax;
        SalesTax = salesTax;
        CategoryName = categoryName;
    }

    public Product(Guid id)
    {
        Id = id;
    }

    public Product() { }
}
