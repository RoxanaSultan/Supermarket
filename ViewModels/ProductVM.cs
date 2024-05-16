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
using System.Diagnostics;

namespace Supermarket.ViewModels
{
    class ProductVM :  BaseVM
    {
        private ProductBLL productBLL;
        private ObservableCollection<Product> products;
        public ObservableCollection<Product> Products
        {
            get { 
                return products; }
            private set
            {
                products = value;
                NotifyPropertyChanged(nameof(Products));
            }
        }

        private ObservableCollection<string> categories;
        public ObservableCollection<string> Categories
        {
            get { 
                return productBLL.GetCategoriesObservable(); }
            private set
            {
                categories = value;
                NotifyPropertyChanged(nameof(Categories));
            }
        }
        public ProductVM()
        {
            productBLL = new ProductBLL();
            products = new ObservableCollection<Product>(productBLL.GetProducts());
            categories = new ObservableCollection<string>(productBLL.GetCategories());
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
            UpdateCategorySet();
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
            UpdateCategorySet();
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
            UpdateCategorySet();
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

        private ICommand updateCategoriesCommand;
        public ICommand UpdateCategoriesCommand
        {
            get
            {
                return updateCategoriesCommand ?? (updateCategoriesCommand = new RelayCommand(UpdateCategories));
            }
        }
        public void UpdateCategories(object obj)
        {
            if (obj is Tuple<string, string> parameters)
            {
                string oldCategory = parameters.Item1;
                string newCategory = parameters.Item2;

                // Call the business logic layer to update the categories
                productBLL.UpdateCategories(oldCategory, newCategory);
                ObservableCollection<Product> list = new ObservableCollection<Product>(productBLL.GetProducts());

                // Update the category set in the ViewModel
                UpdateCategorySet();
                UpdateProducts();
                NotifyPropertyChanged(nameof(Categories));
                NotifyPropertyChanged(nameof(Products));
                
            }
            else
            {
                // Handle incorrect parameter type
                throw new ArgumentException("Invalid parameter type. Expected Tuple<string, string>.");
            }
        }


        private void UpdateCategorySet()
        {
            List<string> categoryList = productBLL.GetCategories();
            foreach (var category in categoryList)
            {
                if (!Categories.Contains(category))
                {
                    Categories.Add(category);
                }
            }
            for (int i = categories.Count - 1; i >= 0; i--)
            {
                var category = Categories.ElementAt(i);
                if (!categoryList.Contains(category))
                {
                    Categories.Remove(category);
                }
            }

            NotifyPropertyChanged(nameof(Categories));

        }

        private void UpdateProducts()
        {
            //// Retrieve all products from the BLL
            //List<Product> allProducts = productBLL.GetProducts();

            // Iterate through each product
            foreach (var product in Products)
            {
                // Check if the product already exists in your collection
                Product existingProduct = GetExistingProduct(product);

                // If the existing product is found and it's different, update it
                if (existingProduct != null)
                {
                    Debug.WriteLine($"Updating product {product.category}");
                    product.name = existingProduct.name;
                    product.barcode = existingProduct.barcode;
                    product.category = existingProduct.category;
                    Debug.WriteLine($"Updated product {existingProduct.category}");

                }
            }
            NotifyPropertyChanged(nameof(Products));
            NotifyPropertyChanged(nameof(Categories));
        }

        private Product GetExistingProduct(Product newProduct)
        {
            foreach (var existingProduct in productBLL.GetProducts())
            {
                // Compare products based on some unique identifier, like ID or barcode
                if (existingProduct.product_id == newProduct.product_id)
                {
                    return existingProduct;
                }
            }
            return null;
        }


        private bool AreProductsEqual(Product existingProduct, Product newProduct)
        {
            if(existingProduct.name != newProduct.name)
            {
                return false;
            }
            if(existingProduct.barcode != newProduct.barcode)
            {
                return false;
            }
            if(existingProduct.category != newProduct.category)
            {
                return false;
            }
            if(existingProduct.producer_id != newProduct.producer_id)
            {
                return false;
            }
            return true;
            // Logic to compare if the existing product and the new product are equal
            // Return true if they are equal, false otherwise
        }

        private void UpdateExistingProduct(Product existingProduct, Product newProduct)
        {
            existingProduct.name = newProduct.name;
            existingProduct.barcode = newProduct.barcode;
            existingProduct.category = newProduct.category;
            // Logic to update the existing product with the data from the new product
        }

    }
}
