using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace VetSystemWpfDesktop.Converters
{
    public class StarWidthConverter : IValueConverter
    {
        public double Stars { get; set; } = 1;       // количество звезд для колонки
        public double TotalStars { get; set; } = 15; // сумма всех звездочек

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double totalWidth)
            {
                // вычисляем ширину пропорционально звёздочкам
                return totalWidth * (Stars / TotalStars);
            }
            return 100.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
