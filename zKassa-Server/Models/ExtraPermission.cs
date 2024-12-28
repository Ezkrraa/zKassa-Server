using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace zKassa_Server.Models
{
    // composite primary key
    [PrimaryKey(nameof(EmployeeId), nameof(ActionID))]
    public class ExtraPermission
    {
        public string EmployeeId { get; set; }
        public int ActionID { get; set; }

        [AllowNull]
        public virtual Employee Employee { get; set; }

        public ExtraPermission(string employeeId, int actionId)
        {
            EmployeeId = employeeId;
            ActionID = actionId;
        }

        // empty constructor for EF Core
        public ExtraPermission(string employeeId)
        {
            EmployeeId = employeeId;
        }
    }
}
