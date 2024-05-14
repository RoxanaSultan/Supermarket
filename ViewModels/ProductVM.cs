using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Supermarket.Helpers;
using Supermarket.Models.BusinessLogicLayer;
using Supermarket.Models;
using Supermarket.Views;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace Supermarket.ViewModels
{
    class ProductVM :  BaseVM
    {
        private ProductBLL productBLL;
        private ObservableCollection<Product> products;
        public ProductVM()
        {
            productBLL = new ProductBLL();
            products = new ObservableCollection<Product>(productBLL.GetProducts());
        }

        public void AddProduct(object obj)
        {
            Product product = obj as Product;
            if (product == null)
            {
                productBLL.ErrorMessage = "Invalid input!";
                return;
            }
            if (!IsValidProduct(product))
            {
                productBLL.ErrorMessage = "Validation failed!";
                return;
            }
            productBLL.AddProduct(product);
            if (string.IsNullOrEmpty(productBLL.ErrorMessage))
            {
                products = new ObservableCollection<Product>(productBLL.GetProducts());
            }
        }

        private ICommand addProductCommand;
        public ICommand AddProductCommand
        {
            get
            {
                return addProductCommand ?? (addProductCommand = new RelayCommand(AddProduct));
            }
        }

        public void UpdateProduct(object obj)
        {
            Product product = obj as Product;
            if (product == null)
            {
                productBLL.ErrorMessage = "Invalid input!";
                return;
            }
            if (!IsValidProduct(product))
            {
                productBLL.ErrorMessage = "Validation failed!";
                return;
            }
            productBLL.UpdateProduct(product);
            if (string.IsNullOrEmpty(productBLL.ErrorMessage))
            {
                products = new ObservableCollection<Product>(productBLL.GetProducts());
            }
        }
        private ICommand updateProductCommand;
        public ICommand UpdateProductCommand
        {
            get
            {
                return updateProductCommand ?? (updateProductCommand = new RelayCommand(UpdateProduct));
            }
        }


        public void DeleteProduct(object obj)
        {
            Product product = obj as Product;
            if (product == null)
            {
                productBLL.ErrorMessage = "Invalid input!";
                return;
            }
            productBLL.DeleteProduct(product);
            if (string.IsNullOrEmpty(productBLL.ErrorMessage))
            {
                products = new ObservableCollection<Product>(productBLL.GetProducts());
            }
        }
        private ICommand deleteProductCommand;
        public ICommand DeleteProductCommand
        {
            get
            {
                return deleteProductCommand ?? (deleteProductCommand = new RelayCommand(DeleteProduct));
            }
        }


        private bool IsValidProduct(Product product)
        {
            // Validate product ID
            if (product.product_id <= 0)
            {
                productBLL.ErrorMessage = "Product ID must be greater than 0.";
                return false;
            }

            // Validate name
            if (string.IsNullOrWhiteSpace(product.name))
            {
                productBLL.ErrorMessage = "Product name cannot be empty or whitespace.";
                return false;
            }

            // Validate barcode
            if (string.IsNullOrWhiteSpace(product.barcode))
            {
                productBLL.ErrorMessage = "Barcode cannot be empty or whitespace.";
                return false;
            }

            // Validate category
            if (string.IsNullOrWhiteSpace(product.category))
            {
                productBLL.ErrorMessage = "Category cannot be empty or whitespace.";
                return false;
            }

            // Validate producer ID
            if (product.producer_id <= 0)
            {
                productBLL.ErrorMessage = "Producer ID must be greater than 0.";
                return false;
            }

            return true; // All validations passed
        }

    }
}
