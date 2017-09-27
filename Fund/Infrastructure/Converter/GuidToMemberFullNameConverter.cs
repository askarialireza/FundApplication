using System.Linq;

namespace Infrastructure.Converter
{
    public class GuidToMemberFullNameConverter : System.Windows.Markup.MarkupExtension, System.Windows.Data.IValueConverter
    {
        public static GuidToMemberFullNameConverter _GuidToMemberFullNameConverter;

        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            System.Guid id = (System.Guid)value;

            DAL.UnitOfWork oUnitOfWork = new DAL.UnitOfWork();

            Models.Member oMember = oUnitOfWork.MemberRepository
                .Get()
                .Where(current => current.Id == id)
                .FirstOrDefault()
                ;

            if (oMember != null)
            {
                return (oMember.FullName);
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }

        public override object ProvideValue(System.IServiceProvider serviceProvider)
        {
            if (_GuidToMemberFullNameConverter == null)
            {
                _GuidToMemberFullNameConverter = new GuidToMemberFullNameConverter();
            }

            return (_GuidToMemberFullNameConverter);
        }
    }
}
