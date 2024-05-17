using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Supermarket.Models.Database;
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
            //aici e hardocdat! expiration date se ia din stocuri
            return context.Products.Where(p => p.name == "da").ToList();
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

    }
}
