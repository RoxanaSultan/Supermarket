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
using Supermarket.Models.Database;

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
        private ObservableCollection<Product> products;
        public ObservableCollection<Product> Products
        {
            get { return products; }
            private set
            {
                products = value;
                NotifyPropertyChanged(nameof(Products));
            }
        }

        private Dictionary<int, int> productsInReceipt;

        private List<string> receipt;
        public List<string> Receipt
        {
            get { return receipt; }
            set
            {
                receipt = value;
                NotifyPropertyChanged(nameof(Receipt));
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

        private ICommand searchProductCommand;
        public ICommand SearchProductCommand
        {
            get
            {
                return searchProductCommand ?? (searchProductCommand = new RelayCommand(SearchProduct));
            }
        }

        private void SearchProduct(object obj)
        {
            Tuple<string, string> tuple = obj as Tuple<string, string>;
            if (tuple == null)
            {
                receiptBLL.ErrorMessage = "Invalid input!";
                return;
            }

            string descripton = tuple.Item1;
            string type = tuple.Item2;
            switch(type)
            {
                case "Name":
                    string name = descripton;
                    if(name == null || name.Length == 0)
                    {
                        receiptBLL.ErrorMessage = "Invalid name!";
                        return;
                    }
                    Products = new ObservableCollection<Product>(receiptBLL.GetProductsByName(name));
                    break;
                case "Barcode":
                    string barcode = descripton;
                    if(barcode == null || barcode.Length == 0)
                    {
                        receiptBLL.ErrorMessage = "Invalid barcode!";
                        return;
                    }
                    Products = new ObservableCollection<Product>(receiptBLL.GetProductsByBarcode(barcode));
                    break;
                case "Expiration date":
                    DateTime expirationDate;
                    if (!DateTime.TryParse(descripton, out expirationDate))
                    {
                        receiptBLL.ErrorMessage = "Invalid expiration date!";
                        return;
                    }
                    Products = new ObservableCollection<Product>(receiptBLL.GetProductsByExpirationDate(expirationDate));
                    break;
                case "Producer":
                    string producer = descripton;
                    if(producer == null || producer.Length == 0)
                    {
                        receiptBLL.ErrorMessage = "Invalid producer!";
                        return;
                    }
                    Products = new ObservableCollection<Product>(receiptBLL.GetProductsByProducerName(producer));
                    break;
                case "Category":
                    string category = descripton;
                    if(category == null || category.Length == 0)
                    {
                        receiptBLL.ErrorMessage = "Invalid category!";
                        return;
                    }
                    Products = new ObservableCollection<Product>(receiptBLL.GetProductsByCategory(category));
                    break;
                default:
                    receiptBLL.ErrorMessage = "Invalid search type!";
                    break;
            }
            NotifyPropertyChanged(nameof(Receipts));
        }

        private ICommand addInReceipt;
        public ICommand AddInReceipt
        {
            get
            {
                return addInReceipt ?? (addInReceipt = new RelayCommand(AddProductInReceipt));
            }
        }

        private void AddProductInReceipt(object obj)
        {
            int product_id = Convert.ToInt32(obj);
            if(product_id == 0)
            {
                receiptBLL.ErrorMessage = "Invalid input!";
                return;
            }
            if(productsInReceipt.ContainsKey(product_id))
            {
                productsInReceipt[product_id]++;
            }
            else
            {
                productsInReceipt.Add(product_id, 1);
            }
            
        }

        private ICommand totalReceipt;
        public ICommand TotalReceipt
        {
            get
            {
                return totalReceipt ?? (totalReceipt = new RelayCommand(TotalReceiptPrice));
            }
        }
        private void TotalReceiptPrice(object obj)
        {
            foreach(var product in productsInReceipt)
            {

                Product p = products.FirstOrDefault(pr => pr.product_id == product.Key);
                decimal price = 30;
                if(p != null)
                {
                    receipt.Add(product.Value + " X " + p.name + " .....  " + price * product.Value + "lei");
                }
            }
            receipt.Add("Total: " + receipt.Sum(r => Convert.ToDecimal(r.Substring(r.LastIndexOf(".....") + 5, r.LastIndexOf("lei") - r.LastIndexOf(".....") - 5))) + "lei");
        }

    }
}
