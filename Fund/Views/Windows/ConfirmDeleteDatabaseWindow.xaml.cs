using System.Linq;

namespace Fund
{
    public partial class ConfirmDeleteDatabaseWindow : DevExpress.Xpf.Core.DXWindow
    {
        public ConfirmDeleteDatabaseWindow()
        {
            InitializeComponent();
        }

        private void AcceptButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(passwordBox.Password) == true)
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

            if (string.IsNullOrWhiteSpace(CaptchaValueTextBox.Text) == true)
            {
                DevExpress.Xpf.Core.DXMessageBox.Show
                (
                    caption: "خطا",
                    messageBoxText: "تکمیل فیلد درج کد امنیتی الزامی است.",
                    button: System.Windows.MessageBoxButton.OK,
                    icon: System.Windows.MessageBoxImage.Error,
                    defaultResult: System.Windows.MessageBoxResult.OK
                );

                return;
            }

            if (CaptchaValueTextBox.Text.Trim() != Captcha.CaptchaValue)
            {
                DevExpress.Xpf.Core.DXMessageBox.Show
                (
                    caption: "خطا",
                    messageBoxText: "کد امنیتی درج شده صحیح نمی‌باشد.",
                    button: System.Windows.MessageBoxButton.OK,
                    icon: System.Windows.MessageBoxImage.Error,
                    defaultResult: System.Windows.MessageBoxResult.OK
                );

                return;
            }

            string password = Dtx.Security.Hashing.GetMD5(passwordBox.Password.Trim());

            if (password == Utility.CurrentUser.Password)
            {
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                DevExpress.Xpf.Core.DXMessageBox.Show
                    (
                        caption: "خطا",
                        messageBoxText: "رمز عبور وارد شده صحیح نمی‌باشد !",
                        button: System.Windows.MessageBoxButton.OK,
                        icon: System.Windows.MessageBoxImage.Error,
                        defaultResult: System.Windows.MessageBoxResult.OK,
                        options: System.Windows.MessageBoxOptions.RightAlign| System.Windows.MessageBoxOptions.RtlReading
                    );

                passwordBox.SelectAll();
                passwordBox.Focus();
                return;
            }
        }

        private void CancelButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void DXWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            passwordBox.Focus();
        }
    }
}
