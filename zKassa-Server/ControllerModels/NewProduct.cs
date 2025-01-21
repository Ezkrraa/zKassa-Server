namespace zKassa_Server.ControllerModels;

public class NewProduct
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public uint BoxAmount { get; set; }
    public ICollection<string> EanCodes { get; set; }

    public NewProduct() { }

    public Models.Product ToProduct()
    {
        Guid guid = Guid.NewGuid();
        return new Models.Product
        {
            Id = guid,
            Name = Name,
            Price = Price,
            EanCodes = EanCodes.Select(i => new Models.EanCode(guid, i)).ToList(),
            AmountInBox = BoxAmount,
        };
    }
}
