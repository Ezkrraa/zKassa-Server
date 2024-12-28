using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace zKassa_Server.Models;

[PrimaryKey(nameof(ProductId), nameof(EAN))]
public class EanCode
{
    public Guid ProductId { get; set; }
    public string EAN { get; set; }

    [AllowNull]
    public virtual Product Product { get; set; }

    public EanCode(Guid productId, string ean)
    {
        ProductId = productId;
        EAN = ean;
    }

    // empty constructor for EF Core
    public EanCode(Guid productId)
    {
        ProductId = productId;
    }

    public EanCode() { }
}
