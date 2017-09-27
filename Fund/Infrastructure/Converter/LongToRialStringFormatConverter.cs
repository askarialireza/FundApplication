
namespace Infrastructure.Converter
{
    public class LongToRialStringFormatConverter : System.Windows.Markup.MarkupExtension, System.Windows.Data.IValueConverter
    {
        public static LongToRialStringFormatConverter _LongToRialStringFormatConverter;

        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (((long)value).ToString("##,#0 ریال"));
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string strvalue = (string)value;

            strvalue = strvalue.Replace(" ریال", string.Empty).Replace(",", string.Empty);

            return (System.Convert.ToInt64(strvalue));
        }

        public override object ProvideValue(System.IServiceProvider serviceProvider)
        {
            if (_LongToRialStringFormatConverter == null)
            {
                _LongToRialStringFormatConverter = new LongToRialStringFormatConverter();
            }

            return _LongToRialStringFormatConverter;
        }
    }
}
