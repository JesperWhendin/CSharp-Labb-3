using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Labb3ProgTemplate.DataModels.Products;
using Labb3ProgTemplate.Enums;
using Labb3ProgTemplate.Managerrs;

namespace Labb3ProgTemplate.Views
{
    /// <summary>
    /// Interaction logic for ShopView.xaml
    /// </summary>
    public partial class ShopView : UserControl
    {
        public ObservableCollection<Product> ObsColShopProducts { get; set; } = new();
        public ObservableCollection<Product> ObsColShopCart { get; set; } = new();
        public ProductTypes Type { get; set; } = new();
        public ShopView()
        {
            DataContext = this;
            foreach (var p in ProductManager.Products)
            {
                ObsColShopProducts.Add(p);
            }

            InitializeComponent();
            UserManager.CurrentUserChanged += UserManager_CurrentUserChanged;
            ProductManager.ProductListChanged += ProductManager_ProductListChanged;
            ShopFilterComboBox.ItemsSource = Enum.GetNames(typeof(ProductTypes));
        }

        private void ProductManager_ProductListChanged()
        {
            ShopFilterComboBox.SelectedItem = null;
            ObsColShopProducts.Clear();
            foreach (var p in ProductManager.Products)
            {
                ObsColShopProducts.Add(p);
            }
        }

        private void UserManager_CurrentUserChanged()
        {
            ObsColShopCart.Clear();
            UserName.Text = $"{UserManager.CurrentUser.Name}'s";
        }

        private void RemoveBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (CartList.SelectedItem is Product selectedItem)
            {
                var selectedProduct = ProductManager.Products.FirstOrDefault(p => p.Name == selectedItem.Name);
                ProductManager.RemoveProduct(selectedProduct);
                ObsColShopCart.Clear();
                foreach (var p in UserManager.CurrentUser.Cart)
                {
                    ObsColShopCart.Add(p);
                }
            }
        }

        private void AddBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ProdList.SelectedItem is Product selectedItem)
            {
                var selectedProduct = ProductManager.Products.FirstOrDefault(p => p.Name == selectedItem.Name);
                ProductManager.AddProduct(selectedProduct);
                ObsColShopCart.Clear();
                foreach (var p in UserManager.CurrentUser.Cart)
                {
                    ObsColShopCart.Add(p);
                }
            }
        }

        private void LogoutBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            UserManager.LogOut();
        }


        private void ShopResetFilterBtn_OnClick(object sender, RoutedEventArgs e)
        {
            ShopFilterComboBox.SelectedItem = null;
            ObsColShopProducts.Clear();
            foreach (var p in ProductManager.Products)
            {
                ObsColShopProducts.Add(p);
            }
        }

        private void ShopFilterComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ShopFilterComboBox.SelectedItem is null)
            {
                return;
            }
            if (ShopFilterComboBox.SelectedItem.Equals("Drink"))
            {
                var filteredProductObsCol = ProductManager.Products.OfType<Drink>().ToList();
                ObsColShopProducts.Clear();
                foreach (var p in filteredProductObsCol)
                {
                    ObsColShopProducts.Add(p);
                }
            }
            if (ShopFilterComboBox.SelectedItem.Equals("Snack"))
            {
                var filteredProductObsCol = ProductManager.Products.OfType<Snack>().ToList();
                ObsColShopProducts.Clear();
                foreach (var p in filteredProductObsCol)
                {
                    ObsColShopProducts.Add(p);
                }
            }
            if (ShopFilterComboBox.SelectedItem.Equals("Excavator"))
            {
                var filteredProductObsCol = ProductManager.Products.OfType<Excavator>().ToList();
                ObsColShopProducts.Clear();
                foreach (var p in filteredProductObsCol)
                {
                    ObsColShopProducts.Add(p);
                }
            }
        }
        private void CheckoutBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var checkoutMessage = string.Empty;
            double checkoutTotal = 0;
            checkoutMessage += $"Hej {UserManager.CurrentUser.Name}! Här kommer en sammanställning av din kundvagn.\n";
            var uniqueProducts = UserManager.CurrentUser.Cart.DistinctBy(p => p.Name);
            foreach (var uniqueProduct in uniqueProducts)
            {
                
                double count = UserManager.CurrentUser.Cart.Count(p => p.Name == uniqueProduct.Name);
                double sum = count * uniqueProduct.Price;
                double checkoutRound = Math.Round(sum, 3);
                checkoutTotal += checkoutRound;
                checkoutMessage += $"\n{count} st {uniqueProduct.Name} för {checkoutRound} kr.";
            }
            double checkoutTotalRound = Math.Round(checkoutTotal, 3);
            checkoutMessage += $"\nTotalt: {checkoutTotalRound} kr.";
            checkoutMessage += $"\n\nTack för att du handlat hos JW's Konditori och Anläggningsmaskiner!\nVälkommen åter!";
            MessageBox.Show($"{checkoutMessage}", "Check this bad boy out!");
            UserManager.CurrentUser.Cart.Clear();
            ObsColShopCart.Clear();
        }
    }
}
