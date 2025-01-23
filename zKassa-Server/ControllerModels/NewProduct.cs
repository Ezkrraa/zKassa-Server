namespace zKassa_Server.ControllerModels;

public class NewProduct
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public decimal Deposit { get; set; }
    public decimal PlasticTax { get; set; }
    public decimal SalesTax { get; set; }
    public uint BoxAmount { get; set; }
    public ICollection<string> EanCodes { get; set; }

    public NewProduct() { }

    public Models.Product ToProduct()
    {
        Guid guid = Guid.NewGuid();
        return new Models.Product(guid, Name, Price, BoxAmount, Deposit, PlasticTax, SalesTax);
    }
}
