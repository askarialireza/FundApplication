using System.Linq;

namespace Fund
{
    public partial class CreateAdminWindow : DevExpress.Xpf.Core.DXWindow
    {
        public CreateAdminWindow()
        {
            InitializeComponent();
        }

        private void Captcha_CaptchaRefreshed(object sender, System.EventArgs e)
        {
            FirstNameTextBox.Focus();
            CaptchaValueTextBox.Clear();
        }

        private void SimpleButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            #region Error Handling Messages

            if (string.IsNullOrWhiteSpace(UsernameTextBox.Text) == true)
            {

                DevExpress.Xpf.Core.DXMessageBox.Show
                (
                    caption: Infrastructure.MessageBoxCaption.Error,
                    messageBoxText: "تکمیل فیلد نام کاربری الزامی است.",
                    button: System.Windows.MessageBoxButton.OK,
                    icon: System.Windows.MessageBoxImage.Error,
                    defaultResult: System.Windows.MessageBoxResult.OK
                );

                return;
            }

            if (string.IsNullOrWhiteSpace(PasswordTextBox.Password) == true)
            {

                DevExpress.Xpf.Core.DXMessageBox.Show
                (
                    caption: Infrastructure.MessageBoxCaption.Error,
                    messageBoxText: "تکمیل فیلد رمز عبور الزامی است.",
                    button: System.Windows.MessageBoxButton.OK,
                    icon: System.Windows.MessageBoxImage.Error,
                    defaultResult: System.Windows.MessageBoxResult.OK
                );

                return;
            }

            if (string.IsNullOrWhiteSpace(ConfirmPasswordTextBox.Password) == true)
            {

                DevExpress.Xpf.Core.DXMessageBox.Show
                (
                    caption: Infrastructure.MessageBoxCaption.Error,
                    messageBoxText: "تکمیل فیلد تایید رمز عبور الزامی است.",
                    button: System.Windows.MessageBoxButton.OK,
                    icon: System.Windows.MessageBoxImage.Error,
                    defaultResult: System.Windows.MessageBoxResult.OK
                );

                return;
            }

            if (PasswordTextBox.Password.Trim() != ConfirmPasswordTextBox.Password.Trim())
            {
                DevExpress.Xpf.Core.DXMessageBox.Show
                (
                    caption: Infrastructure.MessageBoxCaption.Error,
                    messageBoxText: "رمزهای عبور درج شده با یکدیگر مطابقت ندارند.",
                    button: System.Windows.MessageBoxButton.OK,
                    icon: System.Windows.MessageBoxImage.Error,
                    defaultResult: System.Windows.MessageBoxResult.OK
                );

                return;
            }

            if (string.IsNullOrWhiteSpace(CaptchaValueTextBox.Text) == true)
            {
                DevExpress.Xpf.Core.DXMessageBox.Show
                (
                    caption: Infrastructure.MessageBoxCaption.Error,
                    messageBoxText: "تکمیل فیلد درج کد امنیتی الزامی است.",
                    button: System.Windows.MessageBoxButton.OK,
                    icon: System.Windows.MessageBoxImage.Error,
                    defaultResult: System.Windows.MessageBoxResult.OK
                );

                return;
            }

            if (CaptchaValueTextBox.Text.Trim()!= Captcha.CaptchaValue)
            {
                DevExpress.Xpf.Core.DXMessageBox.Show
                (
                    caption: Infrastructure.MessageBoxCaption.Error,
                    messageBoxText: "کد امنیتی درج شده صحیح نمی‌باشد.",
                    button: System.Windows.MessageBoxButton.OK,
                    icon: System.Windows.MessageBoxImage.Error,
                    defaultResult: System.Windows.MessageBoxResult.OK
                );

                return;
            }

            #endregion

            #region Transaction

            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                Models.User oUser = new Models.User();

                oUser.FullName.FirstName = FirstNameTextBox.Text.Trim();
                oUser.FullName.LastName = LastNameTextBox.Text.Trim();
                oUser.Username = UsernameTextBox.Text.Trim();
                oUser.Password = Dtx.Security.Hashing.GetMD5(PasswordTextBox.Password.Trim());
                oUser.LastLoginTime = System.DateTime.Now;
                oUser.PersianLastLoginTime = System.DateTime.Now.ToPersianDateTime();
                oUser.IsAdmin = true;
                oUser.IsAdminToString = (oUser.IsAdmin == true) ? "کاربر مدیر" : "کاربر عادی";
                oUser.CanBeDeleted = false;

                Models.UserSetting oUserSetting = new Models.UserSetting();

                oUserSetting.CanChangeDatabaseBackupPath = true;
                oUserSetting.DatabaseBackupPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + @"\Fund\Backups\";
                oUserSetting.PersianCalendarHijriAdjustment = -1;

                oUser.UserSetting = oUserSetting;

                oUnitOfWork.UserRepository.Insert(oUser);

                oUnitOfWork.Save();

                Utility.AdminUserId = oUser.Id;

                UserLoginWindow oUserLoginWindow = new UserLoginWindow();

                this.Hide();

                oUserLoginWindow.Show();
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

            #endregion
        }

        private void DXWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            App.Current.Shutdown();
        }
    }
}
