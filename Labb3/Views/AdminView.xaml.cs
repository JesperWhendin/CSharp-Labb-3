using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Labb3ProgTemplate.DataModels.Products;
using Labb3ProgTemplate.Enums;
using Labb3ProgTemplate.Managerrs;
using Microsoft.Win32;

namespace Labb3ProgTemplate.Views
{
    /// <summary>
    /// Interaction logic for AdminView.xaml
    /// </summary>
    public partial class AdminView : UserControl
    {
        public ObservableCollection<Product> ObsColAdminProducts { get; set; } = new();

        public ProductTypes Type { get; set; } = new();

        public Product SelectedItem { get; set; }

        public AdminView()
        {
            DataContext = this;
            foreach (var p in ProductManager.Products)
            {
                ObsColAdminProducts.Add(p);
            }

            InitializeComponent();
            UserManager.CurrentUserChanged += UserManager_CurrentUserChanged;
            ProductManager.ProductListChanged += ProductManager_ProductListChanged;
            TypeComboBox.ItemsSource = Enum.GetNames(typeof(ProductTypes));
            AdminFilterComboBox.ItemsSource = Enum.GetNames(typeof(ProductTypes));
        }

        private void ProductManager_ProductListChanged()
        {
            AdminFilterComboBox.SelectedItem = null;
            ObsColAdminProducts.Clear();
            foreach (var p in ProductManager.Products)
            {
                ObsColAdminProducts.Add(p);
            }
        }

        private void UserManager_CurrentUserChanged()
        {

        }

        private void ProdList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProdList.SelectedItem is Product product)
            {
                ProductNameTextBox.Text = product.Name;
                ProductPriceTextBox.Text = product.Price.ToString();
                TypeComboBox.Text = product.Type.ToString();
                Image previewImage = new();
                BitmapImage bi = new();
                bi.BeginInit();
                bi.UriSource = new Uri(SelectedItem.ImagePath, UriKind.Relative);
                bi.EndInit();
                previewImage.Source = bi;
                AdminProductImage.Source = bi;
            }
            if (ProdList.SelectedItem is null)
            {
                ProductNameTextBox.Text = string.Empty;
                ProductPriceTextBox.Text = string.Empty;
                TypeComboBox.Text = null;
                AdminProductImage.Source = null;
            }
        }

        private void SaveBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ProductNameTextBox.Text == "")
            {
                return;
            }
            if (ProductPriceTextBox.Text == "")
            {
                return;
            }
            if (ProdList.SelectedItem is null && ProductManager.Products.Any(p => p.Name == ProductNameTextBox.Text))
            {
                MessageBox.Show("Finns redan en produkt med detta namn. \nMarkera produkten i den vänstra listan för att uppdatera den.", "Felmeddelande");
            }

            if (ProdList.SelectedItem is Product selectedItem)
            {
                var selectedProduct = ProductManager.Products.FirstOrDefault(p => p.Name == selectedItem.Name);
                selectedProduct.Name = ProductNameTextBox.Text;
                selectedProduct.Price = double.Parse(ProductPriceTextBox.Text);
                ProductManager.SaveProductsToFile(selectedProduct.Name, selectedProduct.Price, selectedProduct.Type, "../Images/product-image.png");
            }
            else if (ProductNameTextBox.Text != "" || ProductPriceTextBox.Text != "")
            {
                string prodName = ProductNameTextBox.Text;
                string prodPrice = ProductPriceTextBox.Text;
                var prodPriceReplace = prodPrice.Replace('.', ',');
                if (!double.TryParse(prodPriceReplace, out var parsedPrice))
                {
                    return;
                }
                switch (TypeComboBox.SelectedItem)
                {
                    case nameof(Drink):
                        ProductManager.SaveProductsToFile(prodName, parsedPrice, ProductTypes.Drink, "../Images/product-image.png");
                        break;
                    case nameof(Snack):
                        ProductManager.SaveProductsToFile(prodName, parsedPrice, ProductTypes.Snack, "../Images/product-image.png");
                        break;
                    case nameof(Excavator):
                        ProductManager.SaveProductsToFile(prodName, parsedPrice, ProductTypes.Excavator, "../Images/product-image.png");
                        break;
                }
                TypeComboBox.SelectedItem = null;
            }
        }

        private void RemoveBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ProdList.SelectedItem is Product selectedProduct)
            {
                ProductManager.RemoveProductFromFile(selectedProduct);
            }
        }

        private void LogoutBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            UserManager.LogOut();
        }

        private void AdminResetFilterBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            AdminFilterComboBox.SelectedItem = null;
            ObsColAdminProducts.Clear();
            foreach (var p in ProductManager.Products)
            {
                ObsColAdminProducts.Add(p);
            }
        }

        private void AdminFilterComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AdminFilterComboBox.SelectedItem is null)
            {
                return;
            }
            if (AdminFilterComboBox.SelectedItem.Equals("Drink"))
            {
                var filteredProductObsCol = ProductManager.Products.OfType<Drink>().ToList();
                ObsColAdminProducts.Clear();
                foreach (var p in filteredProductObsCol)
                {
                    ObsColAdminProducts.Add(p);
                }
            }
            if (AdminFilterComboBox.SelectedItem.Equals("Snack"))
            {
                var filteredProductObsCol = ProductManager.Products.OfType<Snack>().ToList();
                ObsColAdminProducts.Clear();
                foreach (var p in filteredProductObsCol)
                {
                    ObsColAdminProducts.Add(p);
                }
            }
            if (AdminFilterComboBox.SelectedItem.Equals("Excavator"))
            {
                var filteredProductObsCol = ProductManager.Products.OfType<Excavator>().ToList();
                ObsColAdminProducts.Clear();
                foreach (var p in filteredProductObsCol)
                {
                    ObsColAdminProducts.Add(p);
                }
            }
        }

        private void SetImageBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (ProdList.SelectedItem is Product selectedItem)
            {
                var selectedProduct = ProductManager.Products.FirstOrDefault(p => p.Name == selectedItem.Name);
                var dlg = new OpenFileDialog();
                dlg.DefaultExt = ".png";
                dlg.Filter = "PNG Files (*.png)|*.png|JPEG Files (*.jpeg)|*.jpeg|JPG Files (*.jpg)|*.jpg";
                dlg.ShowDialog();
                selectedProduct.ImagePath = $"../Images/{dlg.SafeFileName}";
                ProductManager.SaveProductsToFile(selectedProduct.Name, selectedProduct.Price, selectedProduct.Type, selectedProduct.ImagePath);
            }
        }

        private void ClearImageBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (ProdList.SelectedItem is Product selectedItem)
            {
                var selectedProduct = ProductManager.Products.FirstOrDefault(p => p.Name == selectedItem.Name);
                selectedProduct.ImagePath = "../Images/product-image.png";
                ProductManager.SaveProductsToFile(selectedProduct.Name, selectedProduct.Price, selectedProduct.Type, selectedProduct.ImagePath);
            }
        }
    }
}