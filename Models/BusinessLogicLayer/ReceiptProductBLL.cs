using Supermarket.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Supermarket.Models.BusinessLogicLayer
{
    class ReceiptProductBLL
    {
        supermarketEntities context = new supermarketEntities();
        public List<ReceiptProduct> GetReceiptProducts()
        {
            return context.ReceiptProducts.ToList();
        }

        public void AddReceiptProduct(int receiptID, int productID, int quantity, int inventoryID)
        {
            ReceiptProduct receiptProduct = new ReceiptProduct();
            receiptProduct.receipt_id = receiptID;
            receiptProduct.product_id = productID;
            receiptProduct.quantity = quantity;
            receiptProduct.inventory_id = inventoryID;
            context.ReceiptProducts.Add(receiptProduct);
            context.SaveChanges();
        }
        
        public int GetInventoryFromProduct(int productID)
        {
            return context.GetInventoryFromProduct(productID).FirstOrDefault().Value;
        }
    }
}
