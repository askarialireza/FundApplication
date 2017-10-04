using System.Linq;

namespace Fund
{
    public partial class MainWindow : Fluent.RibbonWindow
    {
        private bool ShowMessageBox = true;

        public MainWindow()
        {
            InitializeComponent();

            SetUserSettings();

            RefreshUserInterface();
        }

        private void RibbonWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (ShowMessageBox == true)
            {
                System.Windows.MessageBoxResult oResult =
                    Infrastructure.MessageBox.Show
                        (
                            caption: Infrastructure.MessageBox.Caption.Question,
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

        private void RibbonWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Utility.MainWindow = this;

            CurrentUserLabel.Text = "کاربر کنونی : " + Utility.CurrentUser.Username;

            System.Windows.Threading.DispatcherTimer oDispatcherTimer = new System.Windows.Threading.DispatcherTimer();

            oDispatcherTimer.IsEnabled = true;

            oDispatcherTimer.Tick += ODispatcherTimer_Tick;

            SetUserSettings();
        }

        private void ODispatcherTimer_Tick(object sender, System.EventArgs e)
        {
            CurrentTimeLabel.Content = System.DateTime.Now.ToString("HH:mm:ss");

            TodayLabel.Content = FarsiLibrary.Utils.PersianDate.Today.ToWritten();
        }

        private void FundCreateButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Fund.FundCreateUserControl oFundCreateUserControl = new FundCreateUserControl();

            oFundCreateUserControl.Show();
        }

        private void LargeFundLoginButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            FundLoginWindow oFundLoginWindow = new FundLoginWindow();

            UserControlsPanel.Children.Clear();

            oFundLoginWindow.ShowDialog();
        }

        private void FundLoginButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            FundLoginWindow oFundLoginWindow = new FundLoginWindow();

            UserControlsPanel.Children.Clear();

            oFundLoginWindow.ShowDialog();
        }

        private void FundSettingsButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            FundSettingsUserControl oFundSettingsUserControl = new FundSettingsUserControl();

            oFundSettingsUserControl.Show();
        }

        private void MembersListButton_ItemClick(object sender, System.Windows.RoutedEventArgs e)
        {
            DAL.UnitOfWork oUnitOfWork = new DAL.UnitOfWork();

            int membersCount = oUnitOfWork.MemberRepository
                .Get()
                .Where(current => current.FundId == Utility.CurrentFund.Id)
                .Count();

            if (membersCount != 0)
            {
                MembersListUserControl oMembersListUserControl = new MembersListUserControl();

                oMembersListUserControl.Show();
            }
            else
            {
                Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: "برای صندوق هیچ عضوی در سیستم ثبت نشده است. نسبت به ایجاد عضو اقدام نمایید.");

                return;
            }
        }

        private void FundLogoutButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Utility.CurrentFund = null;
            Utility.CurrentMember = null;
            Utility.CurrentLoan = null;

            UserControlsPanel.Children.Clear();

            RefreshUserInterface();
        }

        private void MemberCreateButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MemberCreateUserControl oMemberCreateUserControl = new MemberCreateUserControl();

            oMemberCreateUserControl.Show();
        }

        private void MembersManagementButton_Click(object sender, System.Windows.RoutedEventArgs e)
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
                Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: "برای صندوق هیچ عضوی در سیستم ثبت نشده است. نسبت به ایجاد عضو اقدام نمایید.");

                return;
            }
        }

        private void MembersListButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MembersListUserControl oMembersListUserControl = new MembersListUserControl();

            oMembersListUserControl.Show();
        }

        private void MembershipCardPrint_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DAL.UnitOfWork oUnitOfWork = new DAL.UnitOfWork();

            int membersCount = oUnitOfWork.MemberRepository
                .Get()
                .Where(current => current.FundId == Utility.CurrentFund.Id)
                .Count();

            if (membersCount != 0)
            {
                MembershipCardPrintWindow oMembershipCardPrintWindow = new MembershipCardPrintWindow();

                oMembershipCardPrintWindow.ShowDialog();
            }
            else
            {
                Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: "برای صندوق هیچ عضوی در سیستم ثبت نشده است. نسبت به ایجاد عضو اقدام نمایید.");

                return;
            }
        }

        private void DebtorListButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DAL.UnitOfWork oUnitOfWork = new DAL.UnitOfWork();

            int transactionsCount = oUnitOfWork.TransactionRepository
                .Get()
                .Where(current => current.FundId == Utility.CurrentFund.Id)
                .Count();

            if (transactionsCount != 0)
            {
                DebtorsListUserControl oDebtorsListUserControl = new DebtorsListUserControl();

                oDebtorsListUserControl.Show();
            }
            else
            {
                Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: "هیچ تراکنشی در صندوق ثبت نشده است، اطلاعاتی برای نمایش وجود ندارد.");

                return;
            }
        }

        private void MembersTransactionButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MemberTransactionsUserControl oMemberTransactionsUserControl = new MemberTransactionsUserControl();

            oMemberTransactionsUserControl.Show();
        }

        private void FundTransactionsButton_Click(object sender, System.Windows.RoutedEventArgs e)
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
                Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: "هیچ تراکنشی در صندوق ثبت نشده است، اطلاعاتی برای نمایش وجود ندارد.");

                return;
            }
        }

        private void FundBalanceButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            FundBalanceWindow oFundBalanceWindow = new FundBalanceWindow();

            oFundBalanceWindow.ShowDialog();
        }

        private void MemberBalanceButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MemberBalanceWindow oMemberBalanceWindow = new MemberBalanceWindow();

            oMemberBalanceWindow.ShowDialog();
        }

        private void PayedLoansList_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DAL.UnitOfWork oUnitOfWork = new DAL.UnitOfWork();

            int loansCount = oUnitOfWork.LoanRepository
                .Get()
                .Where(current => current.Member.FundId == Utility.CurrentFund.Id)
                .Where(current => current.IsActive == true)
                .Count();

            if (loansCount != 0)
            {
                ShowConfirmedLoansUserControl oShowConfirmedLoansUserControl = new ShowConfirmedLoansUserControl();

                oShowConfirmedLoansUserControl.Show();
            }
            else
            {
                Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: "برای صندوق هیچ وامی در سیستم ثبت نشده است. نسبت به ثبت وام اقدام نمایید.");

                return;
            }
        }

        private void DelayedInstallmentsListButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DelayedInstallmentsListUserControl oDelayedInstallmentsListUserControl = new DelayedInstallmentsListUserControl();

            oDelayedInstallmentsListUserControl.Show();
        }

        private void MemberLonasStatus_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Utility.CurrentMember = null;
            Utility.CurrentLoan = null;

            MemberLoansStatusUserControl oMemberLoansStatusUserControl = new MemberLoansStatusUserControl();

            oMemberLoansStatusUserControl.Show();
        }

        private void UsersViewButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Fund.UsersManagementUserControl oUsersViewUserControl = new UsersManagementUserControl();

            oUsersViewUserControl.Show();
        }

        private void CreateUserButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            UsersCreateUserControl oUsersCreateUserControl = new UsersCreateUserControl();

            oUsersCreateUserControl.Show();
        }

        private void DeleteDatabase_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string path = System.Environment.CurrentDirectory + @"\Files\Database\FundDatabase.sdf";

            System.Windows.MessageBoxResult oResult =
                Infrastructure.MessageBox.Show
                    (
                        caption: Infrastructure.MessageBox.Caption.Question,
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
                                caption: Infrastructure.MessageBox.Caption.Information,
                                text: "بانک اطلاعاتی با موفقیت حذف گردید." + System.Environment.NewLine + "برنامه مجددا راه اندازی خواهد شد."
                            );

                        ShowMessageBox = false;

                        System.Diagnostics.Process.Start(System.Windows.Application.ResourceAssembly.Location);

                        System.Windows.Application.Current.Shutdown();
                    }
                }
            }
        }

        private void DatabaseConfigButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DatabaseSettingsUserControl oDatabaseSettingsUserControl = new DatabaseSettingsUserControl();

            oDatabaseSettingsUserControl.Show();

            ShowMessageBox = false;
        }

        private void SchedulerButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SchedulerUserControl oSchedulerUserControl = new SchedulerUserControl();

            oSchedulerUserControl.Show();
        }

        private void CalculatorButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("calc.exe");
        }

        private void NotepadButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("notepad.exe");
        }

        private void AboutSoftwareButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            AboutWindow oAboutWindow = new AboutWindow();

            oAboutWindow.ShowDialog();
        }

        private void GridSplitter_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
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

        private void CreateLoanButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DAL.UnitOfWork oUnitOfWork = new DAL.UnitOfWork();

            int membersCount = oUnitOfWork.MemberRepository
                .Get()
                .Where(current => current.FundId == Utility.CurrentFund.Id)
                .Count();

            if (membersCount != 0)
            {
                CreateLoanUserControl oCreateLoanUserControl = new CreateLoanUserControl();

                oCreateLoanUserControl.Show();
            }
            else
            {
                Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: "برای صندوق هیچ عضوی در سیستم ثبت نشده است. نسبت به ایجاد عضو اقدام نمایید.");

                return;
            }
        }

        private void LoanAndInstallmentsManagentButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            LoansAndInstallmentManagementUserControl oLoansAndInstallmentManagementUserControl = new LoansAndInstallmentManagementUserControl();

            oLoansAndInstallmentManagementUserControl.Show();
        }

        private void UserLogoutButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Utility.CurrentUser = null;
            Utility.CurrentFund = null;

            SthPanel.Children.Clear();

            this.Hide();

            UserLoginWindow oUserLoginWindow = new UserLoginWindow();

            oUserLoginWindow.ShowWelcomeWindow = false;

            oUserLoginWindow.Show();

            ShowMessageBox = false;
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

        public void RefreshUserInterface()
        {
            System.ComponentModel.BackgroundWorker oBackgroundWorker = new System.ComponentModel.BackgroundWorker();

            oBackgroundWorker.WorkerReportsProgress = true;

            oBackgroundWorker.DoWork += OBackgroundWorker_DoWork1;

            oBackgroundWorker.RunWorkerAsync();
        }

        private void OBackgroundWorker_DoWork1(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                switch (Utility.CurrentUser.IsAdmin)
                {
                    case true:
                        {
                            AdminRibbonPage.Visibility = System.Windows.Visibility.Visible;
                            break;
                        }

                    default:
                        {
                            AdminRibbonPage.Visibility = System.Windows.Visibility.Collapsed;
                            break;
                        }
                }

                int fundCountByUser;
                int memberCountByFund;
                int loanCountByFund;

                DAL.UnitOfWork oUnitOfWork = null;

                oUnitOfWork = new DAL.UnitOfWork();

                fundCountByUser = oUnitOfWork.UserRepository
                    .FundsCountByUser(Utility.CurrentUser);

                if (fundCountByUser == 0 && Utility.CurrentFund == null) // یوزر هیچ صندوقی ندارد
                {
                    MembersRibbonPageGroup.Visibility = System.Windows.Visibility.Collapsed;
                    ReportsRibbonPage.Visibility = System.Windows.Visibility.Collapsed;
                    LoanAndPaymentRibbonPageGroup.Visibility = System.Windows.Visibility.Collapsed;

                    FundLoginButton.Visibility = System.Windows.Visibility.Collapsed;
                    FundSettingsButton.Visibility = System.Windows.Visibility.Collapsed;
                    FundLogoutButton.Visibility = System.Windows.Visibility.Collapsed;
                    LargeFundLoginButton.Visibility = System.Windows.Visibility.Collapsed;
                    ToolsPageGroup.Visibility = System.Windows.Visibility.Collapsed;
                    SthPanel.Children.Clear();
                }

                if (fundCountByUser != 0) // یوزر حداقل 1 صندوق دارد
                {
                    if (Utility.CurrentFund == null) // هیچ صندوقی لاگین نشده است
                    {
                        MembersRibbonPageGroup.Visibility = System.Windows.Visibility.Collapsed;
                        LoanAndPaymentRibbonPageGroup.Visibility = System.Windows.Visibility.Collapsed;
                        FundLoginButton.Visibility = System.Windows.Visibility.Collapsed;
                        FundSettingsButton.Visibility = System.Windows.Visibility.Collapsed;
                        FundLogoutButton.Visibility = System.Windows.Visibility.Collapsed;
                        LargeFundLoginButton.Visibility = System.Windows.Visibility.Visible;
                        ToolsPageGroup.Visibility = System.Windows.Visibility.Collapsed;
                        ReportsRibbonPage.Visibility = System.Windows.Visibility.Collapsed;
                        SthPanel.Children.Clear();
                    }
                    else // یک صندوق لاگین شده است
                    {
                        memberCountByFund = oUnitOfWork.MemberRepository
                            .Get()
                            .Where(current => current.FundId == Utility.CurrentFund.Id)
                            .Count();

                        loanCountByFund = oUnitOfWork.LoanRepository
                            .Get()
                            .Where(current => current.Member.FundId == Utility.CurrentFund.Id)
                            .Count();

                        MembersRibbonPageGroup.Visibility = System.Windows.Visibility.Visible;
                        FundLoginButton.Visibility = System.Windows.Visibility.Visible;
                        FundSettingsButton.Visibility = System.Windows.Visibility.Visible;
                        FundLogoutButton.Visibility = System.Windows.Visibility.Visible;
                        LargeFundLoginButton.Visibility = System.Windows.Visibility.Collapsed;

                        ToolsPageGroup.Visibility = System.Windows.Visibility.Visible;

                        if (memberCountByFund == 0) // هیچ عضوی ندارد
                        {
                            ReportsRibbonPage.Visibility = System.Windows.Visibility.Collapsed;
                            LoanAndPaymentRibbonPageGroup.Visibility = System.Windows.Visibility.Collapsed;
                            MembersManagementButton.Visibility = System.Windows.Visibility.Collapsed;
                        }
                        else if (loanCountByFund == 0 && memberCountByFund != 0) // عضو دارد وامی ثبت نشده است
                        {
                            MembersManagementButton.Visibility = System.Windows.Visibility.Visible;
                            ReportsRibbonPage.Visibility = System.Windows.Visibility.Visible;
                            LoanAndPaymentRibbonPageGroup.Visibility = System.Windows.Visibility.Visible;
                            LoanAndInstallmentsManagentButton.Visibility = System.Windows.Visibility.Collapsed;
                            PopularReportRibbonPage.Visibility = System.Windows.Visibility.Visible;
                            DetailsReportRibbonPage.Visibility = System.Windows.Visibility.Collapsed;
                            BalanceReportRibbonPage.Visibility = System.Windows.Visibility.Collapsed;
                            LoansReportRibbonPage.Visibility = System.Windows.Visibility.Collapsed;
                        }
                        else // عضو دارد و وام نیز ثبت شده است.
                        {
                            MembersManagementButton.Visibility = System.Windows.Visibility.Visible;
                            ReportsRibbonPage.Visibility = System.Windows.Visibility.Visible;
                            LoanAndPaymentRibbonPageGroup.Visibility = System.Windows.Visibility.Visible;
                            LoanAndInstallmentsManagentButton.Visibility = System.Windows.Visibility.Visible;
                            PopularReportRibbonPage.Visibility = System.Windows.Visibility.Visible;
                            DetailsReportRibbonPage.Visibility = System.Windows.Visibility.Visible;
                            BalanceReportRibbonPage.Visibility = System.Windows.Visibility.Visible;
                            LoansReportRibbonPage.Visibility = System.Windows.Visibility.Visible;
                        }

                        MainPanelContentUserControl oMainPanelContentUserControl = new MainPanelContentUserControl();
                        SthPanel.Children.Clear();
                        SthPanel.Children.Add(oMainPanelContentUserControl);
                    }
                }
            });
        }

        private void MemberDepositButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MemberDepositUserControl oMemberDepositUserControl = new MemberDepositUserControl();

            oMemberDepositUserControl.Show();
        }
    }
}
