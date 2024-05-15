using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supermarket.Helpers;
using Supermarket.Models.BusinessLogicLayer;
using Supermarket.Models;
using Supermarket.Views;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace Supermarket.ViewModels
{
    class ReceiptVM : BaseVM
    {
        private ReceiptBLL receiptBLL;
        private ObservableCollection<Receipt> receipts;
        public ObservableCollection<Receipt> Receipts
        {
            get { return receipts; }
            private set
            {
                receipts = value;
                NotifyPropertyChanged(nameof(Receipts));
            }
        }
        public ReceiptVM()
        {
            receiptBLL = new ReceiptBLL();
            receipts = new ObservableCollection<Receipt>(receiptBLL.GetReceipts());
        }

        public void AddReceipt(object obj)
        {
            Receipt receipt = obj as Receipt;
            if (receipt == null)
            {
                receiptBLL.ErrorMessage = "Invalid input!";
                return;
            }
            if (!IsValidReceipt(receipt))
            {
                receiptBLL.ErrorMessage = "Validation failed!";
                return;
            }
            receiptBLL.AddReceipt(receipt);
            if (string.IsNullOrEmpty(receiptBLL.ErrorMessage))
            {
                receipts.Add(receipt);
            }
        }

        private ICommand addReceiptCommand;
        public ICommand AddReceiptCommand
        {
            get
            {
                return addReceiptCommand ?? (addReceiptCommand = new RelayCommand(AddReceipt));
            }
        }

        public void DeleteReceipt(object obj)
        {
            Receipt receipt = obj as Receipt;
            if (receipt == null)
            {
                receiptBLL.ErrorMessage = "Invalid input!";
                return;
            }
            receiptBLL.DeleteReceipt(receipt);
            if (string.IsNullOrEmpty(receiptBLL.ErrorMessage))
            {
                receipts.Remove(receipt);
            }
        }

        private ICommand deleteReceiptCommand;
        public ICommand DeleteReceiptCommand
        {
            get
            {
                return deleteReceiptCommand ?? (deleteReceiptCommand = new RelayCommand(DeleteReceipt));
            }
        }

        private bool IsValidReceipt(Receipt receipt)
        {
            if (receipt == null)
            {
                return false;
            }

            // Validate receipt ID (assuming it must be a positive integer)
            if (receipt.receipt_id <= 0)
            {
                return false;
            }

            // Validate date of issue (assuming it cannot be a future date)
            if (receipt.date_issue > DateTime.Now)
            {
                return false;
            }

            // Validate cashier ID (assuming cashier ID must also be a positive integer)
            if (receipt.cashier_id <= 0)
            {
                return false;
            }

            // Validate total price (assuming it should be non-negative)
            if (receipt.total_price < 0)
            {
                return false;
            }

            // Validate User (assuming that a valid User must be linked with the receipt)
            if (receipt.User == null || receipt.User.user_id <= 0) // UserId check depends on your User class structure
            {
                return false;
            }

            return true;
        }

    }
}
