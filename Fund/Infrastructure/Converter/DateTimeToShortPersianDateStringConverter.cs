
namespace Infrastructure.Converter
{
    public class DateTimeToShortPersianDateStringConverter : System.Windows.Markup.MarkupExtension, System.Windows.Data.IValueConverter
    {
        public static DateTimeToShortPersianDateStringConverter _DateTimeToShortPersianDateStringConverter;

        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            System.DateTime oDateTime = (System.DateTime)value;

            return (new FarsiLibrary.Utils.PersianDate(oDateTime).ToString("d"));
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((value is string) == false)
            {
                return null;
            }

            string strValue = value as string;

            FarsiLibrary.Utils.PersianDate oPersianDate = new FarsiLibrary.Utils.PersianDate(strValue);

            return (FarsiLibrary.Utils.PersianDateConverter.ToGregorianDateTime(oPersianDate));
        }

        public override object ProvideValue(System.IServiceProvider serviceProvider)
        {
            if (_DateTimeToShortPersianDateStringConverter == null)
            {
                _DateTimeToShortPersianDateStringConverter = new DateTimeToShortPersianDateStringConverter();
            }

            return (_DateTimeToShortPersianDateStringConverter);
        }
    }
}
