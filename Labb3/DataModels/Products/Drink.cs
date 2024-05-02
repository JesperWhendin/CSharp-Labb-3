using Labb3ProgTemplate.Enums;

namespace Labb3ProgTemplate.DataModels.Products;

public class Drink : Product
{
    public override ProductTypes Type { get; }

    public Drink(string name, double price, string imagePath) : base(name, price, imagePath)
    {
        Name = name;
        Price = price;
        ImagePath = imagePath;
        Type = ProductTypes.Drink;
    }

    public override string ToString()
    {
        var orDrinkTs = $"{Name} {Price} kr.";
        return orDrinkTs;
    }
}