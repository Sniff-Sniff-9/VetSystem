using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Globalization;
using System.Windows.Data;

namespace VetSystemWpfDesktop.Converters
{
    public class WorkdayTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TimeOnly time && time == TimeOnly.MinValue)
                return "Выходной"; // если день отсутствует

            time = (TimeOnly)value;

            return time.ToString("HH:mm"); // иначе показываем время
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s && s == "Выходной")
                return TimeOnly.MinValue;

            s = (string)value;

            return TimeOnly.Parse(s);
        }
    }
}