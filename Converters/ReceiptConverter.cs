using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace Supermarket.Converters
{
    class ReceiptConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string searchText = values[0] as string;
            ComboBoxItem selectedItem = values[1] as ComboBoxItem;

            if (selectedItem == null || string.IsNullOrEmpty(searchText))
            {
                return null;
            }

            string searchType = selectedItem.Content as string;
            return Tuple.Create(searchText, searchType);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
