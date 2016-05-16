using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WpfForumList.Converters
{
    public class QueueCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
                int count = System.Convert.ToInt32(value);
                if (count == 0)
                    return String.Empty;
                else
                    return value.ToString();
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return new NotImplementedException();
        }
    }
 
}
