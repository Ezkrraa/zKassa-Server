using zKassa_Server.Models;

namespace zKassa_Server.ControllerModels
{
    public record ExpandedProductInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal Deposit { get; set; }
        public decimal PlasticTax { get; set; }
        public decimal SalesTax { get; set; }
        public string CategoryName { get; set; }
        public IEnumerable<string> Eans { get; set; }

        public ExpandedProductInfo(Product product)
        {
            Id = product.Id;
            Name = product.Name;
            Price = product.Price;
            Deposit = product.Deposit;
            PlasticTax = product.PlasticTax;
            SalesTax = product.SalesTax;
            CategoryName = product.CategoryName;
            Eans = product.EanCodes.Select(code => code.EAN);
        }
    }
}
