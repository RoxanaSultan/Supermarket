using Supermarket.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.ViewModels
{
    class AdminVM : BaseVM
    {
        public UserVM userVM { get; set; }
        public ProducerVM producerVM { get; set; }
        public ProductVM productVM { get; set; }
        public InventoryVM inventoryVM { get; set; }
        public OfferVM offerVM { get; set; }
        public ReceiptVM receiptVM { get; set; }

        // Constructor
        public AdminVM()
        {
            userVM = new UserVM();
            producerVM = new ProducerVM();
            productVM = new ProductVM();
            inventoryVM = new InventoryVM();
            offerVM = new OfferVM();
            receiptVM = new ReceiptVM("admin");
            
        }
    }

}
