
namespace Infrastructure.Converter
{
    public class GenderToStringConverter : System.Windows.Markup.MarkupExtension, System.Windows.Data.IValueConverter
    {
        public static GenderToStringConverter _GenderToStringConverter;

        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Models.Gender oGender = (Models.Gender)value;

            if(oGender == Models.Gender.Male)
            {
                return ("آقا");
            }
            
            if(oGender == Models.Gender.Female)
            {
                return ("خانم");
            }

            return string.Empty;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string strValue = (string)value;

            if(strValue == "آقا")
            {
                return Models.Gender.Male;
            }
            
            if(strValue == "خانم")
            {
                return Models.Gender.Female;
            }

            return null;
        }

        public override object ProvideValue(System.IServiceProvider serviceProvider)
        {
            if(_GenderToStringConverter == null)
            {
                _GenderToStringConverter = new GenderToStringConverter();
            }

            return _GenderToStringConverter;
        }
    }
}
