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

                        LastPaymentDate = installment
                                            .Where(current => current.InstallmentDate >= System.DateTime.Now)
                                            .OrderBy(current => current.InstallmentDate)
                                            .Select(current=>current.InstallmentDate)
                                            .FirstOrDefault(),
                    })
                    .ToList();


                DebtorsListGridControl.ItemsSource = varList;

                oUnitOfWork.Save();

                long sum = 0;
                 sum = varList
                    .Select(current => current.DebtAmount)
                    .Sum();

                DevExpress.Xpf.Grid.GridSummaryItem oItem = new DevExpress.Xpf.Grid.GridSummaryItem();

                oItem.Alignment = DevExpress.Xpf.Grid.GridSummaryItemAlignment.Right;
                oItem.DisplayFormat = string.Format("مجموع بدهی افراد به صندوق {0}",sum.ToRialStringFormat());

                DebtorsListGridControl.TotalSummary.Add(oItem);
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

        }

        private void ExportToPdfButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void closeButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            System.Windows.Controls.Panel oPanel = this.Parent as System.Windows.Controls.Panel;
            oPanel.Children.Remove(this);
        }
    }
}
