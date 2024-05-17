﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Supermarket.Converters
{
    class DayProfitConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values != null && values.Length == 2 && values[0] is int && values[1] is string)
            {
                int user_id = (int)values[0];
                string monthName = (string)values[1];
                int monthNr = 0;
                switch (monthName)
                {
                    case "January":
                        monthNr = 1;
                        break;
                    case "February":
                        monthNr = 2;
                        break;
                    case "March":
                        monthNr = 3;
                        break;
                    case "April":
                        monthNr = 4;
                        break;
                    case "May":
                        monthNr = 5;
                        break;
                    case "June":
                        monthNr = 6;
                        break;
                    case "July":
                        monthNr = 7;
                        break;
                    case "August":
                        monthNr = 8;
                        break;
                    case "September":
                        monthNr = 9;
                        break;
                    case "October":
                        monthNr = 10;
                        break;
                    case "November":
                        monthNr = 11;
                        break;
                    case "December":
                        monthNr = 12;
                        break;
                    default:
                        break;
                }

                // Combine the user_id and month name
                return Tuple.Create(user_id, monthNr);
            }
            else
            {
                // If the provided values are not as expected, return a default value or throw an exception
                // For example:
                return null;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    
}
