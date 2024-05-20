using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Supermarket.Models.Database;
using Supermarket.ViewModels;
using Supermarket.Views;

namespace Supermarket.Models.BusinessLogicLayer
{
    public class ReceiptBLL
    {
        private supermarketEntities context = new supermarketEntities();
        public string ErrorMessage { get; set; }
        public void AddReceipt(object obj)
        {
            Receipt receipt = obj as Receipt;
            if (receipt == null)
            {
                ErrorMessage = "Invalid input!";
                return;
            }
            var existingUser = context.Users.FirstOrDefault(u => u.user_id == receipt.cashier_id);
            if (existingUser == null)
            {
                ErrorMessage = "User not found!";
                return;
            }
            receipt.User = existingUser;
            context.Receipts.Add(receipt);
            context.SaveChanges();
        }

        public void DeleteReceipt(object obj)
        {
            Receipt receipt = obj as Receipt;
            if (receipt == null)
            {
                ErrorMessage = "Invalid input!";
                return;
            }
            Receipt oldReceipt = context.Receipts.FirstOrDefault(r => r.receipt_id == receipt.receipt_id);
            if (oldReceipt == null)
            {
                ErrorMessage = "Receipt not found!";
                return;
            }
            context.Receipts.Remove(oldReceipt);
            context.SaveChanges();
        }

        public List<Receipt> GetReceipts()
        {
            return context.Receipts.ToList();
        }

        public Receipt GetMaximumReceiptInADate(DateTime date)
        {
            return context.Receipts.Where(r => r.date_issue == date).ToList().OrderByDescending(r => r.total_price).FirstOrDefault();
        }

        public List<Product> GetProductsByName(string name)
        {
            return GetProductsFrom(context.Products.Where(p => p.name.Contains(name)).ToList());
        }

        public List<Product> GetProductsByBarcode(string barcode)
        {
            return context.Products.Where(p => p.barcode.Contains(barcode)).ToList();
        }

        public List<Product> GetProductsByCategory(string category)
        {
            return context.Products.Where(p => p.category.Contains(category)).ToList();
        }

        public List<Product> GetProductsByProducerName(string name)
        {
            return context.Products.Where(p => p.name == name).ToList();
        }

        public List<Product> GetProductsByExpirationDate(DateTime date)
        {
            List<GetProductsByExpirationDate_Result> results = context.GetProductsByExpirationDate(date).ToList();
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
                    Producer = GetProducerById(result.producer_id)
                    // Add other properties as needed
                };
                products.Add(product);
            }
            return products;
            
        }

        public List<Product> GetProductsFrom(List<Product> results)
        {

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
                    Producer = GetProducerById(result.producer_id)
                    // Add other properties as needed
                };
                products.Add(product);
            }
            return products;
            //return context.Products.ToList();
        }

        public Producer GetProducerById(int producerId)
        {
            // Query the database using LINQ to Entities to find the Producer by ID
            return context.Producers.FirstOrDefault(p => p.producer_id == producerId);
        }

        public int getUserId(string username)
        {
            return context.Users.FirstOrDefault(u => u.name == username).user_id;
        }

        public User GetUser(string username)
        {
            return context.Users.FirstOrDefault(u => u.name == username);
        }


        public Product GetProduct(int productId)
        {
               return context.Products.FirstOrDefault(p => p.product_id == productId);
        }

        public void UpdateInventory(int productId)
        {
            context.UpdateInventory(productId);
        }

        public bool IsStockAvailable(int productId)
        {
            var inventoryStatus = context.Inventories.FirstOrDefault(i => i.product_id == productId);
            if (inventoryStatus != null)
            {
                return inventoryStatus.active;
            }
            return false;
        }
        public double GetPriceForProduct(int productId)
        {
            var result = context.GetPriceForProduct(productId).FirstOrDefault();
            return result.Value;
        }
        public int GetLastReceipt()
        {
            var result = context.GetLastReceipt().FirstOrDefault();
            return result.Value;
        }

        public List<Tuple<string, int, double>> GetProductFromReceipt(DateTime date)
        {
            List<GetProductFromReceipt_Result> results = context.GetProductFromReceipt(date).ToList();
            List<Tuple<string, int, double>> products = new List<Tuple<string, int, double>>();
            foreach (var result in results)
            {
                Tuple<string, int, double> productTuple = Tuple.Create(result.product, result.quantity, result.price);
                products.Add(productTuple);
            }
            return products;
        }


    }
}
