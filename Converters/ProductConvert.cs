using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Supermarket.Models.Database;

namespace Supermarket.Converters
{
    class ProductConvert : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(values.Length!=4 || values[0] == null || values[1] == null || values[2] == null || values[3] == null)
            {
                return null;
            }
            string Name = values[0].ToString();
            string Barcode = values[1].ToString();
            string Category = values[2].ToString();
            string ProducerName = values[3].ToString();

            // Retrieve the ID of the producer based on the name
            using (var db = new supermarketEntities())
            {
                var producer = db.Producers.FirstOrDefault(p => p.name == ProducerName);
                if (producer != null)
                {
                    // If producer exists, create a new Product object with the retrieved ID
                    return new Product { name = Name, barcode = Barcode, category = Category, producer_id = producer.producer_id, Producer=producer };
                }
            }
            return null;

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
