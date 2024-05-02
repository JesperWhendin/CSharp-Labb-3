using Labb3ProgTemplate.Enums;

namespace Labb3ProgTemplate.DataModels.Products;

public class Excavator : Product
{
    public override ProductTypes Type { get; }

    public Excavator(string name, double price, string imagePath) : base(name, price, imagePath)
    {
        Name = name;
        Price = price;
        ImagePath = imagePath;
        Type = ProductTypes.Excavator;
    }

    public override string ToString()
    {
        var orExcavatorTs = $"{Name} {Price} kr.";
        return orExcavatorTs;
    }
}