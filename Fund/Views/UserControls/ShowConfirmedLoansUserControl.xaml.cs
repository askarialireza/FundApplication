using System.Linq;

namespace Fund
{
    /// <summary>
    /// Interaction logic for ShowConfirmedLoansUserControl.xaml
    /// </summary>
    public partial class ShowConfirmedLoansUserControl : System.Windows.Controls.UserControl
    {
        public ShowConfirmedLoansUserControl()
        {
            InitializeComponent();

            LoadGridControl();
        }

        private void closeButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            System.Windows.Controls.Panel oPanel = this.Parent as System.Windows.Controls.Panel;
            oPanel.Children.Remove(this);
        }

        private void ExportToPdfButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ShowReport(Infrastructure.Report.ExportType.ExportToPDF);
        }

        private void PrintButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ShowReport(Infrastructure.Report.ExportType.ExportToPDF);
        }

        private void LoadGridControl()
        {
            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                var varList = oUnitOfWork.LoanRepository
                    .Get()
                    .Where(current=>current.Member.FundId == Utility.CurrentFund.Id)
                    .OrderBy(current => current.StartDate)
                    .ToList();

                LoansGridControl.ItemsSource = varList;

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

        private void InstallmentsOfLoan_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Models.Loan oLoan = LoansGridControl.SelectedItem as Models.Loan;

            if (oLoan != null)
            {
                Utility.CurrentLoan = oLoan;

                ShowInstallmentsListPerLoanWindow oShowInstallmentsListPerLoanWindow =
                    new ShowInstallmentsListPerLoanWindow();

                oShowInstallmentsListPerLoanWindow.ShowDialog();
            }
        }

        private void ShowReport(Infrastructure.Report.ExportType reportType)
        {
            var varList = (LoansGridControl.ItemsSource as System.Collections.Generic.List<Models.Loan>)
                .OrderBy(current => current.StartDate)
                .Select(current => new ViewModels.LoanViewModel()
                {
                     IsPayed = current.IsPayed,
                     IsActive = current.IsActive,
                     LoanAmount =current.LoanAmount,
                     RefundAmount = current.RefundAmount,
                     EndDate = current.EndDate,
                     StartDate= current.StartDate,
                     InstallmentsCount =current.InstallmentsCount,
                     MemberId = current.MemberId,
                })
                .Select(current => new
                {
                    current.LoanAmountRialFormat,
                    current.RefundAmountRialFormat,
                    current.PersianEndDate,
                    current.PersianStartDate,
                    current.InstallmentsCount,
                    current.IsPayedDescrption,
                    FullName = current.FullName.ToString(),
                })
                .ToList();
            ;
            

            Stimulsoft.Report.StiReport oStiReport = new Stimulsoft.Report.StiReport();

            oStiReport.Load(Properties.Resources.ConfirmedLoansListReport);

            oStiReport.Dictionary.Variables.Add("Today", System.DateTime.Now.ToPersianDate());
            oStiReport.Dictionary.Variables.Add("FundName", Utility.CurrentFund.Name);
            oStiReport.Dictionary.Variables.Add("FundManagerName", Utility.CurrentFund.ManagerName);
            oStiReport.RegBusinessObject("Loans", varList);
            oStiReport.Compile();
            oStiReport.RenderWithWpf(); oStiReport.WriteToReportRenderingMessages("در حال تهیه گزارش ...");

            oStiReport.DoAction(action: reportType, fileName: "گزارش وام‌های پرداختی");
        }
    }
}
