using zKassa_Server.Models;

namespace zKassa_Server.ControllerModels
{
    public class TransactionItemInfo
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public decimal PaidPrice { get; set; }
        public int Quantity { get; set; }

        public TransactionItemInfo(TransactionItem transactionItem)
        {
            Id = transactionItem.Id;
            ProductId = transactionItem.ProductId;
            PaidPrice = transactionItem.PaidPrice;
            Quantity = transactionItem.Quantity;
        }
    }
}
