using System.Linq;

namespace Fund
{

    public partial class LoanDetailsWindow : DevExpress.Xpf.Core.DXWindow
    {
        private System.Guid LoanId;

        public LoanDetailsWindow(System.Guid loanId)
        {
            InitializeComponent();

            LoanId = loanId;

            LoadData();
        }

        private void LoadData()
        {
            DAL.UnitOfWork oUnitOfWork = new DAL.UnitOfWork();

            Models.Loan oLoan = oUnitOfWork.LoanRepository
                .GetById(LoanId);

            MemberNameLabel.Content = oLoan.Member.FullName;

            StartDatelLabel.Content = oLoan.StartDate.ToPersianDate();

            EndDatelLabel.Content = oLoan.EndDate.ToPersianDate();

            LoanAmountLabel.Content = oLoan.LoanAmount.ToRial();

            RefundAmountLabel.Content = oLoan.RefundAmount.ToRial();

            InstallmentCountLabel.Content = oLoan.InstallmentsCount + " قسط ";

            var varData = oLoan.Installments
                .Where(current => current.IsPayed == true)
                .OrderByDescending(current => current.PaymentDate)
                .FirstOrDefault();

            if (varData != null)
            {
                System.DateTime? oDateTime = varData.PaymentDate;

                LastPayedInstallmentLabel.Content = (oDateTime.HasValue == true) ? ((System.DateTime)oDateTime).ToPersianDate() : string.Empty;
            }
            else
            {
                LastPayedInstallmentLabel.Content = string.Empty;
            }

            PayedInstallmentCountLabel.Content = oLoan.Installments
                .Where(current => current.IsPayed == true)
                .Count()
                + " قسط";

            UnpayedInstallmentCountLabel.Content = oLoan.Installments
                .Where(current => current.IsPayed == false)
                .Count()
                + " قسط";

            SumOfPayedInstallmentsLabel.Content = oLoan.Installments
                .Where(current => current.IsPayed == true)
                .Select(current => current.PaymentAmount)
                .Sum()
                .ToRial();

            SumOfUnpayedInstallmentsLabel.Content = oLoan.Installments
                .Where(current => current.IsPayed == false)
                .Select(current => current.PaymentAmount)
                .Sum()
                .ToRial();

        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, System.Windows.RoutedEventArgs e)
        {
            DAL.UnitOfWork oUnitOfWork = new DAL.UnitOfWork();

            Utility.CurrentLoan = oUnitOfWork.LoanRepository
                .GetById(LoanId);

            ShowInstallmentsListPerLoanWindow oShowInstallmentsListPerLoanWindow =
                new ShowInstallmentsListPerLoanWindow();

            oShowInstallmentsListPerLoanWindow.ShowDialog();
        }
    }
}
