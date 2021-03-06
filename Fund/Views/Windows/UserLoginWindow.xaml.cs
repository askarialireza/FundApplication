﻿using System.Linq;

namespace Fund
{
    public partial class UserLoginWindow : System.Windows.Window
    {
        public bool ShowWelcomeWindow { get; set; }

        public UserLoginWindow()
        {
            InitializeComponent();

            ShowWelcomeWindow = true;
        }

        private void LoginClick(object sender, System.Windows.RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(usernameTextBox.Text) == true)
            {

                Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: "تکمیل فیلد نام کاربری الزامی می‌باشد.");

                return;
            }

            if (string.IsNullOrWhiteSpace(passwordTextBox.Password) == true)
            {

                Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: "تکمیل فیلد رمز عبور الزامی می‌باشد.");

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

                    Infrastructure.MessageBox.Show
                        (
                            caption: Infrastructure.MessageBox.Caption.Information,
                            text: "اطلاعات وارد شده صحیح می‌باشد." + System.Environment.NewLine + "خوش آمدید."
                        );

                    oUser.LastLoginTime = System.DateTime.Now;

                    Utility.CurrentUser = oUser;

                    oUnitOfWork.Save();

                    if (ShowWelcomeWindow == true)
                    {
                        WelcomeWindow oWelcomeWindow = new WelcomeWindow();

                        oWelcomeWindow.Show();

                        this.Hide();
                    }
                    else
                    {
                        MainWindow oMainWindow = new MainWindow();

                        oMainWindow.Show();

                        oMainWindow.WindowState = System.Windows.WindowState.Maximized;

                        this.Hide();
                    }
                }

                else
                {
                    Infrastructure.MessageBox.Show
                        (
                            caption: Infrastructure.MessageBox.Caption.Error,
                            text: "نام کاربری و / یا رمز عبور صحیح نمی‌باشد." + System.Environment.NewLine + "کاربری با مشخصات وارد شده موجود نمی‌باشد."
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

        private void ExitClick(object sender, System.Windows.RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void WindowLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            usernameTextBox.Focus();
        }

        private void forgetPasswordButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ForgetLoginPasswordWindow oForgotLoginPasswordWindow = new ForgetLoginPasswordWindow();

            oForgotLoginPasswordWindow.Owner = this;

            oForgotLoginPasswordWindow.ShowDialog();
        }
    }
}
