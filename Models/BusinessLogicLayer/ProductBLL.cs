using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
            if(product.Producer.active == false)
            {
                ErrorMessage = "Producer is not active!";
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
                    producer_id = result.producer_id,
                    Producer = GetProducerById(result.producer_id),
                    active = result.active
                    // Add other properties as needed
                };
                if(product.active == true)
                {
                    products.Add(product);
                }
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
        public Producer GetProducerById(int producerId)
        {  
            // Query the database using LINQ to Entities to find the Producer by ID
            return context.Producers.FirstOrDefault(p => p.producer_id == producerId);
        }

        public List<Tuple<string,decimal>> GetCategoryProfit()
        {
            var result = context.GetProfitForCategories();
            List<Tuple<string,decimal>> myList = new List<Tuple<string, decimal>>();
            foreach(var item in result)
            {
                myList.Add(new Tuple<string, decimal>(item.category, (decimal)item.total_profit));
            }
            return myList;
        }

}
}
