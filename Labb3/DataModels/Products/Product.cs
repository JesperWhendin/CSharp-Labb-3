using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Labb3ProgTemplate.Enums;

namespace Labb3ProgTemplate.DataModels.Products;

public abstract class Product : INotifyPropertyChanged
{
    private string _name;
    public string Name
    {
        get { return _name; }
        set
        {
            _name = value;
            OnPropertyChanged();
        }
    }

    private double _price;

    public double Price
    {
        get { return _price; }
        set
        {
            _price = value;
            OnPropertyChanged();
        }
    }

    public abstract ProductTypes Type { get; }
    
    public string ImagePath { get; set; }

    protected Product(string name, double price, string imagePath)
    {
        Name = name;
        Price = price;
        ImagePath = imagePath;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}