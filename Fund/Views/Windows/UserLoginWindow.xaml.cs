using System.Linq;

namespace Fund
{
    public partial class UserLoginWindow : DevExpress.Xpf.Core.DXWindow
    {
        public UserLoginWindow()
        {
            InitializeComponent();
        }

        private void LoginClick(object sender, System.Windows.RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(usernameTextBox.Text) == true)
            {
                DevExpress.Xpf.Core.DXMessageBox.Show(
                    caption: "خطا",
                    messageBoxText: "تکمیل فیلد نام کاربری الزامی می‌باشد.",
                    button: System.Windows.MessageBoxButton.OK,
                    icon: System.Windows.MessageBoxImage.Error,
                    defaultResult: System.Windows.MessageBoxResult.OK,
                    options: System.Windows.MessageBoxOptions.RightAlign | System.Windows.MessageBoxOptions.RtlReading
                    );

                return;
            }

            if (string.IsNullOrWhiteSpace(passwordTextBox.Password) == true)
            {
                DevExpress.Xpf.Core.DXMessageBox.Show(
                    caption: "خطا",
                    messageBoxText: "تکمیل فیلد رمز عبور الزامی می‌باشد.",
                    button: System.Windows.MessageBoxButton.OK,
                    icon: System.Windows.MessageBoxImage.Error,
                    defaultResult: System.Windows.MessageBoxResult.OK,
                    options: System.Windows.MessageBoxOptions.RightAlign | System.Windows.MessageBoxOptions.RtlReading
                    );

                return;
            }

            DAL.UnitOfWork oUnitOfWork = null;
            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                string username = usernameTextBox.Text;
                string password = Dtx.Security.Hashing.GetMD5(passwordTextBox.Password);

                Models.User oUser = oUnitOfWork.UserRepository
                    .FindUser(username, password)
                    .FirstOrDefault();

                Utility.AdminUserId = oUnitOfWork.UserRepository
                    .GetAdminUser()
                    .FirstOrDefault()
                    .Id;

                if (oUser != null)
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show
                    (
                        caption: "پیغام",
                        messageBoxText: "اطلاعات وارد شده صحیح می‌باشد." +
                              System.Environment.NewLine +
                              "خوش آمدید.",
                        button: System.Windows.MessageBoxButton.OK,
                        icon: System.Windows.MessageBoxImage.Information,
                        defaultResult: System.Windows.MessageBoxResult.OK,
                        options: System.Windows.MessageBoxOptions.RtlReading | System.Windows.MessageBoxOptions.RightAlign
                    );

                    oUser.LastLoginTime = System.DateTime.Now;
                    oUser.PersianLastLoginTime = System.DateTime.Now.ToPersianDateTime();
                    Utility.CurrentUser = oUser;

                    oUnitOfWork.Save();

                    MainRibbonWindow oMainRibbonWindow = new MainRibbonWindow();
                    oMainRibbonWindow.WindowState = System.Windows.WindowState.Minimized;
                    oMainRibbonWindow.Show();
                    oMainRibbonWindow.WindowState = System.Windows.WindowState.Maximized;

                    this.Hide();

                }

                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show
                    (
                        caption: "خطا",
                        messageBoxText: "نام کاربری و / یا رمز عبور صحیح نمی‌باشد." +
                              System.Environment.NewLine +
                              "کاربری با مشخصات وارد شده موجود نمی‌باشد.",
                        button: System.Windows.MessageBoxButton.OK,
                        icon: System.Windows.MessageBoxImage.Error,
                        defaultResult: System.Windows.MessageBoxResult.OK,
                        options: System.Windows.MessageBoxOptions.RtlReading | System.Windows.MessageBoxOptions.RightAlign
                    );

                    usernameTextBox.Focus();

                    return;
                }

                oUnitOfWork.Save();
                oUnitOfWork.Dispose();
                oUnitOfWork = null;
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

        private void ExitClick(object sender, System.Windows.RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void WindowLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            usernameTextBox.Focus();
        }
    }
}
