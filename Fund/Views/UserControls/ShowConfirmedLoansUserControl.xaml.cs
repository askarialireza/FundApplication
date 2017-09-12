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

        }

        private void PrintButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void LoadGridControl()
        {
            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                var varList = oUnitOfWork.LoanRepository
                    .Get()
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
    }
}
