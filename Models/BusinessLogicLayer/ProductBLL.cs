using System;
using System.Collections.Generic;
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
            oldProduct.name = product.name.Substring(0, 10);
            oldProduct.barcode = product.barcode.Substring(0,10);
            oldProduct.category = product.category.Substring(0,15);
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
            return context.Products.ToList();
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
        }
    }
}
