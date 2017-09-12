using System.Linq;

namespace Fund
{
    /// <summary>
    /// Interaction logic for FundLoginWindow.xaml
    /// </summary>
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
                DevExpress.Xpf.Core.DXMessageBox.Show
                    (
                        caption: Infrastructure.MessageBoxCaption.Information,
                        messageBoxText: "ورود به صندوق با موفقیت انجام گردید.",
                        button: System.Windows.MessageBoxButton.OK,
                        icon: System.Windows.MessageBoxImage.Information,
                        defaultResult: System.Windows.MessageBoxResult.OK,
                        options: System.Windows.MessageBoxOptions.RightAlign | System.Windows.MessageBoxOptions.RtlReading
                    );

                Utility.CurrentFund = selectedFund;
                Utility.MainWindow.RefreshUserInterface();
                this.Close();
            }
            else
            {
                DevExpress.Xpf.Core.DXMessageBox.Show
                    (
                        caption: Infrastructure.MessageBoxCaption.Error,
                        messageBoxText: "رمز عبور وارد شده صحیح نمی‌باشد.",
                        button: System.Windows.MessageBoxButton.OK,
                        icon: System.Windows.MessageBoxImage.Error,
                        defaultResult: System.Windows.MessageBoxResult.OK,
                        options: System.Windows.MessageBoxOptions.RightAlign | System.Windows.MessageBoxOptions.RtlReading
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
