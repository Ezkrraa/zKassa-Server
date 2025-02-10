using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace zKassa_Server.Models
{
    [PrimaryKey(nameof(Id))]
    public class TransactionItem
    {
        public Guid Id { get; set; }
        public Guid TransactionId { get; set; }
        public Guid ProductId { get; set; }
        public decimal PaidPrice { get; set; }
        public int Quantity { get; set; }

        [AllowNull]
        public virtual Transaction Transaction { get; set; }

        [AllowNull]
        public virtual Product Product { get; set; }

        public TransactionItem(
            Guid id,
            Guid transactionId,
            Guid productId,
            decimal paidPrice,
            int quantity
        )
        {
            Id = id;
            TransactionId = transactionId;
            ProductId = productId;
            PaidPrice = paidPrice;
            Quantity = quantity;
        }
    }
}
