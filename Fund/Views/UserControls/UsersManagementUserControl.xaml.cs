using System.Linq;
using Infrastructure;

namespace Fund
{
    public partial class UsersManagementUserControl : System.Windows.Controls.UserControl
    {
        public System.Guid CurrentId { get; set; }
        public UsersManagementUserControl()
        {
            InitializeComponent();
        }

        private void UserControlLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            EditItemsToggleSwitch.IsChecked = false;
            PasswordChangeToggleSwitch.IsChecked = false;

            MainGroupBoxGrid.BlurApply(5);
            ButtonsGrid.BlurApply(5);

            RefreshGridControl();
        }

        private void ExportToPDFClick(object sender, System.Windows.RoutedEventArgs e)
        {

            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                var varList = oUnitOfWork.UserRepository
                    .UsersToReport()
                    .ToList();

                Stimulsoft.Report.StiReport usersReport = new Stimulsoft.Report.StiReport();

                usersReport.Load(System.Environment.CurrentDirectory + "\\Files\\Reports\\UsersViewReport.mrt");
                usersReport.Dictionary.Variables.Add("Today", System.DateTime.Now.ToPersianDate());
                usersReport.RegBusinessObject("Users", varList);
                usersReport.Compile();
                usersReport.RenderWithWpf();
                usersReport.ExportToPdf("لیست کاربران");

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

        private void PrintClick(object sender, System.Windows.RoutedEventArgs e)
        {
            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                var varList = oUnitOfWork.UserRepository
                    .UsersToReport()
                    .ToList();

                Stimulsoft.Report.StiReport usersReport = new Stimulsoft.Report.StiReport();

                usersReport.Load(System.Environment.CurrentDirectory + "\\Files\\Reports\\UsersViewReport.mrt");
                usersReport.Dictionary.Variables.Add("Today", System.DateTime.Now.ToPersianDate());
                usersReport.RegBusinessObject("Users", varList);
                usersReport.Compile();
                usersReport.RenderWithWpf();
                usersReport.Print();

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

        private void CloseClick(object sender, System.Windows.RoutedEventArgs e)
        {
            System.Windows.Controls.Panel oPanel = this.Parent as System.Windows.Controls.Panel;
            oPanel.Children.Remove(this);
        }

        private void PasswordGroupBoxSwitchChecked(object sender, System.Windows.RoutedEventArgs e)
        {
            PasswordGroupBox.Visibility = System.Windows.Visibility.Visible;

            MainGroupBoxGrid.RowDefinitions[8].Height =
                new System.Windows.GridLength(165);
        }

        private void PasswordGroupBoxSwitchUnchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            PasswordGroupBox.Visibility = System.Windows.Visibility.Hidden;

            MainGroupBoxGrid.RowDefinitions[8].Height =
                new System.Windows.GridLength(0);
        }

        private void GridControlItemChanged(object sender, DevExpress.Xpf.Grid.SelectedItemChangedEventArgs e)
        {
            ViewModels.UsersManagementViewModel oViewModel = UsersGridControl.SelectedItem as ViewModels.UsersManagementViewModel;
            

            if (oViewModel != null)
            {
                DAL.UnitOfWork oUnitOfWork = new DAL.UnitOfWork();

                Models.User oUser = oUnitOfWork.UserRepository
                    .GetById(oViewModel.Id);

                UsernameTextBox.Text = oUser.Username;
                FirstNameTextBox.Text = oUser.FullName.FirstName;
                LastNameTextBox.Text = oUser.FullName.LastName;
                UserTypeComboBox.SelectedIndex = (oUser.IsAdmin == true) ? 1 : 0;

                CurrentId = oUser.Id;

                DeleteButton.IsEnabled = (EditItemsToggleSwitch.IsChecked == true) ? ((Utility.AdminUserId == CurrentId) ? false : true) : false;
            }
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

            FirstNameTextBox.IsEnabled = true;
            LastNameTextBox.IsEnabled = true;
            UsernameTextBox.IsEnabled = true;
            UserTypeComboBox.IsEnabled = true;
            PasswordChangeToggleSwitch.IsEnabled = true;

            MainGroupBoxGrid.BlurDisable();
            ButtonsGrid.BlurDisable();

            DeleteButton.IsEnabled = (Utility.AdminUserId == CurrentId) ? false : true;

        }

        private void EditItemsToggleSwitchUnchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            AcceptButton.IsEnabled = false;
            CancelButton.IsEnabled = false;
            DeleteButton.IsEnabled = false;

            PasswordChangeToggleSwitch.IsChecked = false;

            FirstNameLabel.IsEnabled = false;
            LastNameLabel.IsEnabled = false;
            UsernameLabel.IsEnabled = false;
            UserTypeLabel.IsEnabled = false;
            PasswordChangeLabel.IsEnabled = false;

            FirstNameTextBox.IsEnabled = false;
            LastNameTextBox.IsEnabled = false;
            UsernameTextBox.IsEnabled = false;
            UserTypeComboBox.IsEnabled = false;
            PasswordChangeToggleSwitch.IsEnabled = false;

            MainGroupBoxGrid.BlurApply(5);
            ButtonsGrid.BlurApply(5);

        }

        private void ComboBoxIsEnabledChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            UserTypeComboBox.Opacity = (UserTypeComboBox.IsEnabled) ? 1.0 : 0.5;
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
                    defaultResult: System.Windows.MessageBoxResult.OK,
                    options: System.Windows.MessageBoxOptions.RightAlign | System.Windows.MessageBoxOptions.RtlReading

                );

                return;
            }

            #endregion

            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                Models.User editedUser = oUnitOfWork.UserRepository
                    .GetById(CurrentId);

                if (editedUser != null)
                {
                    editedUser.Username = UsernameTextBox.Text.Trim();
                    editedUser.FullName.FirstName = FirstNameTextBox.Text.Trim();
                    editedUser.FullName.LastName = LastNameTextBox.Text.Trim();
                    editedUser.IsAdmin = (UserTypeComboBox.SelectedIndex == 1) ? true : false;
                    editedUser.IsAdminToString = (editedUser.IsAdmin == true) ? "کاربر مدیر" : "کاربر عادی";

                    if (PasswordChangeToggleSwitch.IsChecked == true)
                    {
                        #region Error Handling Messages

                        if (string.IsNullOrWhiteSpace(OldPasswordTextBox.Password) == true)
                        {
                            DevExpress.Xpf.Core.DXMessageBox.Show
                            (
                                caption: "خطا",
                                messageBoxText: "تکمیل فیلد رمز عبور فعلی الزامی است.",
                                button: System.Windows.MessageBoxButton.OK,
                                icon: System.Windows.MessageBoxImage.Error,
                                defaultResult: System.Windows.MessageBoxResult.OK,
                                options: System.Windows.MessageBoxOptions.RightAlign | System.Windows.MessageBoxOptions.RtlReading

                            );

                            return;
                        }

                        if (string.IsNullOrWhiteSpace(PasswordTextBox.Password) == true)
                        {
                            DevExpress.Xpf.Core.DXMessageBox.Show
                            (
                                caption: "خطا",
                                messageBoxText: "تکمیل فیلد رمز عبور جدید الزامی است.",
                                button: System.Windows.MessageBoxButton.OK,
                                icon: System.Windows.MessageBoxImage.Error,
                                defaultResult: System.Windows.MessageBoxResult.OK,
                                options: System.Windows.MessageBoxOptions.RightAlign | System.Windows.MessageBoxOptions.RtlReading

                            );

                            return;
                        }

                        if (string.IsNullOrWhiteSpace(AgainPasswordTextBox.Password) == true)
                        {
                            DevExpress.Xpf.Core.DXMessageBox.Show
                            (
                                caption: "خطا",
                                messageBoxText: "تکمیل فیلد تکرار رمز عبور جدید الزامی است.",
                                button: System.Windows.MessageBoxButton.OK,
                                icon: System.Windows.MessageBoxImage.Error,
                                defaultResult: System.Windows.MessageBoxResult.OK,
                                options: System.Windows.MessageBoxOptions.RightAlign | System.Windows.MessageBoxOptions.RtlReading

                            );

                            return;
                        }

                        #endregion

                        string password = Dtx.Security.Hashing.GetMD5(OldPasswordTextBox.Password.Trim());

                        if (password != editedUser.Password)
                        {
                            DevExpress.Xpf.Core.DXMessageBox.Show
                                (
                                    caption: "خطا",
                                    messageBoxText: "رمز عبور درج شده صحیح نمی‌باشد.",
                                    button: System.Windows.MessageBoxButton.OK,
                                    icon: System.Windows.MessageBoxImage.Error,
                                    defaultResult: System.Windows.MessageBoxResult.OK,
                                    options: System.Windows.MessageBoxOptions.RightAlign | System.Windows.MessageBoxOptions.RtlReading
                                );

                            return;
                        }

                        if (PasswordTextBox.Password.Trim() != AgainPasswordTextBox.Password.Trim())
                        {
                            DevExpress.Xpf.Core.DXMessageBox.Show
                                (
                                    caption: "خطا",
                                    messageBoxText: "رمزهای عبور جدید درج شده با یکدیگر مطابقت ندارند.",
                                    button: System.Windows.MessageBoxButton.OK,
                                    icon: System.Windows.MessageBoxImage.Error,
                                    defaultResult: System.Windows.MessageBoxResult.OK,
                                    options: System.Windows.MessageBoxOptions.RightAlign | System.Windows.MessageBoxOptions.RtlReading
                                );

                            return;
                        }

                        editedUser.Password = Dtx.Security.Hashing.GetMD5(PasswordTextBox.Password.Trim());

                    }

                    oUnitOfWork.UserRepository.Update(editedUser);
                    oUnitOfWork.Save();

                    DevExpress.Xpf.Core.DXMessageBox.Show
                        (
                            caption: "پیغام",
                            messageBoxText: "مشخصات کاربر با موفقیت ویرایش گردید.",
                            button: System.Windows.MessageBoxButton.OK,
                            icon: System.Windows.MessageBoxImage.Information,
                            defaultResult: System.Windows.MessageBoxResult.OK,
                            options: System.Windows.MessageBoxOptions.RightAlign | System.Windows.MessageBoxOptions.RtlReading
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

            RefreshGridControl();
        }

        private void CancelClick(object sender, System.Windows.RoutedEventArgs e)
        {
            EditItemsToggleSwitch.IsChecked = false;

            GridControlItemChanged(null, null);
        }

        private void DeleteUserClick(object sender, System.Windows.RoutedEventArgs e)
        {
            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                System.Windows.MessageBoxResult oResult =
                        DevExpress.Xpf.Core.DXMessageBox.Show
                        (
                            caption: "سوال",
                            messageBoxText: "آیا مطمئن به حذف کاربر از سیستم هستید ؟",
                            button: System.Windows.MessageBoxButton.YesNo,
                            icon: System.Windows.MessageBoxImage.Question,
                            defaultResult: System.Windows.MessageBoxResult.No,
                            options: System.Windows.MessageBoxOptions.RightAlign | System.Windows.MessageBoxOptions.RtlReading
                        );

                if (oResult == System.Windows.MessageBoxResult.Yes)
                {
                    oUnitOfWork.UserRepository.DeleteById(CurrentId);

                    oUnitOfWork.Save();

                    DevExpress.Xpf.Core.DXMessageBox.Show
                    (
                        caption: "پیغام",
                        messageBoxText: "کاربر با موفقیت از سیستم حذف گردید.",
                        button: System.Windows.MessageBoxButton.OK,
                        icon: System.Windows.MessageBoxImage.Information,
                        defaultResult: System.Windows.MessageBoxResult.OK,
                        options: System.Windows.MessageBoxOptions.RightAlign | System.Windows.MessageBoxOptions.RtlReading
                    );

                    if (CurrentId == Utility.CurrentUser.Id)
                    {
                        UserLoginWindow oUserLoginWindow = new UserLoginWindow();

                        (Utility.MainWindow as MainRibbonWindow).Hide();

                        oUserLoginWindow.Show();
                    }

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

            RefreshGridControl();
        }

        private void RefreshGridControl()
        {
            DAL.UnitOfWork oUnitOfWork = new DAL.UnitOfWork();

            UsersGridControl.ItemsSource = oUnitOfWork.UserRepository
            .Get()
            .Select(current => new ViewModels.UsersManagementViewModel
            {
                Id = current.Id,
                Username = current.Username,
                FullName = current.FullName,
                PersianRegisterationDate = current.PersianRegisterationDate,
                IsAdminToString = current.IsAdminToString,
                PersianLastLoginTime = current.PersianLastLoginTime,
            })
            .OrderBy(current => current.Username)
            .ToList();
        }
    }
}
