using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Crypto
{
    public class DivideWidthConverter : IValueConverter
    {
        /// <summary>
        /// Отримує загальну ширину ListView та ділить її на кількість колонок,
        /// передану у ConverterParameter (як рядок, що містить число).
        /// Можна додати поправку для відступів, якщо потрібно.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double totalWidth && parameter != null && int.TryParse(parameter.ToString(), out int colCount) && colCount > 0)
            {
                // Зауваження: Якщо потрібно врахувати внутрішні відступи ListView,
                // то можна відняти їх із totalWidth.
                return totalWidth / colCount;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
