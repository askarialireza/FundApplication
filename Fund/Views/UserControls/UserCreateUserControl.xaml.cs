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

            if (string.IsNullOrWhiteSpace(AgainPasswordTextBox.Password) == true)
            {

                DevExpress.Xpf.Core.DXMessageBox.Show
                (
                    caption: Infrastructure.MessageBoxCaption.Error,
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
                    caption: Infrastructure.MessageBoxCaption.Error,
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
                    Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBoxCaption.Error, text: "این نام کاربری استفاده شده است. از نام کاربری دیگری استفاده نمایید.");

                    return;
                }
                else
                {
                    oUser = new Models.User();

                    oUser.FullName.FirstName = (string.IsNullOrWhiteSpace(FirstNameTextBox.Text) == true) ? string.Empty : FirstNameTextBox.Text.Trim();
                    oUser.FullName.LastName = (string.IsNullOrWhiteSpace(LastNameTextBox.Text) == true) ? string.Empty : LastNameTextBox.Text.Trim();
                    oUser.LastLoginTime = System.DateTime.Now;
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

                    Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBoxCaption.Information, text: "حساب کاربری جدید با موفقیت ایجاد گردید.");

                }

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
    }
}
