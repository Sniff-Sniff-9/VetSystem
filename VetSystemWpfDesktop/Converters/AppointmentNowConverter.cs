using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace VetSystemWpfDesktop.Converters
{
    public class AppointmentNowConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 3)
                return false;

            if (values[0] is DateTime date &&
                values[1] is TimeOnly start &&
                values[2] is TimeOnly end)
            {
                var now = DateTime.Now;

                if (date.Date != now.Date)
                    return false;

                var nowTime = TimeOnly.FromDateTime(now);

                return nowTime >= start && nowTime <= end;
            }

            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
