using System.Linq;

namespace Fund
{
    public partial class DelayedInstallmentsListUserControl : System.Windows.Controls.UserControl
    {
        public DelayedInstallmentsListUserControl()
        {
            InitializeComponent();

            RefreshGridControl();
        }

        private void PayInstallmentsButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ViewModels.InstallmentViewModel oViewModel = InstallmentPerLoanGridControl.SelectedItem as ViewModels.InstallmentViewModel;

            if (oViewModel != null)
            {
                DAL.UnitOfWork oUnitOfWork = null;

                try
                {
                    oUnitOfWork = new DAL.UnitOfWork();

                    Models.Installment oInstallment = oUnitOfWork.InstallmentRepository
                        .GetById(oViewModel.Id);

                    var varList = oUnitOfWork.InstallmentRepository
                        .Get()
                        .Where(current => current.LoanId == oInstallment.LoanId)
                        .Where(current => current.IsPayed == false)
                        .Where(current => current.InstallmentDate < oInstallment.InstallmentDate)
                        .OrderBy(current => current.InstallmentDate)
                        .ToList();

                    if (varList.Count != 0)
                    {
                        Infrastructure.MessageBox.Show
                            (
                                caption: Infrastructure.MessageBox.Caption.Error,
                                text: string.Format("ابتدا بایست قسط تاریخ {0} پرداخت شود.", varList.Select(current => current.InstallmentDate).FirstOrDefault().ToPersianDate())
                            );

                        return;
                    }

                    oInstallment.IsPayed = true;
                    oInstallment.PaymentDate = System.DateTime.Now;

                    oUnitOfWork.InstallmentRepository.Update(oInstallment);

                    Models.Fund oFund = oUnitOfWork.FundRepository
                        .GetById(Utility.CurrentFund.Id);

                    oFund.Balance += oInstallment.PaymentAmount;

                    oUnitOfWork.FundRepository.Update(oFund);

                    Utility.CurrentFund = oFund;

                    Models.Transaction oTransaction = new Models.Transaction();

                    oTransaction.Amount = oInstallment.PaymentAmount;
                    oTransaction.Balance = oFund.Balance;
                    oTransaction.Date = System.DateTime.Now;
                    oTransaction.Description = string.Format("پرداخت قسط به مبلغ {0} ریال از {1}", oInstallment.PaymentAmount, oInstallment.Loan.Member.FullName);
                    oTransaction.TransactionType = Models.TransactionType.Installment;
                    oTransaction.MemberId = oInstallment.Loan.Member.Id;
                    oTransaction.FundId = oFund.Id;

                    oUnitOfWork.TransactionRepository.Insert(oTransaction);

                    Models.Reminder oReminder = oUnitOfWork.RemainderRepository
                        .Get()
                        .Where(current => current.InstallmentId == oInstallment.Id)
                        .FirstOrDefault();

                    if (oReminder != null)
                    {
                        oUnitOfWork.RemainderRepository.Delete(oReminder);
                    }

                    oUnitOfWork.Save();

                    Models.Loan oLoan = oUnitOfWork.LoanRepository
                        .Get()
                        .Where(current => current.Id == oInstallment.LoanId)
                        .Where(current => current.Installments.All(installment => installment.IsPayed == true))
                        .FirstOrDefault();

                    if (oLoan != null)
                    {
                        oLoan.IsPayed = true;
                        oLoan.IsActive = false;

                        oUnitOfWork.LoanRepository.Update(oLoan);

                        oUnitOfWork.Save();
                    }

                    Utility.MainWindow.RefreshUserInterface();
                    (Utility.MainWindow.SthPanel.Children[0] as MainPanelContentUserControl).MiniPersianSchedulerReminder.RefreshMonth();
                    (Utility.MainWindow.SthPanel.Children[0] as MainPanelContentUserControl).RefreshSchedulerListBox();
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
        }

        private void PrintButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ShowReport(Infrastructure.Report.ExportType.Print);
        }

        private void ExportToPdfButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ShowReport(Infrastructure.Report.ExportType.ExportToPDF);
        }

        private void CloseButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Close();
        }

        private void ShowReport(Infrastructure.Report.ExportType reportType)
        {

        }

        private void RefreshGridControl()
        {
            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                var varList = oUnitOfWork.InstallmentRepository
                    .Get()
                    .Where(current => current.Loan.Member.FundId == Utility.CurrentFund.Id)
                    .Where(current => current.IsPayed == false)
                    .Where(current => current.InstallmentDate < System.DateTime.Today)
                    .OrderBy(current => current.InstallmentDate)
                    .Select(current => new ViewModels.InstallmentViewModel()
                    {
                        Id = current.Id,
                        Amount = current.PaymentAmount,
                        InstallmentDate = current.InstallmentDate,
                        PaymentDate = current.PaymentDate,
                        MemberId = current.Loan.MemberId,
                        IsPayed = current.IsPayed,
                    })
                    .ToList();

                InstallmentPerLoanGridControl.ItemsSource = varList;

                oUnitOfWork.Save();
            }
            catch (System.Exception ex)
            {
                Infrastructure.MessageBox.Show(ex.Message);
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

        private void LoanOfInstallmentDetailsButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ViewModels.InstallmentViewModel oInstallmentViewModel = InstallmentPerLoanGridControl.SelectedItem as ViewModels.InstallmentViewModel;

            if (oInstallmentViewModel != null)
            {
                DAL.UnitOfWork oUnitOfWork = null;

                try
                {
                    oUnitOfWork = new DAL.UnitOfWork();

                    System.Guid LoanId = oUnitOfWork.InstallmentRepository
                        .GetById(oInstallmentViewModel.Id).LoanId;

                    Models.Loan oLoan = oUnitOfWork.LoanRepository
                        .GetById(LoanId);

                    oUnitOfWork.Save();

                    LoanDetailsWindow oLoanDetailsWindow = new LoanDetailsWindow(oLoan.Id);

                    oLoanDetailsWindow.ShowDialog();

                }
                catch (System.Exception ex)
                {
                    Infrastructure.MessageBox.Show(ex.Message);
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
        }
    }
}
