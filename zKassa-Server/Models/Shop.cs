using System.Diagnostics.CodeAnalysis;

namespace zKassa_Server.Models;

public class Shop
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid DistributionId { get; set; }

    [AllowNull]
    public virtual DistributionCenter DistributionCenter { get; set; }

    [AllowNull]
    public virtual ICollection<Employee>? Employees { get; set; }

    [AllowNull]
    public virtual ICollection<Transaction>? Transactions { get; set; }

    public Shop(Guid id, string name, Guid distributionId)
    {
        Id = id;
        Name = name;
        DistributionId = distributionId;
    }
}
