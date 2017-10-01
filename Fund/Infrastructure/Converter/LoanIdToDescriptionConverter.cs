using Fund;

namespace Infrastructure.Converter
{
    public class LoanIdToDescriptionConverter : System.Windows.Markup.MarkupExtension, System.Windows.Data.IValueConverter
    {
        public static LoanIdToDescriptionConverter _LoanIdToDescriptionConverter;

        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            System.Guid id = (System.Guid)value;

            DAL.UnitOfWork oUnitOfWork = new DAL.UnitOfWork();

            Models.Loan oLoan = oUnitOfWork.LoanRepository
                .GetById(id);

            string result = string.Empty;

            if (oLoan != null)
            {
                result = string.Format("وام به مبلغ: {0} | بازپرداخت: {1} | اقساط: {2} قسط | تاریخ پرداخت: {3}",
                    oLoan.LoanAmount.ToRialStringFormat(),
                    oLoan.RefundAmount.ToRialStringFormat(),
                    oLoan.InstallmentsCount,
                    oLoan.StartDate.ToPersianDate()
                    );
            }

            return result;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }

        public override object ProvideValue(System.IServiceProvider serviceProvider)
        {
            if (_LoanIdToDescriptionConverter == null)
            {
                _LoanIdToDescriptionConverter = new LoanIdToDescriptionConverter();
            }

            return _LoanIdToDescriptionConverter;
        }
    }
}
