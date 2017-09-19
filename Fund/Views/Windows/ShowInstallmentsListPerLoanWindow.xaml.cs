﻿using System.Linq;

namespace Fund
{
    public partial class ShowInstallmentsListPerLoanWindow : DevExpress.Xpf.Core.DXWindow
    {

        public ShowInstallmentsListPerLoanWindow()
        {
            InitializeComponent();

            RefreshInstallmentGridControl();
        }

        private void RefreshInstallmentGridControl()
        {
            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                var varList = oUnitOfWork.InstallmentRepository
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

                InstallmentPerLoanGridControl.ItemsSource = varList;

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
                                caption: Infrastructure.MessageBoxCaption.Error,
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

                RefreshInstallmentGridControl();
            }
        }

        private void PrintButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ShowReport(Infrastructure.ReportType.Print);
        }

        private void ExportToPdfButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ShowReport(Infrastructure.ReportType.ExportToPDF);
        }

        private void ShowReport(Infrastructure.ReportType reportType)
        {
            var varList = (InstallmentPerLoanGridControl.ItemsSource as System.Collections.Generic.List<ViewModels.InstallmentViewModel>)
                .Select(current=> new
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
}
