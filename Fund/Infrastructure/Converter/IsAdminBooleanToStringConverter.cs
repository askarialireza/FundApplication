
namespace Infrastructure.Converter
{
    public class IsAdminBooleanToStringConverter : System.Windows.Markup.MarkupExtension, System.Windows.Data.IValueConverter
    {
        public static IsAdminBooleanToStringConverter _IsAdminBooleanToStringConverter;

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
                        return "کاربر مدیر";
                    }

                case false:
                    {
                        return "کاربر عادی";
                    }
            }

            return null;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string strValue = (string)value;

            switch (strValue)
            {
                case "کاربر مدیر":
                    {
                        return true;
                    }
                case "کاربر عادی":
                    {
                        return false;
                    }
            }

            return null;
        }

        public override object ProvideValue(System.IServiceProvider serviceProvider)
        {
            if (_IsAdminBooleanToStringConverter == null)
            {
                _IsAdminBooleanToStringConverter = new IsAdminBooleanToStringConverter();
            }

            return (_IsAdminBooleanToStringConverter);
        }
    }
}
