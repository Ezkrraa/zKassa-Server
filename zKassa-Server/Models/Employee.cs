using System.Diagnostics.CodeAnalysis;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace zKassa_Server.Models;

[PrimaryKey(nameof(Id))]
public class Employee : IdentityUser
{
    public int Role { get; set; }

    [AllowNull]
    public virtual ICollection<ExtraPermission> ExtraPermissions { get; set; }

    public Employee(string id, int role)
    {
        Id = id;
        Role = 0;
    }

    // empty EF Core constructor
    public Employee(string id)
    {
        Id = id;
    }

    public Employee() { }
}
