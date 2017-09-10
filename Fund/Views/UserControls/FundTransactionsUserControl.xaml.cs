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
                    .OrderBy(current=>current.Date)
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

        private void SearchButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            #region Error Handling Messages

            if(toDatePicker.SelectedDateTime < fromDatePicker.SelectedDateTime)
            {
                Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBoxCaption.Error, text: "بازه زمانی درج شده نامعتبر می باشد.");

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

        private void ShowAllButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            LoadGridControl();
        }
    }
}
