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

            Utility.MainWindow = this;

            CurrentUserLabel.Content = "کاربر کنونی : " + Utility.CurrentUser.Username;

            TodayLabel.Content = FarsiLibrary.Utils.PersianDate.Today.ToWritten();

            System.Windows.Threading.DispatcherTimer oDispatcherTimer = new System.Windows.Threading.DispatcherTimer();

            oDispatcherTimer.IsEnabled = true;

            oDispatcherTimer.Tick += ODispatcherTimer_Tick;

            SetUserSettings();

            RefreshUserInterface();
        }

        private void NewFundClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Fund.FundCreateUserControl oFundCreateUserControl = new FundCreateUserControl();

            oFundCreateUserControl.Show();

        }

        private void UsersViewClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Fund.UsersManagementUserControl oUsersViewUserControl = new UsersManagementUserControl();

            oUsersViewUserControl.Show();
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

        private void FundLogoutClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Utility.CurrentFund = null;
            Utility.CurrentMember = null;
            Utility.CurrentLoan = null;

            UserControlsPanel.Children.Clear();

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

            oUsersCreateUserControl.Show();
        }

        private void MembersViewClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            DAL.UnitOfWork oUnitOfWork = new DAL.UnitOfWork();

            int membersCount = oUnitOfWork.MemberRepository
                .Get()
                .Where(current => current.FundId == Utility.CurrentFund.Id)
                .Count();

            if (membersCount != 0)
            {
                MembersManagementUserControl oMembersManagementUserControl = new MembersManagementUserControl();

                oMembersManagementUserControl.Show();
            }
            else
            {
                Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBoxCaption.Error, text: "برای صندوق هیچ عضوی در سیستم ثبت نشده است. نسبت به ایجاد عضو اقدام نمایید.");

                return;
            }

        }

        private void SchedulerButtonClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            SchedulerUserControl oSchedulerUserControl = new SchedulerUserControl();

            oSchedulerUserControl.Show();
        }

        private void MemberCreateClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            MemberCreateUserControl oMemberCreateUserControl = new MemberCreateUserControl();

            oMemberCreateUserControl.Show();
        }

        private void DatabaseConfigButton_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            DatabaseSettingsUserControl oDatabaseSettingsUserControl = new DatabaseSettingsUserControl();

            oDatabaseSettingsUserControl.Show();

            ShowMessageBox = false;
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

        private void FontChangeButton_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

            UserThemeWindow oUserThemeWindow = new UserThemeWindow();

            oUserThemeWindow.ShowDialog();
        }

        private void BarButtonItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            CreateLoanUserControl oCreateLoanUserControl = new CreateLoanUserControl();

            oCreateLoanUserControl.Show();
        }

        private void PayedLoansList_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            ShowConfirmedLoansUserControl oShowConfirmedLoansUserControl = new ShowConfirmedLoansUserControl();

            oShowConfirmedLoansUserControl.Show();
        }

        private void DebtorListButton_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            DebtorsListUserControl oDebtorsListUserControl = new DebtorsListUserControl();

            oDebtorsListUserControl.Show();
        }

        private void FundTransactionsButton_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            DAL.UnitOfWork oUnitOfWork = new DAL.UnitOfWork();

            int transactionsCount = oUnitOfWork.TransactionRepository
                .Get()
                .Where(current => current.FundId == Utility.CurrentFund.Id)
                .Count();

            if (transactionsCount != 0)
            {
                FundTransactionsUserControl oFundTransactionsUserControl = new FundTransactionsUserControl();

                oFundTransactionsUserControl.Show();
            }
            else
            {
                Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBoxCaption.Error, text: "هیچ تراکنشی در صندوق ثبت نشده است، اطلاعاتی برای نمایش وجود ندارد.");

                return;
            }


        }

        private void MembersTransactionButton_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

        }

        private void MemberLonasStatus_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

            Utility.CurrentMember = null;
            Utility.CurrentLoan = null;

            MemberLoansStatusUserControl oMemberLoansStatusUserControl = new MemberLoansStatusUserControl();

            oMemberLoansStatusUserControl.Show();
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

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (ShowMessageBox == true)
            {
                System.Windows.MessageBoxResult oResult =
                    Infrastructure.MessageBox.Show
                        (
                            caption: Infrastructure.MessageBoxCaption.Question,
                            text: "آیا مایل به خروج از برنامه هستید؟"
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

        private void DeleteDatabaseClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            string path = System.Environment.CurrentDirectory + @"\Files\Database\FundDatabase.sdf";

            System.Windows.MessageBoxResult oResult =
                Infrastructure.MessageBox.Show
                    (
                        caption: Infrastructure.MessageBoxCaption.Question,
                        text: "آیا مطمئن به حذف پایگاه داده می‌باشید ؟ " + System.Environment.NewLine + "(اطلاعات حذف شده قابل بازیابی نخواهند بود)"
                    );

            if (oResult == System.Windows.MessageBoxResult.Yes)
            {
                ConfirmDeleteDatabaseWindow oConfirmDeleteDatabaseWindow = new ConfirmDeleteDatabaseWindow();

                if (oConfirmDeleteDatabaseWindow.ShowDialog() == true)
                {
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);

                        Infrastructure.MessageBox.Show
                            (
                                caption: Infrastructure.MessageBoxCaption.Information,
                                text: "بانک اطلاعاتی با موفقیت حذف گردید." + System.Environment.NewLine + "برنامه مجددا راه اندازی خواهد شد."
                            );

                        ShowMessageBox = false;

                        System.Diagnostics.Process.Start(System.Windows.Application.ResourceAssembly.Location);

                        System.Windows.Application.Current.Shutdown();
                    }
                }
            }
        }

        public void RefreshUserInterface()
        {

            switch (Utility.CurrentUser.IsAdmin)
            {
                case true:
                    {
                        AdminRibbonPage.IsVisible = true;
                        break;
                    }

                default:
                    {
                        AdminRibbonPage.IsVisible = false;
                        break;
                    }
            }

            int fundCountByUser;
            int memberCountByFund;

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
                    ReportsRibbonPage.IsVisible = false;
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
                        ReportsRibbonPage.IsVisible = false;
                        SthPanel.Children.Clear();
                    }
                    else // یک صندوق لاگین شده است
                    {
                        memberCountByFund = oUnitOfWork.MemberRepository
                            .Get()
                            .Where(current => current.FundId == Utility.CurrentFund.Id)
                            .Count();

                        MembersRibbonPageGroup.IsVisible = true;
                        LoanAndPaymentRibbonPageGroup.IsVisible = true;
                        FundLoginButton.IsVisible = true;
                        FundSettingsButton.IsVisible = true;
                        FundLogoutButton.IsVisible = true;
                        LargeFundLoginButton.IsVisible = false;
                        ReportsRibbonPage.IsVisible = true;
                        ToolsPageGroup.IsVisible = true;

                        MainPanelContentUserControl oMainPanelContentUserControl = new MainPanelContentUserControl();
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
                Infrastructure.MessageBox.Show(ex.Message); ;
            }
        }

        private void DelayedInstallmentsListButton_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            DelayedInstallmentsListUserControl oDelayedInstallmentsListUserControl = new DelayedInstallmentsListUserControl();

            oDelayedInstallmentsListUserControl.Show();
        }

        private void InstallmentsManagent_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

        }

        private void LoanManagements_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

        }

        private void FundSettingsButton_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            FundSettingsUserControl oFundSettingsUserControl = new FundSettingsUserControl();

            oFundSettingsUserControl.Show();
        }
    }
}
