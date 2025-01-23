using zKassa_Server.Models;

namespace zKassa_Server.ControllerModels
{
    public class EmployeeInformation
    {
        public string Id { get; set; }
        public string? UserName { get; set; }
        public Role Role { get; set; }
        public string PhoneNumber { get; set; }
        public IEnumerable<Permission> ExtraPermissionIds { get; set; }
        public Guid StoreId { get; set; }

        public EmployeeInformation(Employee employee)
        {
            Id = employee.Id;
            UserName = employee.UserName;
            Role = employee.Role;
            PhoneNumber = employee.PhoneNumber;
            ExtraPermissionIds = employee.ExtraPermissions.Select(x => x.ActionID);
            StoreId = employee.ShopId;
        }
    }
}
