using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supermarket.ViewModels;
using Supermarket.Helpers;

namespace Supermarket.ViewModels
{
    class MainVM : BaseVM
    {
        public AdminVM adminVM { get; set; }
        public ReceiptVM receiptVM { get; set; }

        public MainVM()
        {
            adminVM = new AdminVM();
            receiptVM = new ReceiptVM();
        }   
    }
}
