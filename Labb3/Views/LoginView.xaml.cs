using System.Windows;
using System.Windows.Controls;
using Labb3ProgTemplate.Enums;
using Labb3ProgTemplate.Managerrs;

namespace Labb3ProgTemplate.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
            UserManager.CurrentUserChanged += UserManager_CurrentUserChanged;
        }

        private void UserManager_CurrentUserChanged()
        {
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            if (LoginName.Text == "")
            {
                MessageBox.Show("Du har inte skrivit in något användarnamn.", "Felmeddelande");
                return;
            }
            if (LoginPwd.Password == "")
            {
                MessageBox.Show("Du har inte skrivit in något lösenord.", "Felmeddelande");
                return;
            }
            string loginName = LoginName.Text;
            string loginPassword = LoginPwd.Password;
            LoginName.Text = string.Empty;
            LoginPwd.Password = string.Empty;
            UserManager.LogIn(loginName, loginPassword);
        }

        private void RegisterAdminBtn_Click(object sender, RoutedEventArgs e)
        {
            if (RegisterName.Text == "")
            {
                MessageBox.Show("Du har inte skrivit in något användarnamn.", "Felmeddelande");
                return;
            }
            if (RegisterName.Text.Contains(' '))
            {
                MessageBox.Show("Du får inte ha ett mellanslag i användarnamnet.", "Felmeddelande");
                return;
            }
            if (RegisterPwd.Password == "")
            {
                MessageBox.Show("Du har inte skrivit in något lösenord.", "Felmeddelande");
                return;
            }
            string username = RegisterName.Text;
            string password = RegisterPwd.Password;
            UserManager.SaveUsersToFile(username, password, UserTypes.Admin);
            RegisterName.Text = string.Empty;
            RegisterPwd.Password = string.Empty;
            MessageBox.Show($"Du har skapat ett nytt konto. \nNamn: {username}\nTyp: {UserTypes.Admin.ToString().ToLower()}", "Nytt konto.");
        }

        private void RegisterCustomerBtn_OnClickBtn_Click(object sender, RoutedEventArgs e)
        {
            // Ändrat från "_OnClickmerBtn_"
            if (RegisterName.Text == "")
            {
                MessageBox.Show("Du har inte skrivit in något användarnamn.", "Felmeddelande");
                return;
            }
            if (RegisterName.Text.Contains(' '))
            {
                MessageBox.Show("Du får inte ha ett mellanslag i användarnamnet.", "Felmeddelande");
                return;
            }
            if (RegisterPwd.Password == "")
            {
                MessageBox.Show("Du har inte skrivit in något lösenord.", "Felmeddelande");
                return;
            }
            string username = RegisterName.Text;
            string password = RegisterPwd.Password;
            UserManager.SaveUsersToFile(username, password, UserTypes.Customer);
            RegisterName.Text = string.Empty;
            RegisterPwd.Password = string.Empty;
            MessageBox.Show($"Du har skapat ett nytt konto. \nNamn: {username}\nTyp: {UserTypes.Customer.ToString().ToLower()}", "Nytt konto.");
        }
    }
}