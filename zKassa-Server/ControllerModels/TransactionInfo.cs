using zKassa_Server.Models;

namespace zKassa_Server.ControllerModels
{
    public class TransactionInfo
    {
        public Guid Id { get; set; }
        public Guid ShopId { get; set; }
        public DateTime SoldAt { get; set; }
        public DateTime? PaidAt { get; set; }
        public decimal TotalPrice { get; set; }
        public ICollection<TransactionItemInfo> Items { get; set; }

        public TransactionInfo(Transaction transaction)
        {
            Id = transaction.Id;
            ShopId = transaction.ShopId;
            SoldAt = transaction.SoldAt;
            PaidAt = transaction.PaidAt;
            TotalPrice = transaction.TransactionItems.Sum(item => item.PaidPrice);
            Items = transaction
                .TransactionItems.Select(item => new TransactionItemInfo(item))
                .ToList();
        }
    }
}
