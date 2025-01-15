namespace zKassa_Server.Models;
public class PriceLog {
    public Guid ProductId {
        get; set;
    }
    public decimal Price {
        get; set;
    }
    public DateTime TimeStamp {
        get; set;
    }

    public virtual Product Product {
        get; set;
    }

    public PriceLog(Guid productId, decimal price)
        => ProductId(productId, price, DateTime.UtcNow);
    public PriceLog(Guid productId, decimal price, DateTime timeStamp) {

    }
}
