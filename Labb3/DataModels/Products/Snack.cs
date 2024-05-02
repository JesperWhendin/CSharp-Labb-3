using Labb3ProgTemplate.Enums;

namespace Labb3ProgTemplate.DataModels.Products;

public class Snack : Product
{
    public override ProductTypes Type { get; }

    public Snack(string name, double price, string imagePath) : base(name, price, imagePath)
    {
        Name = name;
        Price = price;
        ImagePath = imagePath;
        Type = ProductTypes.Snack;
    }

    public override string ToString()
    {
        var orSnackTs = $"{Name} {Price} kr.";
        return orSnackTs;
    }
}