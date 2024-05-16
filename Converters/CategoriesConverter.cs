using System;
using System.Globalization;
using System.Windows.Data;

namespace Supermarket.Converters
{
    public class CategoriesConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // Ensure that two values are passed
            if (values.Length == 2 && values[0] is string && values[1] is string)
            {
                // Create a tuple containing the selected item and the new category text
                return Tuple.Create((string)values[0], (string)values[1]);

            }
            else
            {
                // If the number of values is incorrect, return null or throw an exception
                // Return null or throw exception based on your application's requirements
                return null;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}