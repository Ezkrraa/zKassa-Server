using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace zKassa_Server.Models;

[PrimaryKey(nameof(ProductId), nameof(DistributionCenterId), nameof(TimeStamp))]
public class ProductStatus
{
    public Guid ProductId { get; set; }
    public Guid DistributionCenterId { get; set; }
    public ProductStatusType Status { get; set; }
    public DateTime TimeStamp { get; set; }

    [AllowNull]
    public virtual Product Product { get; set; }

    [AllowNull]
    public virtual DistributionCenter DistributionCenter { get; set; }

    public ProductStatus(
        Guid productId,
        Guid distributionCenterId,
        ProductStatusType status,
        DateTime timeStamp
    )
    {
        ProductId = productId;
        DistributionCenterId = distributionCenterId;
        Status = status;
        TimeStamp = timeStamp;
    }
}
