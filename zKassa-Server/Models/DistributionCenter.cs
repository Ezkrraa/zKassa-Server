using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace zKassa_Server.Models;

[PrimaryKey(nameof(Id))]
public class DistributionCenter
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    [AllowNull]
    public virtual ICollection<ProductStatus> ProductStatuses { get; set; }
    [AllowNull]
    public virtual ICollection<Shop> Shops { get; set; }

    public DistributionCenter(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
}
