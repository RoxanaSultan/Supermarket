using Supermarket.Models;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Supermarket.Converters
{
    class ProducerConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2 || values[0] == null || values[1] == null)
                return null; // Return null if not all values are provided.

            string Name = values[0].ToString();
            string Country = values[1].ToString();
            return new Producer { name = Name, country = Country };
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException(); // Typically, only one-way binding is needed for command parameters.
        }
    }
}
