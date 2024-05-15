using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supermarket.Models.Database;

namespace Supermarket.Models.BusinessLogicLayer
{
    public class ReceiptBLL
    {
        private supermarketEntities2 context = new supermarketEntities2();
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
    }
}
