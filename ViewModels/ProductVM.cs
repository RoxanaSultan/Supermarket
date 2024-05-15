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
using Supermarket.Models.Database;

namespace Supermarket.ViewModels
{
    class ProductVM :  BaseVM
    {
        private ProductBLL productBLL;
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
                products.Add(product);
            }
        }

        private ICommand addProductCommand;
        public ICommand AddCommand
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
                // Update the ObservableCollection
                var existingProduct = Products.FirstOrDefault(p => p.product_id == product.product_id);
                if (existingProduct != null)
                {
                    var index = Products.IndexOf(existingProduct);
                    Products[index] = product; // This only works if the collection is bound to UI with bindings that detect changes.
                }
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
                products.Remove(product);
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
