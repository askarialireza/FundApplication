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
            ShowReport(Infrastructure.ReportType.ExportToPDF);
        }

        private void PrintButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ShowReport(Infrastructure.ReportType.ExportToPDF);
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
                    .Select(current => new ViewModels.LoanViewModel()
                    {
                        Id = current.Id,
                        StartDate = current.StartDate,
                        LoanAmount = current.LoanAmount,
                        InstallmentsCount = current.InstallmentsCount,
                        Description = current.Description,
                        RefundAmount = current.RefundAmount,
                        EndDate = current.EndDate,
                        MemberId = current.MemberId,
                        IsPayed = current.IsPayed,
                    })
                    .ToList();

                LoansGridControl.ItemsSource = varList;

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
            ViewModels.LoanViewModel oViewModel = LoansGridControl.SelectedItem as ViewModels.LoanViewModel;

            if (oViewModel != null)
            {
                DAL.UnitOfWork oUnitOfWork = null;

                oUnitOfWork = new DAL.UnitOfWork();

                Utility.CurrentLoan = oUnitOfWork.LoanRepository
                    .GetById(oViewModel.Id);

                ShowInstallmentsListPerLoanWindow oShowInstallmentsListPerLoanWindow =
                    new ShowInstallmentsListPerLoanWindow();

                oShowInstallmentsListPerLoanWindow.ShowDialog();
            }
        }

        private void ShowReport(Infrastructure.ReportType reportType)
        {
            var varList = (LoansGridControl.ItemsSource as System.Collections.Generic.List<ViewModels.LoanViewModel>)
                .OrderBy(current => current.StartDate)
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

            if (varList.Count == 0)
            {
                Infrastructure.MessageBox.Show
                    (
                        caption: Infrastructure.MessageBoxCaption.Error,
                        text: "اطلاعاتی برای تهیه گزارش در جدول موجود نمی‌باشد. "
                    );

                return;
            }

            Stimulsoft.Report.StiReport oStiReport = new Stimulsoft.Report.StiReport();

            oStiReport.Load(Properties.Resources.ConfirmedLoansListReport);

            oStiReport.Dictionary.Variables.Add("Today", System.DateTime.Now.ToPersianDate());
            oStiReport.Dictionary.Variables.Add("FundName", Utility.CurrentFund.Name);
            oStiReport.Dictionary.Variables.Add("FundManagerName", Utility.CurrentFund.ManagerName);
            oStiReport.RegBusinessObject("Loans", varList);
            oStiReport.Compile();
            oStiReport.RenderWithWpf();

            oStiReport.DoAction(action: reportType, fileName: "گزارش وام‌های پرداختی");
        }
    }
}
