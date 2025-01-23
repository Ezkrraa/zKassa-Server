using zKassa_Server.Models;

namespace zKassa_Server.ControllerModels
{
    public record ProductInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal Deposit { get; set; }
        public decimal PlasticTax { get; set; }
        public decimal SalesTax { get; set; }
        public ProductStatusType ProductStatus { get; set; }

        public ProductInfo(Product product, ProductStatusType? status)
        {
            Id = product.Id;
            Name = product.Name;
            Price = product.Price;
            Deposit = product.Deposit;
            PlasticTax = product.PlasticTax;
            SalesTax = product.SalesTax;
            ProductStatus = status ?? ProductStatusType.Passive;
        }
    }
}
