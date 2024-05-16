using Supermarket.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Supermarket.Views
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        LoginVM loginVM;
        string _type;
        public LoginWindow(string type)
        {
            InitializeComponent();
            loginVM = new LoginVM();
            _type = type;
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;
            if(loginVM.ableToLogin(username,password, _type))
            {
                //MainWindow mainWindow = new MainWindow();
                //mainWindow.Show();
                //mainWindow.AdminWindow.Visibility = Visibility.Visible;
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password!");
            }


        }
    }
}
