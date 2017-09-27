
namespace Fund
{
    public partial class EnterEmailPasswordWindow : DevExpress.Xpf.Core.DXWindow
    {
        public string EmailPassword { get; set; }

        public EnterEmailPasswordWindow()
        {
            InitializeComponent();

            EmailLabel.Content = Utility.CurrentUser.EmailAddress;
        }

        private void AcceptButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EmailPasswordBox.Password) == true)
            {

                Infrastructure.MessageBox.Show
                (
                    caption: Infrastructure.Caption.Error,
                    text: "تکمیل فیلد رمز عبور الزامی است."
                );

                return;
            }

            EmailPassword = EmailPasswordBox.Password.Trim();

            this.DialogResult = true;

            this.Close();
        }

        private void CancelButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.DialogResult = false;

            this.Close();
        }

        private void DXWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            EmailPasswordBox.Focus();
        }
    }
}
