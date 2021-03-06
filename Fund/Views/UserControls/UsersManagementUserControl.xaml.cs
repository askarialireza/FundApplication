﻿using System.Linq;
using Infrastructure;

namespace Fund
{
    public partial class UsersManagementUserControl : System.Windows.Controls.UserControl
    {
        private System.Guid _currentId;

        public UsersManagementUserControl()
        {
            InitializeComponent();

            UserTypeComboBox.ItemsSource = Infrastructure.UserType.UserTypesList;

            EditItemsToggleSwitch.IsChecked = false;

            PasswordChangeToggleSwitch.IsChecked = false;

            MainGroupBoxGrid.BlurApply(5);

            ButtonsGrid.BlurApply(5);

            RefreshGridControl();
        }

        private void UserControlLoaded(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void ExportToPDFClick(object sender, System.Windows.RoutedEventArgs e)
        {
            ShowReport(reportType: Infrastructure.Report.ExportType.ExportToPDF);
        }

        private void PrintClick(object sender, System.Windows.RoutedEventArgs e)
        {
            ShowReport(reportType: Infrastructure.Report.ExportType.Print);
        }

        private void CloseClick(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Close();
        }

        private void PasswordGroupBoxSwitchChecked(object sender, System.Windows.RoutedEventArgs e)
        {
            System.Windows.Controls.CheckBox oCheckBox = ((System.Windows.Controls.CheckBox)sender);

            int row = System.Windows.Controls.Grid.GetRow(oCheckBox);

            System.Windows.Controls.RowDefinition oRowDefinition = MainGroupBoxGrid.RowDefinitions[row + 1];

            oRowDefinition.Height = new System.Windows.GridLength(165);
        }

        private void PasswordGroupBoxSwitchUnchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            System.Windows.Controls.CheckBox oCheckBox = ((System.Windows.Controls.CheckBox)sender);

            int row = System.Windows.Controls.Grid.GetRow(oCheckBox);

            System.Windows.Controls.RowDefinition oRowDefinition = MainGroupBoxGrid.RowDefinitions[row + 1];

            oRowDefinition.Height = new System.Windows.GridLength(0);
        }

        private void EditItemsToggleSwitchChecked(object sender, System.Windows.RoutedEventArgs e)
        {
            AcceptButton.IsEnabled = true;
            CancelButton.IsEnabled = true;
            DeleteButton.IsEnabled = true;

            FirstNameLabel.IsEnabled = true;
            LastNameLabel.IsEnabled = true;
            UsernameLabel.IsEnabled = true;
            UserTypeLabel.IsEnabled = true;
            PasswordChangeLabel.IsEnabled = true;
            EmailAddressLabel.IsEnabled = true;

            FirstNameTextBox.IsEnabled = true;
            LastNameTextBox.IsEnabled = true;
            UsernameTextBox.IsEnabled = true;
            EmailAddressTextBox.IsEnabled = true;
            UserTypeComboBox.IsEnabled = true;
            PasswordChangeToggleSwitch.IsEnabled = true;

            MainGroupBoxGrid.BlurDisable();
            ButtonsGrid.BlurDisable();

            DeleteButton.IsEnabled = (Utility.AdminUserId == _currentId) ? false : true;
        }

        private void EditItemsToggleSwitchUnchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            AcceptButton.IsEnabled = false;
            CancelButton.IsEnabled = false;
            DeleteButton.IsEnabled = false;

            PasswordChangeToggleSwitch.IsChecked = false;

            EmailAddressLabel.IsEnabled = false;
            FirstNameLabel.IsEnabled = false;
            LastNameLabel.IsEnabled = false;
            UsernameLabel.IsEnabled = false;
            UserTypeLabel.IsEnabled = false;
            PasswordChangeLabel.IsEnabled = false;

            EmailAddressTextBox.IsEnabled = false;
            FirstNameTextBox.IsEnabled = false;
            LastNameTextBox.IsEnabled = false;
            UsernameTextBox.IsEnabled = false;
            UserTypeComboBox.IsEnabled = false;
            PasswordChangeToggleSwitch.IsEnabled = false;

            MainGroupBoxGrid.BlurApply(5);
            ButtonsGrid.BlurApply(5);

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

            if (string.IsNullOrWhiteSpace(EmailAddressTextBox.Text) == true)
            {
                Infrastructure.MessageBox.Show
                (
                    caption: Infrastructure.MessageBox.Caption.Error,
                    text: "تکمیل فیلد پست الکترونیکی الزامی است."
                );

                return;
            }

            #endregion

            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                Models.User oUser = oUnitOfWork.UserRepository
                    .GetById(_currentId);

                if (oUser != null)
                {
                    oUser.Username = UsernameTextBox.Text.Trim();
                    oUser.FullName.FirstName = FirstNameTextBox.Text.Trim();
                    oUser.FullName.LastName = LastNameTextBox.Text.Trim();
                    oUser.IsAdmin = (UserTypeComboBox.SelectedItem as ViewModels.UserTypeViewModel).IsAdmin;
                    oUser.EmailAddress = EmailAddressTextBox.Text.Trim();

                    if (PasswordChangeToggleSwitch.IsChecked == true)
                    {
                        #region Error Handling Messages

                        if (string.IsNullOrWhiteSpace(OldPasswordTextBox.Password) == true)
                        {
                            Infrastructure.MessageBox.Show
                            (
                                caption: Infrastructure.MessageBox.Caption.Error,
                                text: "تکمیل فیلد رمز عبور فعلی الزامی است."
                            );

                            return;
                        }


                        if (string.IsNullOrWhiteSpace(PasswordTextBox.Password) == true)
                        {
                            Infrastructure.MessageBox.Show
                            (
                                caption: Infrastructure.MessageBox.Caption.Error,
                                text: "تکمیل فیلد رمز عبور جدید الزامی است."
                            );

                            return;
                        }

                        if (string.IsNullOrWhiteSpace(AgainPasswordTextBox.Password) == true)
                        {
                            Infrastructure.MessageBox.Show
                            (
                                caption: Infrastructure.MessageBox.Caption.Error,
                                text: "تکمیل فیلد تکرار رمز عبور جدید الزامی است."
                            );

                            return;
                        }

                        #endregion

                        string password = Dtx.Security.Hashing.GetMD5(OldPasswordTextBox.Password.Trim());

                        if (password != oUser.Password)
                        {
                            Infrastructure.MessageBox.Show
                                (
                                    caption: Infrastructure.MessageBox.Caption.Error,
                                    text: "رمز عبور درج شده صحیح نمی‌باشد."
                                );

                            return;
                        }

                        if (PasswordTextBox.Password.Trim() != AgainPasswordTextBox.Password.Trim())
                        {
                            Infrastructure.MessageBox.Show
                                (
                                    caption: Infrastructure.MessageBox.Caption.Error,
                                    text: "رمزهای عبور جدید درج شده با یکدیگر مطابقت ندارند."
                                );

                            return;
                        }

                        oUser.Password = Dtx.Security.Hashing.GetMD5(PasswordTextBox.Password.Trim());

                    }

                    oUnitOfWork.UserRepository.Update(oUser);
                    oUnitOfWork.Save();

                    Infrastructure.MessageBox.Show
                        (
                            caption: Infrastructure.MessageBox.Caption.Information,
                            text: "مشخصات کاربر با موفقیت ویرایش گردید."
                        );

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

            RefreshGridControl();
        }

        private void CancelClick(object sender, System.Windows.RoutedEventArgs e)
        {
            EditItemsToggleSwitch.IsChecked = false;

            UsersGridControl_SelectionChanged(null, null);
        }

        private void DeleteUserClick(object sender, System.Windows.RoutedEventArgs e)
        {
            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                System.Windows.MessageBoxResult oResult =
                        Infrastructure.MessageBox.Show
                        (
                            caption: Infrastructure.MessageBox.Caption.Question,
                            text: "آیا مطمئن به حذف کاربر از سیستم هستید ؟"
                        );

                if (oResult == System.Windows.MessageBoxResult.Yes)
                {
                    oUnitOfWork.UserRepository.DeleteById(_currentId);

                    oUnitOfWork.Save();

                    Infrastructure.MessageBox.Show
                    (
                        caption: Infrastructure.MessageBox.Caption.Information,
                        text: "کاربر با موفقیت از سیستم حذف گردید."
                    );

                    if (_currentId == Utility.CurrentUser.Id)
                    {
                        UserLoginWindow oUserLoginWindow = new UserLoginWindow();

                        (Utility.MainWindow as MainWindow).Hide();

                        oUserLoginWindow.Show();
                    }
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

            RefreshGridControl();

            UsersGridControl.SelectedIndex = 0;
        }

        private void RefreshGridControl()
        {
            DAL.UnitOfWork oUnitOfWork = new DAL.UnitOfWork();

            var varList = oUnitOfWork.UserRepository
            .Get()
            .OrderBy(current => current.Username)
            .ToList();

            UsersGridControl.ItemsSource = varList;

            ExportToPdfButton.IsEnabled = (varList.Count == 0) ? false : true;
            PrintButton.IsEnabled = (varList.Count == 0) ? false : true;
        }

        private void ShowReport(Infrastructure.Report.ExportType reportType)
        {
            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                var varList = oUnitOfWork.UserRepository
                    .Get()
                    .OrderBy(current => current.Username)
                    .Select(current => new ViewModels.UserViewModel()
                    {
                        Id = current.Id,
                        Username = current.Username,
                        RegisterationDate = current.RegisterationDate,
                        IsAdmin = current.IsAdmin,
                        LastLoginTime = current.LastLoginTime,
                    })
                    .ToList();

                var varListToReport = varList
                    .OrderBy(current => current.Username)
                    .Select(current => new
                    {
                        Username = current.Username,
                        PersianRegisterationDate = current.PersianRegisterationDate,
                        IsAdminDescription = current.IsAdminDescription,
                        PersianLastLoginTime = current.PersianLastLoginTime,
                    })
                    .ToList();

                Stimulsoft.Report.StiReport oStiReport = new Stimulsoft.Report.StiReport();

                oStiReport.Load(Properties.Resources.UsersViewReport);
                oStiReport.Dictionary.Variables.Add("Today", System.DateTime.Now.ToPersianDate());
                oStiReport.RegBusinessObject("Users", varListToReport);
                oStiReport.Compile();
                oStiReport.RenderWithWpf(); oStiReport.WriteToReportRenderingMessages("در حال تهیه گزارش ...");

                oStiReport.DoAction(reportType, "گزارش کاربران");

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

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://myaccount.google.com/lesssecureapps");

            EmailPopup.IsOpen = false;
        }

        private void EmailAddressTextBox_GotKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            EmailPopup.Show();
        }

        private void UsersGridControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Models.User oUser = UsersGridControl.SelectedItem as Models.User;

            if (oUser != null)
            {
                UsernameTextBox.Text = oUser.Username;

                FirstNameTextBox.Text = oUser.FullName.FirstName;

                LastNameTextBox.Text = oUser.FullName.LastName;

                EmailAddressTextBox.Text = oUser.EmailAddress;

                UserTypeComboBox.SelectedItem = (UserTypeComboBox.ItemsSource as System.Collections.Generic.List<ViewModels.UserTypeViewModel>)
                    .Where(current => current.IsAdmin == oUser.IsAdmin)
                    .FirstOrDefault();

                _currentId = oUser.Id;

                DeleteButton.IsEnabled = (EditItemsToggleSwitch.IsChecked == true) ? ((Utility.AdminUserId == _currentId) ? false : true) : false;
            }
        }
    }
}
