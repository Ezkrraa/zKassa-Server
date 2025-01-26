using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace zKassa_Server.Models;

[PrimaryKey(nameof(Name))]
public class Category
{
    public string Name { get; set; }

    [AllowNull]
    public virtual ICollection<Product> Products { get; set; }

    public Category(string name)
    {
        Name = name;
    }
}
