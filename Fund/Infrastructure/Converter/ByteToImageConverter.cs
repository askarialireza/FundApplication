using System.Data;

namespace Infrastructure.Converter
{
    public class ByteToImageConverter : System.Windows.Markup.MarkupExtension, System.Windows.Data.IValueConverter
    {
        public static ByteToImageConverter _byteToImageConverter;

        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((value is byte[]) == false)
            {
                var uriSource = new System.Uri(@"/Fund;component/Resources/Images/MemberPicture.png", System.UriKind.Relative);

                return new System.Windows.Media.Imaging.BitmapImage(uriSource);
            }

            else
            {
                System.Windows.Media.Imaging.BitmapImage biImg = new System.Windows.Media.Imaging.BitmapImage();
                System.IO.MemoryStream ms = new System.IO.MemoryStream((byte[])value);
                biImg.BeginInit();
                biImg.StreamSource = ms;
                biImg.EndInit();

                System.Windows.Media.ImageSource imgSrc = biImg as System.Windows.Media.ImageSource;

                return imgSrc;
            }
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            System.Windows.Media.Imaging.BmpBitmapEncoder oBmpBitmapEncoder = new System.Windows.Media.Imaging.BmpBitmapEncoder();

            byte[] bytes = null;
            var bitmapSource = value as System.Windows.Media.Imaging.BitmapSource;

            if (bitmapSource != null)
            {
                oBmpBitmapEncoder.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(bitmapSource));

                using (var stream = new System.IO.MemoryStream())
                {
                    oBmpBitmapEncoder.Save(stream);
                    bytes = stream.ToArray();
                }
            }

            return bytes;
        }

        public override object ProvideValue(System.IServiceProvider serviceProvider)
        {
            if (_byteToImageConverter == null)
            {
                _byteToImageConverter = new ByteToImageConverter();
            }

            return _byteToImageConverter;
        }
    }
}
