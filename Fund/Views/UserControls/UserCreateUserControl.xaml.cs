using System.Linq;

namespace Fund
{
    public partial class UsersCreateUserControl : System.Windows.Controls.UserControl
    {
        public UsersCreateUserControl()
        {
            InitializeComponent();
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

                DevExpress.Xpf.Core.DXMessageBox.Show
                (
                    caption: "خطا",
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
                    caption: "خطا",
                    messageBoxText: "تکمیل فیلد رمز عبور الزامی است.",
                    button: System.Windows.MessageBoxButton.OK,
                    icon: System.Windows.MessageBoxImage.Error,
                    defaultResult: System.Windows.MessageBoxResult.OK
                );

                return;
            }

            if (string.IsNullOrWhiteSpace(AgainPasswordTextBox.Password) == true)
            {

                DevExpress.Xpf.Core.DXMessageBox.Show
                (
                    caption: "خطا",
                    messageBoxText: "تکمیل فیلد تکرار رمز عبور الزامی است.",
                    button: System.Windows.MessageBoxButton.OK,
                    icon: System.Windows.MessageBoxImage.Error,
                    defaultResult: System.Windows.MessageBoxResult.OK
                );

                return;
            }

            if (PasswordTextBox.Password.Trim() != AgainPasswordTextBox.Password.Trim())
            {
                DevExpress.Xpf.Core.DXMessageBox.Show
                (
                    caption: "خطا",
                    messageBoxText: "رمزهای عبور درج شده با یکدیگر مطابقت ندارند.",
                    button: System.Windows.MessageBoxButton.OK,
                    icon: System.Windows.MessageBoxImage.Error,
                    defaultResult: System.Windows.MessageBoxResult.OK
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
                    DevExpress.Xpf.Core.DXMessageBox.Show
                    (
                        caption: "خطا",
                        messageBoxText: "این نام کاربری استفاده شده است. از نام کاربری دیگری استفاده نمایید.",
                        button: System.Windows.MessageBoxButton.OK,
                        icon: System.Windows.MessageBoxImage.Error,
                        defaultResult: System.Windows.MessageBoxResult.OK
                    );

                    return;
                }
                else
                {
                    Models.User newUser = new Models.User();

                    newUser.FullName.FirstName = (string.IsNullOrWhiteSpace(FirstNameTextBox.Text) == true) ? string.Empty : FirstNameTextBox.Text.Trim();
                    newUser.FullName.LastName = (string.IsNullOrWhiteSpace(LastNameTextBox.Text) == true) ? string.Empty : LastNameTextBox.Text.Trim();
                    newUser.LastLoginTime = System.DateTime.Now;
                    newUser.PersianLastLoginTime = System.DateTime.Now.ToPersianDateTime();
                    newUser.IsAdminToString = (newUser.IsAdmin == true) ? "کاربر مدیر" : "کاربر عادی";
                    newUser.Username = username;
                    newUser.Password = Dtx.Security.Hashing.GetMD5(PasswordTextBox.Password.Trim());
                    newUser.IsAdmin = (UserTypeComboBox.SelectedIndex == 0) ? false : true;

                    Models.UserSetting oUserSetting = new Models.UserSetting();

                    oUserSetting.CanChangeDatabaseBackupPath = newUser.IsAdmin;
                    oUserSetting.DatabaseBackupPath = Utility.DatabaseBackupPath;

                    newUser.UserSetting = oUserSetting;

                    oUnitOfWork.UserRepository.Insert(newUser);

                    oUnitOfWork.Save();

                    DevExpress.Xpf.Core.DXMessageBox.Show
                    (
                        caption: "پیغام",
                        messageBoxText: "حساب کاربری جدید با موفقیت ایجاد گردید.",
                        button: System.Windows.MessageBoxButton.OK,
                        icon: System.Windows.MessageBoxImage.Information,
                        defaultResult: System.Windows.MessageBoxResult.OK
                    );

                }

                oUnitOfWork.Save();
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
    }
}
