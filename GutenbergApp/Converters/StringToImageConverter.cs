using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace GutenbergApp.Converters
{
    public class StringToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is string base64String && !string.IsNullOrWhiteSpace(base64String))
            {
                byte[] imageArray = System.Convert.FromBase64String(base64String);

                if(imageArray != null && imageArray.Length > 0)
                {
                    return ImageSource.FromStream(() => new MemoryStream(imageArray));
                }
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
