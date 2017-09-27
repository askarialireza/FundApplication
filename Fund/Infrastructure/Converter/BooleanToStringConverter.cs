
namespace Infrastructure.Converter
{
    public class BooleanToStringConverter : System.Windows.Markup.MarkupExtension, System.Windows.Data.IValueConverter
    {
        public static BooleanToStringConverter _BooleanToStringConverter;

        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((value is bool) == false)
            {
                return string.Empty;
            }

            switch ((bool)value)
            {
                case true:
                    {
                        return "بله";
                    }

                case false:
                    {
                        return "خیر";
                    }
            }

            return null;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string strValue = (string)value;

            switch (strValue)
            {
                case "بله":
                    {
                        return true;
                    }
                case "خیر":
                    {
                        return false;
                    }
            }

            return null;
        }

        public override object ProvideValue(System.IServiceProvider serviceProvider)
        {
            if (_BooleanToStringConverter == null)
            {
                _BooleanToStringConverter = new BooleanToStringConverter();
            }

            return (_BooleanToStringConverter);
        }
    }
}
