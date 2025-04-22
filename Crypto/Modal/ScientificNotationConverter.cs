using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Crypto.Modal
{
    public class ScientificNotationConverter : IValueConverter
    {

        
        private const decimal UpperThreshold = 100000000000000;
        private const decimal LowerThreshold = 0.00001m;
        private const decimal IntegerThreshold = LowerThreshold;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is decimal dec))
                return value;

            var abs = Math.Abs(dec);
            var frac = abs - Math.Truncate(abs);

            
            if (frac < IntegerThreshold)
                return dec.ToString("F0", culture);

            
            if ((abs > UpperThreshold && abs > 0) ||
                (abs < LowerThreshold && abs > 0))
            {
                
                return dec.ToString("0.###E+0", culture);
            }

            
            if (abs >= 1)
            {
                return dec.ToString("F3", culture);
            }

            
            return dec.ToString("F4", culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var s = (value as string)?.Trim();
            if (string.IsNullOrWhiteSpace(s))
                return 0m;

           
            if (decimal.TryParse(s, NumberStyles.Float, culture, out var dec))
                return dec;

            
            return 0m;
        }
    }
}
