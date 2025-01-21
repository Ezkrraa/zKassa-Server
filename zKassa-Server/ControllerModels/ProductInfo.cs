using zKassa_Server.Models;

namespace zKassa_Server.ControllerModels
{
    public record ProductInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public ProductInfo(Product product)
        {
            Id = product.Id; 
            Name = product.Name;
            Price = product.Price;
        }
    }
}
