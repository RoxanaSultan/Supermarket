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
using Supermarket.ViewModels;

namespace Supermarket.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string user = "";
        MainVM mainVM;
        public MainWindow()
        {
            InitializeComponent();
            //AdminWindow.Visibility = Visibility.Hidden;
            //CashierWindow.Visibility = Visibility.Hidden;
            MainVM initializingVM = new MainVM();
            DataContext = initializingVM;


        }
        private void AdminButton_Click(object sender, RoutedEventArgs e)
        {
            //LoginWindow loginWindow = new LoginWindow("cde");
            //bool? result = loginWindow.ShowDialog();

            //if (result == true)
            {
                // If login is successful, you can proceed to show the admin window or do other tasks
                AdminButton.Visibility = Visibility.Hidden;
                CashierButton.Visibility = Visibility.Hidden;
               // AdminWindow.Visibility = Visibility.Visible;
                mainVM = new MainVM();
                mainVM.IsAdminVisible = true;
                mainVM.IsReceiptVisible = false;
                DataContext = mainVM;
            }

        }

        private void CashierButton_Click(object sender, RoutedEventArgs e)
        {
            //LoginWindow loginWindow = new LoginWindow("cashier");
            //bool? result = loginWindow.ShowDialog();

            //if (result == true)
            {
                // If login is successful, you can proceed to show the cashier window or do other tasks
                // Example: CashierWindow.Visibility = Visibility.Visible;
                AdminButton.Visibility = Visibility.Hidden;
                CashierButton.Visibility = Visibility.Hidden;
               // CashierWindow.Visibility = Visibility.Visible;
                //user = loginWindow.username;
                user = "livia";
                mainVM = new MainVM();
                mainVM.LoadReceiptVM(user);
                mainVM.IsAdminVisible = false;
                mainVM.IsReceiptVisible = true;
                DataContext = mainVM;

            }
        }

        //private void AdminButton_Click(object sender, RoutedEventArgs e)
        //{
        //    AdminButton.Visibility = Visibility.Hidden;
        //    CashierButton.Visibility = Visibility.Hidden;
        //    AdminWindow.Visibility = Visibility.Visible;
        //}
    }
}
