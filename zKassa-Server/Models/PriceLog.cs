using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace zKassa_Server.Models;

// joined PK
[PrimaryKey(nameof(ProductId), nameof(TimeStamp))]
public class PriceLog {
    public Guid ProductId { get; set; }
    public decimal Price { get; set; }
    public DateTime TimeStamp { get; set; }

    [AllowNull]
    public virtual Product Product { get; set; }

    public PriceLog(Guid productId, decimal price) : this(productId, price, DateTime.UtcNow) { }

    public PriceLog(Guid productId, decimal price, DateTime timeStamp)
    {
        ProductId = productId;
        Price = price;
        TimeStamp = timeStamp;
    }
}
