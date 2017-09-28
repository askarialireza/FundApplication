using System.Linq;

namespace Fund
{
    public partial class ChangePasswordWindow : DevExpress.Xpf.Core.DXWindow
    {
        private System.Guid UserId;

        public ChangePasswordWindow(System.Guid id)
        {
            InitializeComponent();

            UserId = id;
        }

        private void AcceptButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            #region Error Handling Messages

            if (string.IsNullOrWhiteSpace(PasswordTextBox.Password) == true)
            {

                Infrastructure.MessageBox.Show(caption: Infrastructure.Caption.Error, text: "تکمیل فیلد رمز عبور الزامی می‌باشد.");

                return;
            }

            if (string.IsNullOrWhiteSpace(ConfirmPasswordTextBox.Password) == true)
            {

                Infrastructure.MessageBox.Show(caption: Infrastructure.Caption.Error, text: "تکمیل فیلد تایید رمز عبور الزامی می‌باشد.");

                return;
            }

            if (PasswordTextBox.Password.Trim() != ConfirmPasswordTextBox.Password.Trim())
            {
                Infrastructure.MessageBox.Show
                    (
                        caption: Infrastructure.Caption.Error,
                        text: "رمزهای عبور جدید درج شده با یکدیگر مطابقت ندارند."
                    );

                return;
            }

            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                Models.User oUser = oUnitOfWork.UserRepository
                    .GetById(UserId);

                if (oUser != null)
                {
                    oUser.Password = Dtx.Security.Hashing.GetMD5(PasswordTextBox.Password.Trim());

                    oUnitOfWork.UserRepository.Update(oUser);
                }

                oUnitOfWork.Save();

                System.Windows.MessageBoxResult oResult =
                    Infrastructure.MessageBox.Show(caption: Infrastructure.Caption.Information, text: "رمز عبور کاربر با موفقیت به روز رسانی شد.");

                if(oResult == System.Windows.MessageBoxResult.OK)
                {
                    //((ForgetLoginPasswordWindow)this.Parent).Close();

                    this.Close();
                }
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

            #endregion
        }

        private void CancelButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
