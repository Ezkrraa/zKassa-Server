using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace zKassa_Server.Models;

[PrimaryKey(nameof(ProductId), nameof(DistributionCenterId))]
public class ProductStatus
{
    public Guid ProductId { get; set; }
    public Guid DistributionCenterId { get; set; }
    public ProductStatusType Status { get; set; }

    [AllowNull]
    public virtual Product Product { get; set; }
    [AllowNull]
    public virtual DistributionCenter DistributionCenter { get; set; }

    public ProductStatus(Guid productId, Guid distributionCenterId, ProductStatusType status)
    {
        ProductId = productId;
        DistributionCenterId = distributionCenterId;
        Status = status;
    }
}
