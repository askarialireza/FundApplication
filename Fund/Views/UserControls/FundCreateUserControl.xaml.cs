
namespace Fund
{
    public partial class FundCreateUserControl : System.Windows.Controls.UserControl
    {
        public FundCreateUserControl()
        {
            InitializeComponent();

            PercentCheckBox.IsChecked = false;
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
                Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: "تکمیل فیلد نام صندوق الزامی است.");

                return;
            }

            if (string.IsNullOrWhiteSpace(fundManagerTextBox.Text) == true)
            {
                Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: "تکمیل فیلد نام مدیر صندوق الزامی می‌باشد.");

                return;
            }

            if (string.IsNullOrWhiteSpace(fundBuildYearTextBox.Text) == true)
            {
                Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: "تکمیل فیلد سال تاسیس الزامی می‌باشد.");

                return;
            }


            if (string.IsNullOrWhiteSpace(fundBalanceTextBox.Text) == true)
            {
                Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: "تکمیل فیلد تراز صندوق الزامی می‌باشد.");

                return;
            }

            if (string.IsNullOrWhiteSpace(fundRemovalLimitTextBox.Text) == true)
            {
                Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: "تکمیل فیلد سقف برداشت وام الزامی می‌باشد.");

                return;
            }

            if (string.IsNullOrWhiteSpace(passwordBox.Password) == true)
            {
                Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: "تکمیل فیلد رمز عبور الزامی می‌باشد.");

                return;
            }

            if (string.IsNullOrWhiteSpace(againPasswordBox.Password) == true)
            {
                Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: "تکمیل فیلد تکرار رمز عبور الزامی می‌باشد.");

                return;
            }

            if (PercentCheckBox.IsChecked == true)
            {
                if (string.IsNullOrWhiteSpace(PercentTextBox.Text.Trim()) == true)
                {
                    Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: "در صورت فعال بودن گزینه کارمزد، تکمیل فیلد کارمزد الزامی است.");

                    return;
                }
            }

            if (Utility.StringToMoney(fundBalanceTextBox.Text) <= Utility.StringToMoney(fundRemovalLimitTextBox.Text))
            {
                Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: "سقف برداشت نمیتواند مقداری بیشتر از تراز صندوق داشته باشد.");

                return;
            }

            if (passwordBox.Password.Trim() != againPasswordBox.Password.Trim())
            {
                Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: "گذرواژه های درج شده با یکدیگر مطابقت ندارند.");

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
                oFund.Percent = (PercentCheckBox.IsChecked == true) ? System.Convert.ToInt32(PercentTextBox.Text) : 0;

                oUnitOfWork.FundRepository.Insert(oFund);

                oUnitOfWork.Save();
                oUnitOfWork.Dispose();
                oUnitOfWork = null;

                Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Information, text: "صندوق جدید با موفقیت در بانک اطلاعاتی ایجاد گردید.");

                Utility.MainWindow.RefreshUserInterface();
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

        private void ToggleSwitch_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            PercentTextBox.Visibility = System.Windows.Visibility.Visible;
            PercentLabel.Visibility = System.Windows.Visibility.Visible;
        }

        private void ToggleSwitch_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            PercentTextBox.Visibility = System.Windows.Visibility.Hidden;
            PercentLabel.Visibility = System.Windows.Visibility.Hidden;

        }

        private void fundBalanceTextBox_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            ((System.Windows.Controls.TextBox)sender).Text =
                ((System.Windows.Controls.TextBox)sender).Text.Replace(" ریال", string.Empty).Replace(",", string.Empty);
        }

        private void fundBalanceTextBox_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(((System.Windows.Controls.TextBox)sender).Text) == false)
            {
                long value = System.Convert.ToInt64(((System.Windows.Controls.TextBox)sender).Text.Replace(" ریال", string.Empty).Replace(",", string.Empty));

                ((System.Windows.Controls.TextBox)sender).Text = value.ToRialStringFormat();
            }
            else
            {
                long zero = 0;
                ((System.Windows.Controls.TextBox)sender).Text = zero.ToRialStringFormat();
            }
        }
    }
}
