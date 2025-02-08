using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace zKassa_Server.Models
{
    [PrimaryKey(nameof(TransactionId), nameof(ProductId))]
    public class TransactionItem
    {
        public Guid TransactionId { get; set; }
        public Guid ProductId { get; set; }
        public decimal PaidPrice { get; set; }

        [AllowNull]
        public virtual Transaction Transaction { get; set; }

        [AllowNull]
        public virtual Product Product { get; set; }

        public TransactionItem(Guid transactionId, Guid productId, decimal paidPrice)
        {
            TransactionId = transactionId;
            ProductId = productId;
            PaidPrice = paidPrice;
        }
    }
}
