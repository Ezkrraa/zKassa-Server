namespace zKassa_Server.ControllerModels;

public class NewProduct
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public uint BoxAmount { get; set; }
    public ICollection<string> EanCodes { get; set; }

    public NewProduct() { }

    public Models.Product ToProduct()
    {
        return new Models.Product
        {
            Id = Id,
            Name = Name,
            Price = Price,
            EanCodes = EanCodes.Select(i => new Models.EanCode(Id, i)).ToList(),
        };
    }
}
