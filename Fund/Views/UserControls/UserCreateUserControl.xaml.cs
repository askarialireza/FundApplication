using System.Linq;

namespace Fund
{
    public partial class UsersCreateUserControl : System.Windows.Controls.UserControl
    {
        public UsersCreateUserControl()
        {
            InitializeComponent();

            UserTypeComboBox.ItemsSource = Infrastructure.UserType.UserTypesList;
        }

        private void CloseClick(object sender, System.Windows.RoutedEventArgs e)
        {
            (this.Parent as System.Windows.Controls.Panel).Children.Remove(this);
        }

        private void AcceptClick(object sender, System.Windows.RoutedEventArgs e)
        {
            #region Error Handling Messages

            if (string.IsNullOrWhiteSpace(UsernameTextBox.Text) == true)
            {

                Infrastructure.MessageBox.Show
                (
                    caption: Infrastructure.MessageBox.Caption.Error,
                    text: "تکمیل فیلد نام کاربری الزامی است."
                );

                return;
            }

            if (string.IsNullOrWhiteSpace(PasswordTextBox.Password) == true)
            {

                Infrastructure.MessageBox.Show
                (
                    caption: Infrastructure.MessageBox.Caption.Error,
                    text: "تکمیل فیلد رمز عبور الزامی است."
                );

                return;
            }

            if (string.IsNullOrWhiteSpace(AgainPasswordTextBox.Password) == true)
            {

                Infrastructure.MessageBox.Show
                (
                    caption: Infrastructure.MessageBox.Caption.Error,
                    text: "تکمیل فیلد تکرار رمز عبور الزامی است."
                );

                return;
            }

            if (string.IsNullOrWhiteSpace(emailAddressTextBox.Text) == true)
            {
                Infrastructure.MessageBox.Show
                (
                    caption: Infrastructure.MessageBox.Caption.Error,
                    text: "تکمیل فیلد پست الکترونیکی الزامی است."
                );

                return;
            }

            if (PasswordTextBox.Password.Trim() != AgainPasswordTextBox.Password.Trim())
            {
                Infrastructure.MessageBox.Show
                (
                    caption: Infrastructure.MessageBox.Caption.Error,
                    text: "رمزهای عبور درج شده با یکدیگر مطابقت ندارند."
                );

                return;
            }

            #endregion

            DAL.UnitOfWork oUnitOfWork = new DAL.UnitOfWork();

            string username = UsernameTextBox.Text.Trim();

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                Models.User oUser = oUnitOfWork.UserRepository
                    .FindUsername(username)
                    .FirstOrDefault();

                if (oUser != null)
                {
                    Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: "این نام کاربری استفاده شده است. از نام کاربری دیگری استفاده نمایید.");

                    return;
                }
                else
                {
                    oUser = new Models.User();

                    oUser.FullName.FirstName = (string.IsNullOrWhiteSpace(FirstNameTextBox.Text) == true) ? string.Empty : FirstNameTextBox.Text.Trim();

                    oUser.FullName.LastName = (string.IsNullOrWhiteSpace(LastNameTextBox.Text) == true) ? string.Empty : LastNameTextBox.Text.Trim();

                    oUser.EmailAddress = emailAddressTextBox.Text.Trim();

                    oUser.LastLoginTime = null;

                    oUser.RegisterationDate = System.DateTime.Now;

                    oUser.Username = username;

                    oUser.Password = Dtx.Security.Hashing.GetMD5(PasswordTextBox.Password.Trim());

                    oUser.IsAdmin = (UserTypeComboBox.SelectedItem as ViewModels.UserTypeViewModel).IsAdmin;

                    Models.UserSetting oUserSetting = new Models.UserSetting();

                    oUserSetting.CanChangeDatabaseBackupPath = oUser.IsAdmin;

                    oUserSetting.DatabaseBackupPath = Utility.DatabaseBackupPath;

                    oUser.UserSetting = oUserSetting;

                    oUnitOfWork.UserRepository.Insert(oUser);

                    oUnitOfWork.Save();

                    Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Information, text: "حساب کاربری جدید با موفقیت ایجاد گردید.");

                }

                oUnitOfWork.Save();
            }
            catch (System.Exception ex)
            {
                Infrastructure.MessageBox.Show(ex.Message); ;
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

        private void emailAddressTextBox_GotKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            EmailPopup.Show();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://myaccount.google.com/lesssecureapps");

            EmailPopup.IsOpen = false;
        }

    }
}
