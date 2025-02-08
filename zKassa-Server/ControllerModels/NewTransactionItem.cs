namespace zKassa_Server.ControllerModels
{
    public class NewTransactionItem
    {
        public Guid ProductId { get; set; }
        public decimal PricePaid { get; set; }
        public int Quantity { get; set; }
    }
}
