namespace zKassa_Server.ControllerModels
{
    public class NewTransaction
    {
        public IEnumerable<NewTransactionItem> Items { get; set; }

        public NewTransaction(IEnumerable<NewTransactionItem> items)
        {
            Items = items;
        }

        public Models.Transaction ToTransaction(Guid storeId)
        {
            Guid id = Guid.NewGuid();
            return new Models.Transaction(id, storeId)
            {
                TransactionItems = Items
                    .Select(item => new Models.TransactionItem(
                        Guid.NewGuid(),
                        id,
                        item.ProductId,
                        item.PricePaid,
                        item.Quantity
                    ))
                    .ToList(),
            };
        }

        public bool IsValid()
        {
            return !Items.Any(item1 =>
                Items.Count(item2 => item1.ProductId == item2.ProductId) > 1
            );
        }
    }
}
