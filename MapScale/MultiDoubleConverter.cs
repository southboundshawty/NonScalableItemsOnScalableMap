using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace MapScale
{
    public class MultiDoubleConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double[] nums = values.OfType<double>().ToArray();
            if (values.Length != nums.Length)
            {
                return null;
            }

            return nums.Aggregate((x, y) => x * y);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
