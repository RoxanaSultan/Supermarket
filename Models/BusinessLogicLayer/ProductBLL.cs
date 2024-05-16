﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supermarket.Models.Database;

namespace Supermarket.Models.BusinessLogicLayer
{
    public class ProductBLL
    {
        private supermarketEntities context = new supermarketEntities();
        public string ErrorMessage { get; set; }
        public void AddProduct(object obj)
        {
            Product product = obj as Product;
            if (product == null)
            {
                ErrorMessage = "Invalid input!";
                return;
            }
            context.Products.Add(product);
            context.SaveChanges();
        }

        public void UpdateProduct(object obj)
        {
            Product product = obj as Product;
            if (product == null)
            {
                ErrorMessage = "Invalid input!";
                return;
            }
            Product oldProduct = context.Products.FirstOrDefault(p => p.product_id == product.product_id);
            if (oldProduct == null)
            {
                ErrorMessage = "Product not found!";
                return;
            }
            oldProduct.name = product.name.Substring(0, Math.Min(product.name.Length, 10));
            oldProduct.barcode = product.barcode.Substring(0, Math.Min(product.barcode.Length, 10));
            oldProduct.category = product.category.Substring(0, Math.Min(product.category.Length, 15));
            oldProduct.producer_id = product.producer_id;
            context.SaveChanges();
        }

        public void DeleteProduct(object obj)
        {
            Product product = obj as Product;
            if (product == null)
            {
                ErrorMessage = "Invalid input!";
                return;
            }
            Product oldProduct = context.Products.FirstOrDefault(p => p.product_id == product.product_id);
            if (oldProduct == null)
            {
                ErrorMessage = "Product not found!";
                return;
            }
            context.Products.Remove(oldProduct);
            context.SaveChanges();
        }

        public List<Product> GetProducts()
        {
            List<GetProducts_Result> results = context.GetProducts().ToList(); // Assuming this method retrieves data from the database

            List<Product> products = new List<Product>();
            foreach (var result in results)
            {
                // Convert GetProducts_Result objects to Product objects
                Product product = new Product
                {
                    // Assign properties based on data from GetProducts_Result
                    product_id = result.product_id,
                    name = result.name,
                    barcode = result.barcode,
                    category = result.category,
                    producer_id = result.producer_id
                    // Add other properties as needed
                };
                products.Add(product);
            }
            return products;
            //return context.Products.ToList();
        }

        public List<Product> GetProductsByCategory(string category)
        {
            return context.Products.Where(p => p.category == category).ToList();
        }

        public List<Product> GetProductsByProducer(string producer)
        {
            return context.Products.Where(p => p.Producer.name == producer).ToList();
        }

        public Product GetProductByBarcode(string barcode)
        {
            return context.Products.FirstOrDefault(p => p.barcode == barcode);
        }

        public void UpdateCategories(string oldCategory, string newCategory)
        {
            context.UpdateCategories(oldCategory, newCategory);
            context.SaveChanges();
        }

        public List<string> GetCategories()
        {
            return context.GetCategories().ToList();
        }
        public ObservableCollection<string> GetCategoriesObservable()
        {
            return new ObservableCollection<string>(context.GetCategories().ToList());
        }
    }
}
