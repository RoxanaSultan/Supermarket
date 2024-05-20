using Supermarket.Helpers;
using Supermarket.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Supermarket.ViewModels
{
    class AdminVM : BaseVM
    {
        public UserVM userVM { get; set; }
        public ProducerVM producerVM { get; set; }
        public ProductVM productVM { get; set; }
        public InventoryVM inventoryVM { get; set; }
        public OfferVM offerVM { get; set; }
        public ReceiptVM receiptVM { get; set; }

        // Constructor
        public AdminVM()
        {
            userVM = new UserVM();
            producerVM = new ProducerVM();
            productVM = new ProductVM();
            inventoryVM = new InventoryVM();
            offerVM = new OfferVM();
            receiptVM = new ReceiptVM("admin");
            
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
            // Setarea vizibilității unei alte ferestre la Visible
            var cashierWindow = Application.Current.Windows.OfType<Cashier>().FirstOrDefault();
            if (cashierWindow != null)
            {
                cashierWindow.Visibility = Visibility.Visible;
            }

            // Ascunderea ferestrei curente, dacă este cazul
            var adminWindow = Application.Current.Windows.OfType<Admin>().FirstOrDefault();
            if (adminWindow != null)
            {
                adminWindow.Visibility = Visibility.Hidden;
            }
        }
    }

}
