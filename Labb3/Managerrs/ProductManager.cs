using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Labb3ProgTemplate.DataModels.Products;
using Labb3ProgTemplate.Enums;


namespace Labb3ProgTemplate.Managerrs;

public static class ProductManager
{
    private static readonly IEnumerable<Product>? _products = new List<Product>();
    public static IEnumerable<Product>? Products => _products;

    public static string ProductFilePath = string.Empty;

    // Skicka detta efter att produktlistan ändrats eller lästs in
    public static event Action ProductListChanged;


    static ProductManager()
    {

    }

    public static void FindProductFilePath()
    {
        var dirPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Labb 3 JW");
        Directory.CreateDirectory(dirPath);
        ProductFilePath = Path.Combine(dirPath, "Products.json");
    }

    public static void AddProduct(Product product)
    {
        UserManager.CurrentUser.Cart.Add(product);
    }

    public static void RemoveProduct(Product product)
    {
        UserManager.CurrentUser.Cart.Remove(product);
    }

    public static async Task SaveProductsToFile(string name, double price, ProductTypes type, string imgPath)
    {
        if (!_products.Any(p => p.Name == name))
        {
            if (type == ProductTypes.Drink)
            {
                var newDrink = new Drink(name, price, imgPath);
                ((List<Product>)_products).Add(newDrink);
            }
            if (type == ProductTypes.Snack)
            {
                var newSnack = new Snack(name, price, imgPath);
                ((List<Product>)_products).Add(newSnack);
            }
            if (type == ProductTypes.Excavator)
            {
                var newExcavator = new Excavator(name, price, imgPath);
                ((List<Product>)_products).Add(newExcavator);
            }
        }

        using var streamWriter = new StreamWriter(ProductFilePath);
        var json = JsonSerializer.Serialize(((List<Product>)_products), new JsonSerializerOptions() { WriteIndented = true });
        streamWriter.WriteLine(json);
        ProductListChanged?.Invoke();
    }

    public static async Task LoadProductsFromFile()
    {
        if (!File.Exists(ProductFilePath))
        {
            ((List<Product>)_products).AddRange(new Product[]
            {
                new Drink (      "Vatten",          7,       "../Images/vatten-image.png"          ),
                new Drink (      "Cola",            13,      "../Images/cola-image.png"            ),
                new Drink (      "Nocco",           18,      "../Images/nocco-image.png"           ),
                new Drink (      "Kaffe",           20,      "../Images/kaffe-image.png"           ),
                new Snack (      "Skotte",          7.95,    "../Images/skotte-image.png"          ),
                new Snack (      "Delicato 6p",     19.95,   "../Images/delicato-6p-image.png"     ),
                new Snack (      "Gott&Blandat",    18.90,   "../Images/gott-blandat-image.png"    ),
                new Excavator (  "Volvo EC60E",     1700000, "../Images/volvo-ec60e-image.png"     ),
                new Excavator (  "Komatsu PC170LC", 2600000, "../Images/komatsu-pc170lc-image.png" )
            });
            using var streamWriter = new StreamWriter(ProductFilePath);
            var json = JsonSerializer.Serialize(((List<Product>)_products), new JsonSerializerOptions() { WriteIndented = true });
            streamWriter.WriteLine(json);
        }

        var jsonText = string.Empty;
        string? jsonLine = string.Empty;
        using StreamReader streamReader = new (ProductFilePath);
        while ((jsonLine = streamReader.ReadLine()) != null)
        {
            jsonText += jsonLine;
        }
        var jsonDesProdList = new List<Product>();
        using (var jsonDocument = JsonDocument.Parse(jsonText))
        {
            if (jsonDocument.RootElement.ValueKind == JsonValueKind.Array)
            {
                foreach (var jsonElement in jsonDocument.RootElement.EnumerateArray())
                {
                    Product p;
                    switch (jsonElement.GetProperty("Type").GetInt32())
                    {
                        case 0:
                            p = jsonElement.Deserialize<Drink>();
                            jsonDesProdList.Add(p);
                            break;
                        case 1:
                            p = jsonElement.Deserialize<Snack>();
                            jsonDesProdList.Add(p);
                            break;
                        case 2:
                            p = jsonElement.Deserialize<Excavator>();
                            jsonDesProdList.Add(p);
                            break;
                    }
                }
            }
        }
        ((List<Product>)_products).Clear();
        foreach (var p in jsonDesProdList)
        {
            ((List<Product>)_products).Add(p);
        }
        ProductListChanged?.Invoke();
    }

    public static async Task RemoveProductFromFile(Product product)
    {
        ((List<Product>)_products).Remove(product);
        using var streamWriter = new StreamWriter(ProductFilePath);
        var json = JsonSerializer.Serialize(((List<Product>)_products), new JsonSerializerOptions() { WriteIndented = true });
        streamWriter.WriteLine(json);
        ProductListChanged?.Invoke();
    }
}