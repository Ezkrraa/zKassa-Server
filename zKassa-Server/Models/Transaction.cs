using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace zKassa_Server.Models
{
    [PrimaryKey(nameof(Id))]
    public class Transaction
    {
        public Guid Id { get; set; }

        // todo: add payment stuffs (idc abt money I just want to make transactions)
        public Guid ShopId { get; set; }
        public DateTime SoldAt { get; set; }
        public DateTime? PaidAt { get; set; }

        [AllowNull]
        public virtual ICollection<TransactionItem> TransactionItems { get; set; }

        [AllowNull]
        public virtual Shop Shop { get; set; }

        public Transaction() { }

        public Transaction(Guid id, Guid storeId)
            : this(id, storeId, DateTime.UtcNow, null) { }

        public Transaction(Guid id, Guid storeId, DateTime soldAt, DateTime? paidAt)
        {
            Id = id;
            ShopId = storeId;
            SoldAt = soldAt;
            PaidAt = paidAt;
        }
    }
}
