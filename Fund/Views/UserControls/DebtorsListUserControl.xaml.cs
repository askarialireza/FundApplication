using System.Linq;

namespace Fund
{
    public partial class DebtorsListUserControl : System.Windows.Controls.UserControl
    {
        public DebtorsListUserControl()
        {
            InitializeComponent();

            LoadGridControl();
        }

        private void LoadGridControl()
        {
            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                var varList = oUnitOfWork.InstallmentRepository
                    .Get()
                    .Where(current => current.Loan.Member.FundId == Utility.CurrentFund.Id)
                    .Where(current => current.IsPayed == false)
                    .GroupBy(current => current.Loan.MemberId)
                    .Select(installment => new ViewModels.DebtorViewModel()
                    {
                        DebtAmount = installment
                                        .Sum(current => current.PaymentAmount),

                        FullName = installment
                                    .Select(current => current.Loan.Member.FullName)
                                    .FirstOrDefault(),


                        MemberId = installment
                                    .Select(current => current.Loan.Member.Id)
                                    .FirstOrDefault(),

                        NextPaymentDate = installment
                                            .Where(current => current.InstallmentDate >= System.DateTime.Now)
                                            .OrderBy(current => current.InstallmentDate)
                                            .Select(current=>current.InstallmentDate)
                                            .FirstOrDefault(),

                        InstallmentCount = installment
                                            .Count(),
                    })
                    .ToList();


                DebtorsListGridControl.ItemsSource = varList;

                ExportToPdfButton.IsEnabled = (varList.Count == 0) ? false : true;
                PrintButton.IsEnabled = (varList.Count == 0) ? false : true;

                oUnitOfWork.Save();
            }
            catch (System.Exception ex)
            {
                Infrastructure.MessageBox.Show(ex.Message);;
            }
            finally
            {
                if (oUnitOfWork != null)
                {
                    oUnitOfWork.Dispose();
                    oUnitOfWork = null;
                }
            }
        }

        private void PrintButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ShowReport(reportType: Infrastructure.ReportType.Print);
        }

        private void ExportToPdfButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ShowReport(reportType: Infrastructure.ReportType.ExportToPDF);
        }

        private void ShowReport(Infrastructure.ReportType reportType)
        {
            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                var varList = oUnitOfWork.InstallmentRepository
                    .Get()
                    .Where(current => current.Loan.Member.FundId == Utility.CurrentFund.Id)
                    .Where(current => current.IsPayed == false)
                    .GroupBy(current => current.Loan.MemberId)
                    .Select(installment => new ViewModels.DebtorViewModel()
                    {
                        DebtAmount = installment
                                        .Sum(current => current.PaymentAmount),

                        FullName = installment
                                    .Select(current => current.Loan.Member.FullName)
                                    .FirstOrDefault(),

                        NextPaymentDate = installment
                                            .Where(current => current.InstallmentDate >= System.DateTime.Now)
                                            .OrderBy(current => current.InstallmentDate)
                                            .Select(current => current.InstallmentDate)
                                            .FirstOrDefault(),

                        InstallmentCount = installment
                                            .Count(),
                    })
                    .Select( current => new
                    {
                        DebtAmount = current.DebtAmount.ToRialStringFormat(),
                        FullName = current.FullName.ToString(),
                        LastPaymentDate = ((System.DateTime)current.NextPaymentDate).ToPersianDate(),
                        current.InstallmentCount,
                    })
                    .ToList();

                oUnitOfWork.Save();
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
            finally
            {
                if (oUnitOfWork != null)
                {
                    oUnitOfWork.Dispose();
                    oUnitOfWork = null;
                }

            }
        }

        private void closeButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
