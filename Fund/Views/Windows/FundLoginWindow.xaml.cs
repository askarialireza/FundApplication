using System.Linq;

namespace Fund
{
    public partial class FundLoginWindow : DevExpress.Xpf.Core.DXWindow
    {
        public FundLoginWindow()
        {
            InitializeComponent();
        }

        private void WindowLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            passwordBox.Focus();

            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                var fundslist = oUnitOfWork.FundRepository
                    .GetFundsByUser(Utility.CurrentUser)
                    .ToList();

                FundsComboBox.ItemsSource = fundslist;
                FundsComboBox.DisplayMemberPath = "Name";

                oUnitOfWork.Save();
                oUnitOfWork.Dispose();
                oUnitOfWork = null;
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

        private void LoginFundClick(object sender, System.Windows.RoutedEventArgs e)
        {
            Models.Fund selectedFund = FundsComboBox.SelectedItem as Models.Fund;

            string password = Dtx.Security.Hashing.GetMD5(passwordBox.Password);

            if (selectedFund.ManagerPassword == password)
            {
                Infrastructure.MessageBox.Show
                    (
                        caption: Infrastructure.MessageBox.Caption.Information,
                        text: "ورود به صندوق با موفقیت انجام گردید."
                    );

                Utility.CurrentFund = selectedFund;
                Utility.MainWindow.RefreshUserInterface();
                this.Close();
            }
            else
            {
                Infrastructure.MessageBox.Show
                    (
                        caption: Infrastructure.MessageBox.Caption.Error,
                        text: "رمز عبور وارد شده صحیح نمی‌باشد."
                    );

                return;
            }
        }

        private void CancelClick(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Close();
        }
    }

}
