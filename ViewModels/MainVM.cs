using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supermarket.ViewModels;
using Supermarket.Helpers;
using System.Windows.Input;
using Supermarket.Views;

namespace Supermarket.ViewModels
{
    class MainVM : BaseVM
    {
        public AdminVM adminVM { get; set; }
        public ReceiptVM receiptVM { get; set; }

        public MainVM()
        {
            LoadAdminVM();
        } 
        public void LoadAdminVM()
        {
            adminVM = new AdminVM();
        }
        public void LoadReceiptVM(string user)
        {
            receiptVM = new ReceiptVM(user);
        }

        private bool isAdminVisible = false;
        public bool IsAdminVisible
        {
            get { return isAdminVisible; }
            set
            {
                isAdminVisible = value;
                NotifyPropertyChanged("IsAdminVisible");
            }
        }

        private bool isReceiptVisible = false;
        public bool IsReceiptVisible
        {
            get { return isReceiptVisible; }
            set
            {
                isReceiptVisible = value;
                NotifyPropertyChanged("IsReceiptVisible");
            }
        }

        public ICommand SwitchToCashierCommand
        {
            get
            {
                return new RelayCommand(SwitchToCashier);
            }
        }

        private void SwitchToCashier(object obj)
        {
            LoginWindow loginWindow = new LoginWindow("cashier");
            bool? result = loginWindow.ShowDialog();
            if (result == true)
            {
                LoadReceiptVM("cashier");
                IsAdminVisible = false;
                IsReceiptVisible = true;
            }
        }

        public ICommand SwitchToAdminCommand
        {
            get
            {
                return new RelayCommand(SwitchToAdmin);
            }
        }

        private void SwitchToAdmin(object obj)
        {
            LoginWindow loginWindow = new LoginWindow("admin");
            bool? result = loginWindow.ShowDialog();
            if (result == true)
            {
                LoadAdminVM();
                IsAdminVisible = true;
                IsReceiptVisible = false;
            }
        }

    }
}
