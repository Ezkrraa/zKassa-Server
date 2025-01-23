using zKassa_Server.Models;

namespace zKassa_Server.ControllerModels;

public class UpdatedProductStatus
{
    public Guid ProductId { get; set; }
    public Guid DistributionCenterId { get; set; }
    public ProductStatusType Status { get; set; }

    public UpdatedProductStatus(Guid productId, Guid distributionCenterId, ProductStatusType status)
    {
        ProductId = productId;
        DistributionCenterId = distributionCenterId;
        Status = status;
    }
}
