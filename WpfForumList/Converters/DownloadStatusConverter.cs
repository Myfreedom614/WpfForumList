using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WpfForumList.Converters
{
    public class DownloadStatusConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string v1, v2;
            v1 = values[0].ToString();
            v2 = values[1].ToString();

            if (v1 == "Collapsed" && v2 == "Collapsed")
            {
                return "Download Completed, Please Close this Window Now";
            }

            return "Press Button to Download Data Files";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
