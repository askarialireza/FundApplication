using System.Linq;

namespace Fund
{
    public partial class ForgetLoginPasswordWindow : System.Windows.Window
    {
        private string EmailRecoveryCode;

        private System.Guid Id;

        public ForgetLoginPasswordWindow()
        {
            InitializeComponent();
        }

        private void ConfirmCodeButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            #region Error Handling Messages

            if (string.IsNullOrWhiteSpace(SentCodeTextBox.Text) == true)
            {

                Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: "تکمیل فیلد کد بازیابی الزامی است.");

                return;
            }

            #endregion

            if (string.Compare(EmailRecoveryCode, SentCodeTextBox.Text.Trim(), true) == 0)
            {
                ChangePasswordWindow oChangePasswordWindow = new Fund.ChangePasswordWindow(Id);

                oChangePasswordWindow.Owner = this;

                oChangePasswordWindow.ShowDialog();

                this.Hide();
            }
            else
            {
                Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: "کد بازیابی درج شده اشتباه می‌باشد.");

                return;
            }
        }

        private void SendCodeButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            #region Error Handling Messages

            if (string.IsNullOrWhiteSpace(UsernameTextBox.Text) == true)
            {

                Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: "تکمیل فیلد نام کاربری الزامی است.");

                return;
            }

            if (string.IsNullOrWhiteSpace(EmailAddressTextBox.Text) == true)
            {

                Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: "تکمیل فیلد پست الکترونیکی الزامی است.");

                return;
            }

            #endregion

            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                string username = UsernameTextBox.Text.Trim();

                Models.User oUser = oUnitOfWork.UserRepository
                    .FindUsername(username: username)
                    .FirstOrDefault();

                if (oUser == null)
                {
                    Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: "نام کاربری و / یا پست الکترونیکی درج شده صحیح نمی‌باشد.");

                    return;
                }
                else
                {
                    string emailAddress = EmailAddressTextBox.Text.Trim();

                    if (string.Compare(emailAddress, oUser.EmailAddress, true) == 0)
                    {
                        RefreshCode();

                        Utility.SendEmail
                            (
                                senderEmail: "askarialireza373@gmail.com",
                                senderPassword: "aakh1373181299216alireza",
                                displayName: "Alireza Askari",
                                receiverEmail: oUser.EmailAddress,
                                subject: "برنامه مدیریت صندوق قرض الحسنه خانوادگی | بازیابی رمز عبور",
                                body: "کد بازیابی برای کاربر " + oUser.FullName.ToString() + " : " 
                                    + System.Environment.NewLine
                                    + EmailRecoveryCode
                                    + System.Environment.NewLine +
                                    new FarsiLibrary.Utils.PersianDate(System.DateTime.Now).ToString(),
                                attachment: null,
                                attachmentName: null
                            );

                        Id = oUser.Id;
                    }
                    else
                    {
                        Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: "نام کاربری و / یا پست الکترونیکی درج شده صحیح نمی‌باشد.");

                        return;
                    }
                }

                oUnitOfWork.Save();
            }
            catch (System.Exception ex)
            {
                Infrastructure.MessageBox.Show(ex.Message);
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

        private void RefreshCode()
        {
            EmailRecoveryCode = Dtx.Guid.NewGuidWithoutDash;
        }

        private void EmailAddressTextBox_PreviewLostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            Infrastructure.Validation.EmailAddressValidation(sender, e);
        }
    }
}
