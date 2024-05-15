using System;
using System.Globalization;
using System.Windows.Data;
using Supermarket.Models.Database;

namespace Supermarket.Converters
{
    public class UserConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // Check if the values array has the expected length and if all values are provided
            if (values.Length == 3 && values[0] != null && values[1] != null && values[2] != null)
            {
                // Assuming the values are in the order of: username, age, email
                string Username = values[0].ToString();
                string Password = values[1].ToString();
                string Type = values[2].ToString();

                // Create and return a new User object
                return new User { name = Username, password = Password, type = Type };
            }
            else
            {
                // Return null if the values are not provided correctly
                return null;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException(); // Implement this method if needed
        }
    }
}
