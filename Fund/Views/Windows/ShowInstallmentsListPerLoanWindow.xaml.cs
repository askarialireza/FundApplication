using System.Linq;

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
                        InstallmentDate = current.PersianInstallmentDate,
                        IsActive = !(current.IsPayed),
                        IsPayed = current.IsPayed,
                    })
                    .OrderBy(current=>current.InstallmentDate)
                    .ToList();

                InstallmentPerLoanGridControl.ItemsSource = varList;

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
                    oTransaction.Description = string.Format("پرداخت قسط به مبلغ {0} ریال از {1}", oInstallment.PaymentAmount,Utility.CurrentMember.FullName);
                    oTransaction.TransactionType = Models.TransactionType.Installment;
                    oTransaction.MemberId = Utility.CurrentMember.Id;
                    oTransaction.FundId = oFund.Id;

                    oUnitOfWork.TransactionRepository.Insert(oTransaction);

                    oUnitOfWork.Save();

                    Utility.MainWindow.RefreshUserInterface();
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

                RefreshInstallmentGridControl();
            }
        }
    }
}
