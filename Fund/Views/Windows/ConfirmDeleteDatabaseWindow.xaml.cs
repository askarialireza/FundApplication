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

                Infrastructure.MessageBox.Show
                (
                    caption: Infrastructure.Caption.Error,
                    text: "تکمیل فیلد رمز عبور الزامی است."
                );

                return;
            }

            if (string.IsNullOrWhiteSpace(CaptchaValueTextBox.Text) == true)
            {
                Infrastructure.MessageBox.Show
                (
                    caption: Infrastructure.Caption.Error,
                    text: "تکمیل فیلد درج کد امنیتی الزامی است."
                );

                return;
            }

            if (CaptchaValueTextBox.Text.Trim().ToUpper() != Captcha.CaptchaValue.ToUpper())
            {
                Infrastructure.MessageBox.Show
                (
                    caption: Infrastructure.Caption.Error,
                    text: "کد امنیتی درج شده صحیح نمی‌باشد."
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
                Infrastructure.MessageBox.Show
                    (
                        caption: Infrastructure.Caption.Error,
                        text: "رمز عبور وارد شده صحیح نمی‌باشد !"
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
