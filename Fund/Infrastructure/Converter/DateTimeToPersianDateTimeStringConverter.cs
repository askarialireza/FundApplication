
namespace Infrastructure.Converter
{
    public class DateTimeToPersianDateTimeStringConverter : System.Windows.Markup.MarkupExtension, System.Windows.Data.IValueConverter
    {
        public static DateTimeToPersianDateTimeStringConverter _DateTimeToPersianDateTimeStringConverter;

        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((value is System.DateTime) == false)
            {
                return string.Empty;
            }

            System.DateTime oDateTime = (System.DateTime)value;

            return (new FarsiLibrary.Utils.PersianDate(oDateTime).ToString());
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
            if (_DateTimeToPersianDateTimeStringConverter == null)
            {
                _DateTimeToPersianDateTimeStringConverter = new DateTimeToPersianDateTimeStringConverter();
            }

            return (_DateTimeToPersianDateTimeStringConverter);
        }
    }
}
