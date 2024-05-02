using System.Windows;
using Labb3ProgTemplate.Managerrs;

namespace Labb3ProgTemplate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            ProductManager.FindProductFilePath();
            ProductManager.LoadProductsFromFile();
            UserManager.FindUserFilePath();
            UserManager.LoadUsersFromFile();
            UserManager.StartUp();
            InitializeComponent();
            UserManager.CurrentUserChanged += UserManager_CurrentUserChanged;
            ProductManager.ProductListChanged += ProductManager_ProductListChanged;
            AdminTab.Visibility = Visibility.Collapsed;
            ShopTab.Visibility = Visibility.Collapsed;
        }

        private void ProductManager_ProductListChanged()
        {
        }

        private void UserManager_CurrentUserChanged()
        {
            if (UserManager.IsAdminLoggedIn)
            {
                AdminTab.Visibility = Visibility.Visible;
                ShopTab.Visibility = Visibility.Visible;
                LoginTab.Visibility = Visibility.Collapsed;
                AdminTab.IsSelected = true;
            }
            if (UserManager.IsCustomerLoggedIn)
            {
                ShopTab.Visibility = Visibility.Visible;
                AdminTab.Visibility = Visibility.Collapsed;
                LoginTab.Visibility = Visibility.Collapsed;
                ShopTab.IsSelected = true;
            }
            if (UserManager.IsGuest)
            {
                LoginTab.Visibility = Visibility.Visible;
                AdminTab.Visibility = Visibility.Collapsed;
                ShopTab.Visibility = Visibility.Collapsed;
                LoginTab.IsSelected = true;
            }
        }
    }
}