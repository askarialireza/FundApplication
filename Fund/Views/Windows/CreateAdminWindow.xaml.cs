using System.Linq;

namespace Fund
{
    public partial class CreateAdminWindow : System.Windows.Window
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

                Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: "تکمیل فیلد نام کاربری الزامی است.");

                return;
            }

            if (string.IsNullOrWhiteSpace(PasswordTextBox.Password) == true)
            {

                Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: "تکمیل فیلد رمز عبور الزامی است.");

                return;
            }

            if (string.IsNullOrWhiteSpace(EmailAddressTextBox.Text) == true)
            {

                Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: "تکمیل فیلد پست الکترونیکی الزامی است.");

                return;
            }

            if (string.IsNullOrWhiteSpace(ConfirmPasswordTextBox.Password) == true)
            {

                Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: "تکمیل فیلد تایید رمز عبور الزامی است.");

                return;
            }

            if (PasswordTextBox.Password.Trim() != ConfirmPasswordTextBox.Password.Trim())
            {
                Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: "رمزهای عبور درج شده با یکدیگر مطابقت ندارند.");

                return;
            }

            if (string.IsNullOrWhiteSpace(CaptchaValueTextBox.Text) == true)
            {
                Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: "تکمیل فیلد درج کد امنیتی الزامی است.");

                return;
            }

            if (CaptchaValueTextBox.Text.Trim().ToUpper() != Captcha.CaptchaValue.ToUpper())
            {
                Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: "کد امنیتی درج شده صحیح نمی‌باشد.");

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
                oUser.Password = Dtx.Security.Hashing.GetMD5(PasswordTextBox.Password);
                oUser.EmailAddress = EmailAddressTextBox.Text.Trim();
                oUser.LastLoginTime = null ;
                oUser.RegisterationDate = System.DateTime.Now;
                oUser.IsAdmin = true;
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

            #endregion
        }

        private void DXWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://myaccount.google.com/lesssecureapps");

            EmailPopup.IsOpen = false;
        }

        private void EmailAddressTextBox_GotKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            EmailPopup.Show();
        }
    }
}
