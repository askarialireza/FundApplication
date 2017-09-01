using System.Linq;

namespace Fund
{
    public partial class MainRibbonWindow : DevExpress.Xpf.Ribbon.DXRibbonWindow
    {
        private bool ShowMessageBox = true;

        public MainRibbonWindow()
        {
            InitializeComponent();

            Utility.SetUserTheme();
        }

        private void NewFundClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Fund.FundCreateUserControl oFundCreateUserControl = new FundCreateUserControl();

            UserControlsPanel.Children.Clear();

            UserControlsPanel.Children.Add(oFundCreateUserControl);

        }

        private void UsersViewClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Fund.UsersManagementUserControl oUsersViewUserControl = new UsersManagementUserControl();

            UserControlsPanel.Children.Clear();

            UserControlsPanel.Children.Add(oUsersViewUserControl);
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (ShowMessageBox == true)
            {
                System.Windows.MessageBoxResult oResult =
                    DevExpress.Xpf.Core.DXMessageBox.Show
                    (
                        messageBoxText: "آیا مایل به خروج از برنامه هستید؟",
                        caption: Infrastructure.MessageBoxCaption.Question,
                        button: System.Windows.MessageBoxButton.YesNo,
                        icon: System.Windows.MessageBoxImage.Question,
                        defaultResult: System.Windows.MessageBoxResult.No,
                        options: System.Windows.MessageBoxOptions.RightAlign | System.Windows.MessageBoxOptions.RtlReading
                    );

                if (oResult == System.Windows.MessageBoxResult.Yes)
                {
                    App.Current.Shutdown();
                }
                if (oResult == System.Windows.MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
            }
            else
            {
                App.Current.Shutdown();
            }

        }

        private void CalculatorItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            System.Diagnostics.Process.Start("calc.exe");
        }

        private void NotepadItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            System.Diagnostics.Process.Start("notepad.exe");
        }

        private void WindowLoaded(object sender, System.Windows.RoutedEventArgs e)
        {

            Utility.MainWindow = this;

            CurrentUserLabel.Content = "کاربر کنونی : " + Utility.CurrentUser.Username;

            TodayLabel.Content = FarsiLibrary.Utils.PersianDate.Today.ToWritten();

            System.Windows.Threading.DispatcherTimer oDispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            oDispatcherTimer.IsEnabled = true;
            oDispatcherTimer.Tick += ODispatcherTimer_Tick;

            SetUserSettings();

            RefreshUserInterface();
        }


        private void ODispatcherTimer_Tick(object sender, System.EventArgs e)
        {
            CurrentTimeLabel.Content = System.DateTime.Now.ToString("HH:mm:ss");
        }

        private void FundLoginClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            FundLoginWindow oFundLoginWindow = new FundLoginWindow();

            UserControlsPanel.Children.Clear();

            oFundLoginWindow.Owner = this;

            oFundLoginWindow.ShowDialog();
        }

        public void RefreshUserInterface()
        {

            switch (Utility.CurrentUser.IsAdmin)
            {
                case true:
                    AdminRibbonPage.IsVisible = true;
                    break;
                default:
                    AdminRibbonPage.IsVisible = false;
                    break;
            }

            int fundCountByUser;
            //int memberCountByFund;

            DAL.UnitOfWork oUnitOfWork = null;
            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                fundCountByUser = oUnitOfWork.UserRepository
                    .FundsCountByUser(Utility.CurrentUser);

                if (fundCountByUser == 0 && Utility.CurrentFund == null) // یوزر هیچ صندوقی ندارد
                {
                    MembersRibbonPageGroup.IsVisible = false;
                    LoanAndPaymentRibbonPageGroup.IsVisible = false;
                    FundLoginButton.IsVisible = false;
                    FundSettingsButton.IsVisible = false;
                    FundLogoutButton.IsVisible = false;
                    LargeFundLoginButton.IsVisible = false;
                    ToolsPageGroup.IsVisible = false;
                    SthPanel.Children.Clear();
                }

                if (fundCountByUser != 0) // یوزر حداقل 1 صندوق دارد
                {
                    if (Utility.CurrentFund == null) // هیچ صندوقی لاگین نشده است
                    {
                        MembersRibbonPageGroup.IsVisible = false;
                        LoanAndPaymentRibbonPageGroup.IsVisible = false;
                        FundLoginButton.IsVisible = false;
                        FundSettingsButton.IsVisible = false;
                        FundLogoutButton.IsVisible = false;
                        LargeFundLoginButton.IsVisible = true;
                        ToolsPageGroup.IsVisible = false;
                        SthPanel.Children.Clear();
                    }
                    else // یک صندوق لاگین شده است
                    {
                        MembersRibbonPageGroup.IsVisible = true;
                        LoanAndPaymentRibbonPageGroup.IsVisible = true;
                        FundLoginButton.IsVisible = true;
                        FundSettingsButton.IsVisible = true;
                        FundLogoutButton.IsVisible = true;
                        LargeFundLoginButton.IsVisible = false;
                        ToolsPageGroup.IsVisible = true;

                        MainPanelContentUserControl oMainPanelContentUserControl
                            = new MainPanelContentUserControl();
                        SthPanel.Children.Clear();
                        SthPanel.Children.Add(oMainPanelContentUserControl);


                    }
                }

                oUnitOfWork.Save();
                oUnitOfWork.Dispose();
                oUnitOfWork = null;
            }

            catch (System.Exception ex)
            {
                DevExpress.Xpf.Core.DXMessageBox.Show(ex.Message);
            }
        }

        private void DeleteDatabaseClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            string path = System.Environment.CurrentDirectory + @"\Files\Database\FundDatabase.sdf";
            System.Windows.MessageBoxResult oResult =
                DevExpress.Xpf.Core.DXMessageBox.Show
                (
                    caption: Infrastructure.MessageBoxCaption.Question,
                    messageBoxText: "آیا مطمئن به حذف پایگاه داده می‌باشید ؟ "
                                    + System.Environment.NewLine
                                    + "(اطلاعات حذف شده قابل بازیابی نخواهند بود)",
                    button: System.Windows.MessageBoxButton.YesNo,
                    icon: System.Windows.MessageBoxImage.Information,
                    defaultResult: System.Windows.MessageBoxResult.No,
                    options: System.Windows.MessageBoxOptions.RightAlign | System.Windows.MessageBoxOptions.RtlReading
                );
            if (oResult == System.Windows.MessageBoxResult.Yes)
            {
                ConfirmDeleteDatabaseWindow oConfirmDeleteDatabaseWindow =
                    new ConfirmDeleteDatabaseWindow();

                if ((bool)oConfirmDeleteDatabaseWindow.ShowDialog() == true)
                {
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);

                        DevExpress.Xpf.Core.DXMessageBox.Show
                        (
                            caption: Infrastructure.MessageBoxCaption.Information,
                            messageBoxText: "بانک اطلاعاتی با موفقیت حذف گردید."
                                            + System.Environment.NewLine +
                                            "برنامه مجددا راه اندازی خواهد شد.",
                            button: System.Windows.MessageBoxButton.OK,
                            icon: System.Windows.MessageBoxImage.Information,
                            defaultResult: System.Windows.MessageBoxResult.OK,
                            options: System.Windows.MessageBoxOptions.RightAlign | System.Windows.MessageBoxOptions.RtlReading
                        );
                        ShowMessageBox = false;
                        System.Diagnostics.Process.Start(System.Windows.Application.ResourceAssembly.Location);
                        System.Windows.Application.Current.Shutdown();
                    }


                }
            }
        }

        private void FundLogoutClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Utility.CurrentFund = null;
            Utility.CurrentMember = null;

            this.RefreshUserInterface();
        }

        private void UserLogoutClick(object sender, System.Windows.RoutedEventArgs e)
        {
            Utility.CurrentUser = null;
            Utility.CurrentFund = null;

            this.Hide();

            UserLoginWindow oUserLoginWindow = new UserLoginWindow();

            oUserLoginWindow.Show();

            ShowMessageBox = false; 
        }

        private void CreateUserClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            UsersCreateUserControl oUsersCreateUserControl = new UsersCreateUserControl();

            UserControlsPanel.Children.Clear();

            UserControlsPanel.Children.Add(oUsersCreateUserControl);
        }

        private void MembersViewClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            MembersManagementUserControl oMembersManagementUserControl = new MembersManagementUserControl();

            UserControlsPanel.Children.Clear();

            UserControlsPanel.Children.Add(oMembersManagementUserControl);
        }

        private void SchedulerButtonClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            SchedulerUserControl oSchedulerUserControl = new SchedulerUserControl();

            UserControlsPanel.Children.Clear();

            UserControlsPanel.Children.Add(oSchedulerUserControl);
        }

        private void MemberCreateClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            MemberCreateUserControl oMemberCreateUserControl = new MemberCreateUserControl();

            UserControlsPanel.Children.Clear();

            UserControlsPanel.Children.Add(oMemberCreateUserControl);
        }

        private void DatabaseConfigButton_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            DatabaseSettingsUserControl oDatabaseSettingsUserControl = new DatabaseSettingsUserControl();

            UserControlsPanel.Children.Clear();

            UserControlsPanel.Children.Add(oDatabaseSettingsUserControl);

            ShowMessageBox = false;
        }

        private void SplitterDragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                Models.User oUser = oUnitOfWork.UserRepository
                    .GetById(Utility.CurrentUser.Id);

                oUser.UserSetting.GridWidth.MainPanelWidth = MainGrid.ColumnDefinitions[0].ActualWidth;
                oUser.UserSetting.GridWidth.FundPanelWidth = MainGrid.ColumnDefinitions[2].ActualWidth;

                oUnitOfWork.UserRepository.Update(oUser);

                oUnitOfWork.Save();
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
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

        private void AboutSoftwareButton_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            AboutWindow oAboutWindow = new AboutWindow();

            oAboutWindow.ShowDialog();
        }

        private void MembershipCardPrint_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            MembershipCardPrintWindow oMembershipCardPrintWindow = new MembershipCardPrintWindow();

            oMembershipCardPrintWindow.ShowDialog();
        }

        private void SetUserSettings()
        {
            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                Models.User oUser = oUnitOfWork.UserRepository
                    .GetById(Utility.CurrentUser.Id);

                if (oUser.UserSetting.GridWidth.FundPanelWidth == 0 && oUser.UserSetting.GridWidth.MainPanelWidth == 0)
                {
                    oUser.UserSetting.GridWidth.MainPanelWidth = MainGrid.ColumnDefinitions[0].ActualWidth;
                    oUser.UserSetting.GridWidth.FundPanelWidth = MainGrid.ColumnDefinitions[2].ActualWidth;

                    oUnitOfWork.UserRepository.Update(oUser);
                }
                else
                {
                    MainGrid.ColumnDefinitions[0].Width =
                        new System.Windows.GridLength(oUser.UserSetting.GridWidth.MainPanelWidth, System.Windows.GridUnitType.Star);

                    MainGrid.ColumnDefinitions[2].Width =
                        new System.Windows.GridLength(oUser.UserSetting.GridWidth.FundPanelWidth, System.Windows.GridUnitType.Star);
                }

                Utility.DatabaseBackupPath = oUser.UserSetting.DatabaseBackupPath;

                oUnitOfWork.Save();
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
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

        private void FontChangeButton_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

            UserThemeWindow oUserThemeWindow = new UserThemeWindow();

            oUserThemeWindow.ShowDialog();
        }
    }
}
