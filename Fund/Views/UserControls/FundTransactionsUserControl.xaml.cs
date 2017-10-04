using System.Linq;

namespace Fund
{
    public partial class FundTransactionsUserControl : System.Windows.Controls.UserControl
    {
        public FundTransactionsUserControl()
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

                var varList = oUnitOfWork.TransactionRepository
                    .Get()
                    .Where(current => current.FundId == Utility.CurrentFund.Id)
                    .OrderBy(current => current.Date)
                    .Select(current => new ViewModels.TransactionViewModel()
                    {
                        Id = current.Id,
                        TransactionType = current.TransactionType,
                        Amount = current.Amount,
                        Balance = current.Balance,
                        Description = current.Description,
                        Date = current.Date,
                        FundId = current.FundId,
                        MemberId = current.MemberId,
                    })
                    .ToList();

                if (varList.Count == 0)
                {
                    Infrastructure.MessageBox.Show("تراکنشی برای صندوق در سیسستم ثبت نگردیده است .");

                    this.Close();
                }

                ExportToPdfButton.IsEnabled = (varList.Count == 0) ? false : true;
                PrintButton.IsEnabled = (varList.Count == 0) ? false : true;

                GridControl.ItemsSource = varList;

                fromDatePicker.PersianDateTextBox.Text = varList.Select(current => current.Date).FirstOrDefault().ToPersianDate();
                toDatePicker.PersianDateTextBox.Text = varList.Select(current => current.Date).LastOrDefault().ToPersianDate();

                oUnitOfWork.Save();
            }
            catch (System.Exception ex)
            {
                Infrastructure.MessageBox.Show(ex.Message); ;
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

        private void SearchButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            #region Error Handling Messages

            if (toDatePicker.SelectedDateTime < fromDatePicker.SelectedDateTime)
            {
                Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: "بازه زمانی درج شده نامعتبر می باشد.");

                return;
            }

            #endregion

            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                var varList = oUnitOfWork.TransactionRepository
                .GetTransactions(fromDatePicker.SelectedDateTime, toDatePicker.SelectedDateTime)
                .Where(current => current.FundId == Utility.CurrentFund.Id)
                .Select(current => new ViewModels.TransactionViewModel()
                {
                    Id = current.Id,
                    TransactionType = current.TransactionType,
                    Amount = current.Amount,
                    Balance = current.Balance,
                    Description = current.Description,
                    Date = current.Date,
                    FundId = current.FundId,
                    MemberId = current.MemberId,
                })
                .ToList();

                GridControl.ItemsSource = varList;

                ExportToPdfButton.IsEnabled = (varList.Count == 0) ? false : true;
                PrintButton.IsEnabled = (varList.Count == 0) ? false : true;

                oUnitOfWork.Save();
            }
            catch (System.Exception ex)
            {
                Infrastructure.MessageBox.Show(ex.Message); ;
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

        private void ShowAllButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            LoadGridControl();
        }

        private void PrintButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ShowReport(reportType: Infrastructure.Report.ExportType.Print);
        }

        private void ExportToPdfButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ShowReport(reportType: Infrastructure.Report.ExportType.ExportToPDF);
        }

        private void ShowReport(Infrastructure.Report.ExportType reportType)
        {
            var varList =
                (GridControl.ItemsSource as System.Collections.Generic.List<ViewModels.TransactionViewModel>)
                .OrderBy(current => current.Date)
                .Select(current => new
                {
                    current.AmountRialFormat,
                    current.BalanceRialFormat,
                    current.PersianDate,
                    current.TransactionDescription,
                    MemberFullName = current.MemberFullName.ToString(),
                })
                .ToList();

            if (varList.Count == 0)
            {
                Infrastructure.MessageBox.Show
                    (
                        caption: Infrastructure.MessageBox.Caption.Error,
                        text: "اطلاعاتی برای تهیه گزارش در جدول موجود نمی‌باشد. " + System.Environment.NewLine + "لطفا بازه زمانی دیگری را انتخاب نمایید."
                    );

                return;
            }

            Stimulsoft.Report.StiReport oStiReport = new Stimulsoft.Report.StiReport();

            oStiReport.Load(Properties.Resources.TransactionsReport);
            oStiReport.Dictionary.Variables.Add("Today", System.DateTime.Now.ToPersianDate());
            oStiReport.Dictionary.Variables.Add("FundName", Utility.CurrentFund.Name);
            oStiReport.Dictionary.Variables.Add("FundManagerName", Utility.CurrentFund.ManagerName);
            oStiReport.Dictionary.Variables.Add("FromDate", varList.Select(current => current.PersianDate).FirstOrDefault());
            oStiReport.Dictionary.Variables.Add("ToDate", varList.Select(current => current.PersianDate).LastOrDefault());
            oStiReport.RegBusinessObject("Transactions", varList);
            oStiReport.Compile();
            oStiReport.RenderWithWpf();
            oStiReport.DoAction(action: reportType, fileName: "گزارش ریز حساب صندوق");
        }

        private void closeButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
