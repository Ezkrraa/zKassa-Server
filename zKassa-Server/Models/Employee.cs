using System.Diagnostics.CodeAnalysis;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace zKassa_Server.Models;

[PrimaryKey(nameof(Id))]
public class Employee : IdentityUser
{
    public Role Role { get; set; }
    public Guid ShopId { get; set; }

    [AllowNull]
    public virtual ICollection<ExtraPermission> ExtraPermissions { get; set; }
    [AllowNull]
    public virtual Shop Shop { get; set; }

    public Employee(string id, string email, string phoneNumber, string userName, Guid shopId, Role role)
    {
        Id = id;
        Role = role;
        Email = email;
        PhoneNumber = phoneNumber;
        UserName = userName;
        ShopId = shopId;
    }

    // empty EF Core constructor
    public Employee(string id)
    {
        Id = id;
    }

    public Employee() { }
}
