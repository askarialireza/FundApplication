using System.Linq;

namespace Fund
{
    /// <summary>
    /// Interaction logic for CreateFundUserControl.xaml
    /// </summary>
    public partial class FundCreateUserControl : System.Windows.Controls.UserControl
    {
        public FundCreateUserControl()
        {
            InitializeComponent();

        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            FarsiLibrary.Utils.PersianDate oPersianDate = FarsiLibrary.Utils.PersianDate.Today;

            fundBuildYearTextBox.Text = oPersianDate.Year.ToString();
        }

        private void CloseClick(object sender, System.Windows.RoutedEventArgs e)
        {
            (this.Parent as System.Windows.Controls.Panel).Children.Remove(this);
            Utility.MainWindow.RefreshUserInterface();
        }

        private void AcceptClick(object sender, System.Windows.RoutedEventArgs e)
        {
            #region Error Handling Messages

            if (string.IsNullOrWhiteSpace(fundNameTextBox.Text) == true)
            {

                DevExpress.Xpf.Core.DXMessageBox.Show
                (
                    caption: "خطا",
                    messageBoxText: "تکمیل فیلد نام صندوق الزامی است.",
                    button: System.Windows.MessageBoxButton.OK,
                    icon: System.Windows.MessageBoxImage.Error,
                    defaultResult: System.Windows.MessageBoxResult.OK
                );

                return;
            }

            if (string.IsNullOrWhiteSpace(fundManagerTextBox.Text) == true)
            {

                DevExpress.Xpf.Core.DXMessageBox.Show
                (
                    caption: "خطا",
                    messageBoxText: "تکمیل فیلد نام مدیر صندوق الزامی می‌باشد.",
                    button: System.Windows.MessageBoxButton.OK,
                    icon: System.Windows.MessageBoxImage.Error,
                    defaultResult: System.Windows.MessageBoxResult.OK
                );

                return;
            }

            if (string.IsNullOrWhiteSpace(fundBuildYearTextBox.Text) == true)
            {
                DevExpress.Xpf.Core.DXMessageBox.Show
                (
                    caption: "خطا",
                    messageBoxText: "تکمیل فیلد سال تاسیس الزامی می‌باشد.",
                    button: System.Windows.MessageBoxButton.OK,
                    icon: System.Windows.MessageBoxImage.Error,
                    defaultResult: System.Windows.MessageBoxResult.OK
                );
                return;
            }


            if (string.IsNullOrWhiteSpace(fundBalanceTextBox.Text) == true)
            {
                DevExpress.Xpf.Core.DXMessageBox.Show
                (
                    caption: "خطا",
                    messageBoxText: "تکمیل فیلد تراز صندوق الزامی می‌باشد.",
                    button: System.Windows.MessageBoxButton.OK,
                    icon: System.Windows.MessageBoxImage.Error,
                    defaultResult: System.Windows.MessageBoxResult.OK
                );

                return;
            }

            if (string.IsNullOrWhiteSpace(fundRemovalLimitTextBox.Text) == true)
            {
                DevExpress.Xpf.Core.DXMessageBox.Show
                (
                    caption: "خطا",
                    messageBoxText: "تکمیل فیلد سقف برداشت وام الزامی می‌باشد.",
                    button: System.Windows.MessageBoxButton.OK,
                    icon: System.Windows.MessageBoxImage.Error,
                    defaultResult: System.Windows.MessageBoxResult.OK
                );

                return;
            }

            if (string.IsNullOrWhiteSpace(passwordBox.Password) == true)
            {
                DevExpress.Xpf.Core.DXMessageBox.Show
                (
                    caption: "خطا",
                    messageBoxText: "تکمیل فیلد رمز عبور الزامی می‌باشد.",
                    button: System.Windows.MessageBoxButton.OK,
                    icon: System.Windows.MessageBoxImage.Error,
                    defaultResult: System.Windows.MessageBoxResult.OK
                );
                return;
            }

            if (string.IsNullOrWhiteSpace(againPasswordBox.Password) == true)
            {
                DevExpress.Xpf.Core.DXMessageBox.Show
                (
                    caption: "خطا",
                    messageBoxText: "تکمیل فیلد تکرار رمز عبور الزامی می‌باشد.",
                    button: System.Windows.MessageBoxButton.OK,
                    icon: System.Windows.MessageBoxImage.Error,
                    defaultResult: System.Windows.MessageBoxResult.OK
                );
                return;
            }

            if (Utility.StringToMoney(fundBalanceTextBox.Text) <= Utility.StringToMoney(fundRemovalLimitTextBox.Text))
            {
                DevExpress.Xpf.Core.DXMessageBox.Show
                (
                    caption: "خطا",
                    messageBoxText: "سقف برداشت نمیتواند مقداری بیشتر از تراز صندوق داشته باشد.",
                    button: System.Windows.MessageBoxButton.OK,
                    icon: System.Windows.MessageBoxImage.Error,
                    defaultResult: System.Windows.MessageBoxResult.OK
                );

                return;
            }

            if (passwordBox.Password.Trim() != againPasswordBox.Password.Trim())
            {
                DevExpress.Xpf.Core.DXMessageBox.Show
                (
                    caption: "خطا",
                    messageBoxText: "گذرواژه های درج شده با یکدیگر مطابقت ندارند.",
                    button: System.Windows.MessageBoxButton.OK,
                    icon: System.Windows.MessageBoxImage.Error,
                    defaultResult: System.Windows.MessageBoxResult.OK
                );

                return;
            }

            #endregion

            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                Models.Fund oFund = new Models.Fund();

                oFund.Name = fundNameTextBox.Text.Trim();
                oFund.ManagerName = fundManagerTextBox.Text.Trim();
                oFund.FoundationYear = System.Convert.ToInt32(fundBuildYearTextBox.Text);
                oFund.Balance = Utility.StringToMoney(fundBalanceTextBox.Text);
                oFund.UserId = Utility.CurrentUser.Id;
                oFund.RemovalLimit = Utility.StringToMoney(fundRemovalLimitTextBox.Text);
                oFund.ManagerPassword = Dtx.Security.Hashing.GetMD5(passwordBox.Password.Trim());

                oUnitOfWork.FundRepository.Insert(oFund);

                oUnitOfWork.Save();
                oUnitOfWork.Dispose();
                oUnitOfWork = null;

                DevExpress.Xpf.Core.DXMessageBox.Show
                (
                    caption: "پیغام",
                    messageBoxText: "صندوق جدید با موفقیت در بانک اطلاعاتی ایجاد گردید.",
                    button: System.Windows.MessageBoxButton.OK,
                    icon: System.Windows.MessageBoxImage.Information,
                    defaultResult: System.Windows.MessageBoxResult.OK
                );

                Utility.MainWindow.RefreshUserInterface();
            }
            catch (System.Exception ex)
            {
                DevExpress.Xpf.Core.DXMessageBox.Show(ex.Message);
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

        private void TextBoxEditorActivated(object sender, System.Windows.RoutedEventArgs e)
        {
            ((DevExpress.Xpf.Editors.TextEdit)sender).Text =
                ((DevExpress.Xpf.Editors.TextEdit)sender).Text.Replace(" ریال", string.Empty).Replace(",", string.Empty);
        }

        private void TextBoxValidate(object sender, DevExpress.Xpf.Editors.ValidationEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(((DevExpress.Xpf.Editors.TextEdit)sender).Text) == false)
            {
                long value = System.Convert.ToInt64(((DevExpress.Xpf.Editors.TextEdit)sender).Text.Replace(" ریال", string.Empty).Replace(",", string.Empty));

                ((DevExpress.Xpf.Editors.TextEdit)sender).Text = value.ToRialStringFormat();
            }
            else
            {
                long zero = 0;
                ((DevExpress.Xpf.Editors.TextEdit)sender).Text = zero.ToRialStringFormat();
            }
        }
    }
}
