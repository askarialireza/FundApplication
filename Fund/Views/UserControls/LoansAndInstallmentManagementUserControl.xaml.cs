using System.Linq;
using System.Data.Entity;

namespace Fund
{
    public partial class LoansAndInstallmentManagementUserControl : System.Windows.Controls.UserControl
    {
        public LoansAndInstallmentManagementUserControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            RefreshList();
        }

        private void RefreshList()
        {
            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                var varList = oUnitOfWork.MemberRepository
                    .Get()
                    .Include(current => current.Loans)
                    .Where(current => current.FundId == Utility.CurrentFund.Id)
                    .Where(current => current.Loans.Count != 0)
                    .OrderBy(current => current.FullName.LastName)
                    .ThenBy(current => current.FullName.FirstName)
                    .ToList();

                LoansDataGrid.ItemsSource = varList;

                PayAllInstallmentsButton.IsEnabled = (varList.Count == 0) ? false : true;
                ExportToPdfButton.IsEnabled = (varList.Count == 0) ? false : true;
                PrintButton.IsEnabled = (varList.Count == 0) ? false : true;

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

                    Models.Member oMember = oUnitOfWork.MemberRepository
                        .GetById(oInstallment.Loan.Member.Id);

                    oMember.Balance -= oInstallment.PaymentAmount;

                    oUnitOfWork.MemberRepository.Update(oMember);

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
                    oTransaction.InstallmentId = oInstallment.Id;

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

                RefreshInstallmentGridControl();
            }
        }

        private void LoansTreeView_SelectedItemChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<object> e)
        {
            Models.Loan oLoan = LoansDataGrid.SelectedItem as Models.Loan;

            if (oLoan != null)
            {
                Utility.CurrentLoan = oLoan;

                RefreshInstallmentGridControl();
            }
            else
            {
                return;
            }
        }

        private void PayAllInstallmentsButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            System.ComponentModel.BackgroundWorker oBackgroundWorker = new System.ComponentModel.BackgroundWorker();

            oBackgroundWorker.WorkerReportsProgress = true;

            oBackgroundWorker.DoWork += OBackgroundWorker_DoWork;
            oBackgroundWorker.RunWorkerCompleted += OBackgroundWorker_RunWorkerCompleted;
            oBackgroundWorker.ProgressChanged += OBackgroundWorker_ProgressChanged;

            oBackgroundWorker.RunWorkerAsync();

            Utility.MainWindow.MainProgressBar.IsIndeterminate = false;
            Utility.MainWindow.MainProgressBar.Visibility = System.Windows.Visibility.Visible;
        }

        #region PayAllInstallment BackgroundWorker

        private void OBackgroundWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            Utility.MainWindow.MainProgressBar.Value = e.ProgressPercentage;
        }

        private void OBackgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                Utility.MainWindow.RefreshUserInterface();
                (Utility.MainWindow.SthPanel.Children[0] as MainPanelContentUserControl).MiniPersianSchedulerReminder.RefreshMonth();
                (Utility.MainWindow.SthPanel.Children[0] as MainPanelContentUserControl).RefreshSchedulerListBox();

                Utility.MainWindow.MainProgressBar.IsIndeterminate = false;
                Utility.MainWindow.MainProgressBar.Visibility = System.Windows.Visibility.Hidden;


                Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Information, text: "تمامی اقساط وام مربوطه پرداخت شدند.");

                RefreshInstallmentGridControl();
            });
        }

        private void OBackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                var varList = oUnitOfWork.InstallmentRepository
                    .Get()
                    .Where(current => current.LoanId == Utility.CurrentLoan.Id)
                    .Where(current => current.IsPayed == false)
                    .OrderBy(current => current.InstallmentDate)
                    .ToList();


                if (varList.Count != 0)
                {
                    int count = varList.Count;
                    int counter = 0;

                    foreach (Models.Installment oInstallment in varList)
                    {
                        int progressBarPercent = System.Convert.ToInt32(((double)(counter + 1) / varList.Count) * 100);

                        oInstallment.IsPayed = true;
                        oInstallment.PaymentDate = System.DateTime.Now;

                        oUnitOfWork.InstallmentRepository.Update(oInstallment);

                        Models.Member oMember = oUnitOfWork.MemberRepository
                            .GetById(oInstallment.Loan.Member.Id);

                        oMember.Balance -= oInstallment.PaymentAmount;

                        oUnitOfWork.MemberRepository.Update(oMember);

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
                        oTransaction.InstallmentId = oInstallment.Id;

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

                        (sender as System.ComponentModel.BackgroundWorker).ReportProgress(progressBarPercent);

                        counter++;
                    }
                }

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

        #endregion

        private void DeleteLoanButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            System.ComponentModel.BackgroundWorker oDeleteLoanBackgroundWorker = new System.ComponentModel.BackgroundWorker();

            oDeleteLoanBackgroundWorker.WorkerReportsProgress = true;

            oDeleteLoanBackgroundWorker.DoWork += ODeleteLoanBackgroundWorker_DoWork;
            oDeleteLoanBackgroundWorker.RunWorkerCompleted += ODeleteLoanBackgroundWorker_RunWorkerCompleted;

            oDeleteLoanBackgroundWorker.RunWorkerAsync();

            Utility.MainWindow.MainProgressBar.IsIndeterminate = true;
            Utility.MainWindow.MainProgressBar.Visibility = System.Windows.Visibility.Visible;
        }

        #region DeleteLoan BackgroundWorker

        private void ODeleteLoanBackgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            Utility.MainWindow.MainProgressBar.IsIndeterminate = false;
            Utility.MainWindow.MainProgressBar.Visibility = System.Windows.Visibility.Hidden;

            Infrastructure.MessageBox.Show
                (
                    caption: Infrastructure.MessageBox.Caption.Information,
                    text: "وام ثبت شده در سیستم با موفقیت حذف گردید."
                );

            RefreshList();

            RefreshInstallmentGridControl();

            ExportToPdfButton.IsEnabled = false;
            PrintButton.IsEnabled = false;

            Utility.MainWindow.RefreshUserInterface();

        }

        private void ODeleteLoanBackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                Models.Fund oFund = oUnitOfWork.FundRepository
                    .GetById(Utility.CurrentFund.Id);

                var varList = oUnitOfWork.InstallmentRepository
                    .Get()
                    .Where(current => current.LoanId == Utility.CurrentLoan.Id)
                    .ToList();


                if (varList.Count != 0)
                {
                    foreach (Models.Installment oInstallment in varList)
                    {
                        Models.Transaction oTransaction = oUnitOfWork.TransactionRepository
                            .Get()
                            .Where(current => current.InstallmentId == oInstallment.Id)
                            .FirstOrDefault();

                        Models.Member oMember = oUnitOfWork.MemberRepository
                            .GetById(Utility.CurrentLoan.MemberId);

                        if (oInstallment.IsPayed == true)
                        {
                            oMember.Balance += oInstallment.PaymentAmount;

                            oUnitOfWork.MemberRepository.Update(oMember);
                        }

                        if (oTransaction != null)
                        {
                            oUnitOfWork.TransactionRepository.Delete(oTransaction);
                        }

                        Models.Reminder oReminder = oUnitOfWork.RemainderRepository
                            .Get()
                            .Where(current => current.InstallmentId == oInstallment.Id)
                            .FirstOrDefault();

                        if (oReminder != null)
                        {
                            oUnitOfWork.RemainderRepository.Delete(oReminder);
                        }

                        if (oFund != null)
                        {
                            oFund.Balance -= oInstallment.PaymentAmount;
                        }

                        oUnitOfWork.Save();
                    }

                    oUnitOfWork.FundRepository.Update(oFund);

                    oUnitOfWork.Save();

                    Utility.CurrentFund = oFund;
                }

                Models.Loan oLoan = oUnitOfWork.LoanRepository
                    .GetById(Utility.CurrentLoan.Id);


                if (oLoan != null)
                {
                    Models.Transaction oTransaction = oUnitOfWork.TransactionRepository
                        .Get()
                        .Where(current => current.LoanId == oLoan.Id)
                        .FirstOrDefault();


                    Models.Member oMember = oUnitOfWork.MemberRepository
                        .GetById(Utility.CurrentLoan.Member.Id);

                    oUnitOfWork.LoanRepository.Delete(oLoan);

                    if (oTransaction != null)
                    {
                        oUnitOfWork.TransactionRepository.Delete(oTransaction);
                    }

                    oUnitOfWork.Save();

                    oFund.Balance += oLoan.LoanAmount;
                    oMember.Balance -= oLoan.LoanAmount;

                    oUnitOfWork.FundRepository.Update(oFund);
                    oUnitOfWork.MemberRepository.Update(oMember);
                }

                oUnitOfWork.Save();

                Utility.CurrentFund = oFund;
                Utility.CurrentLoan = null;

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

        #endregion

        private void RefreshInstallmentGridControl()
        {
            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                System.Collections.Generic.List<ViewModels.InstallmentViewModel> varList = null;

                if (Utility.CurrentLoan != null)
                {
                    varList = oUnitOfWork.InstallmentRepository
                        .Get()
                        .Where(current => current.LoanId == Utility.CurrentLoan.Id)
                        .Select(current => new ViewModels.InstallmentViewModel()
                        {
                            Id = current.Id,
                            Amount = current.PaymentAmount,
                            InstallmentDate = current.InstallmentDate,
                            PaymentDate = current.PaymentDate,
                            IsActive = !(current.IsPayed),
                            IsPayed = current.IsPayed,
                        })
                        .OrderBy(current => current.InstallmentDate)
                        .ToList();

                    PayAllInstallmentsButton.IsEnabled = (Utility.CurrentLoan.IsPayed == true) ? false : true;
                    ExportToPdfButton.IsEnabled = (varList.Count == 0) ? false : true;
                    PrintButton.IsEnabled = (varList.Count == 0) ? false : true;
                }
                else
                {
                    varList = null;
                }

                InstallmentPerLoanGridControl.ItemsSource = varList;

                PayAllInstallmentsButton.IsEnabled = (varList != null) ? true : false;
                DeleteLoanButton.IsEnabled = (varList != null) ? true : false;

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

        private void CancelButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Close();
        }

        private void PrintButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ShowReport(Infrastructure.Report.ExportType.Print);
        }

        private void ExportToPdfButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ShowReport(Infrastructure.Report.ExportType.ExportToPDF);
        }

        private void ShowReport(Infrastructure.Report.ExportType reportType)
        {

            if (InstallmentPerLoanGridControl.ItemsSource == null)
            {
                return;
            }
            else
            {
                var varList = (InstallmentPerLoanGridControl.ItemsSource as System.Collections.Generic.List<ViewModels.InstallmentViewModel>)
                    .Select(current => new
                    {
                        current.AmountRialFormat,
                        current.PersianInstallmentDate,
                        current.PersianPaymentDate,
                        current.IsPayedDescription,
                    })
                    .ToList();

                Stimulsoft.Report.StiReport oStiReport = new Stimulsoft.Report.StiReport();

                oStiReport.Load(Properties.Resources.InstallmentListReport);

                oStiReport.Dictionary.Variables.Add("Today", System.DateTime.Now.ToPersianDate());
                oStiReport.Dictionary.Variables.Add("FundName", Utility.CurrentFund.Name);
                oStiReport.Dictionary.Variables.Add("FundManagerName", Utility.CurrentFund.ManagerName);
                oStiReport.Dictionary.Variables.Add("MemberName", Utility.CurrentLoan.Member.FullName.ToString());
                oStiReport.Dictionary.Variables.Add("LoanAmount", (new ViewModels.LoanViewModel { LoanAmount = Utility.CurrentLoan.LoanAmount }).LoanAmountRialFormat);
                oStiReport.Dictionary.Variables.Add("RefundAmount", (new ViewModels.LoanViewModel { RefundAmount = Utility.CurrentLoan.RefundAmount }).RefundAmountRialFormat);

                oStiReport.RegBusinessObject("InstallmentsList", varList);
                oStiReport.Compile();
                oStiReport.RenderWithWpf();
                oStiReport.DoAction(action: reportType, fileName: "گزارش لیست اقساط");
            }

        }

        private void DataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Models.Loan oLoan = ((System.Windows.Controls.DataGrid)sender).SelectedItem as Models.Loan;

            if (oLoan != null)
            {
                Utility.CurrentLoan = oLoan;

                RefreshInstallmentGridControl();
            }
            else
            {
                return;
            }
        }

        private void LoansDataGrid_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
