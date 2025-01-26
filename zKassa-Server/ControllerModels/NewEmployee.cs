using zKassa_Server.Models;

namespace zKassa_Server.ControllerModels
{
    public class NewEmployee
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Role Role { get; set; }
        public Guid? ShopId { get; set; }

        public Employee ToEmployee()
        {
            return new Employee(Guid.NewGuid().ToString(), Email, PhoneNumber, UserName, ShopId, Role);
        }
    }
}
