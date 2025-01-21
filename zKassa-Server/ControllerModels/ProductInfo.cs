using zKassa_Server.Models;

namespace zKassa_Server.ControllerModels
{
    public record ProductInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public ProductInfo(Product product, int quantity)
        {
            Id = product.Id; 
            Name = product.Name;
            Price = product.Price;
            Quantity = quantity;
        }
    }
}
