using Supermarket.Models.Database;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Xml.Linq;

namespace Supermarket.Converters
{
    class StockConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 6 || string.IsNullOrEmpty(values[0]?.ToString()) || string.IsNullOrEmpty(values[1]?.ToString()) || string.IsNullOrEmpty(values[2]?.ToString()) || string.IsNullOrEmpty(values[3]?.ToString()) || string.IsNullOrEmpty(values[4]?.ToString()) || string.IsNullOrEmpty(values[5]?.ToString()))
            {
                return null;
            }

            int quantity = System.Convert.ToInt32(values[0]);
            string measure = values[1].ToString();
            if (DateTime.TryParse(values[2].ToString(), out DateTime dateSupply) == false || DateTime.TryParse(values[3].ToString(), out DateTime dateExpiration) == false)
            {
                return null;
            }
            double pricePurchase = System.Convert.ToDouble(values[4]);
            double priceSelling = pricePurchase + (pricePurchase * 0.1);
            string productName = values[5].ToString();
            
            // Retrieve the ID of the producer based on the name
            using (var db = new supermarketEntities())
            {
                var product = db.Products.FirstOrDefault(p => p.name == productName);
                if (product != null)
                {
                   
                    //Product product = context.Products.FirstOrDefault(p => p.name == productName);
                    return new Inventory
                    {
                        quantity = quantity,
                        measure = measure,
                        date_supply = dateSupply,
                        date_expiration = dateExpiration,
                        price_purchase = pricePurchase,
                        price_selling = priceSelling,
                        product_id = product.product_id,
                        Product = product
                    };
                }
            }


            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException(); // Implement this method if needed
        }
    }
    
}
