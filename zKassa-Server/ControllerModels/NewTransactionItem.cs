namespace zKassa_Server.ControllerModels
{
    public class NewTransactionItem
    {
        public Guid ProductId { get; set; }
        public decimal PricePaid { get; set; }
        public int Quantity { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is NewTransactionItem other)
            {
                return other.ProductId == this.ProductId;
            }
            return false;
        }
    }
}
