using zKassa_Server.Models;

namespace zKassa_Server.ControllerModels;

public class DistCenterInfo
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DistCenterInfo(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
    public DistCenterInfo(DistributionCenter center)
    {
        Id = center.Id;
        Name = center.Name;
    }
}
