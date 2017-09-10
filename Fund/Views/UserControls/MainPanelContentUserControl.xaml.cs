using System.Linq;

namespace Fund
{
    public partial class MainPanelContentUserControl : System.Windows.Controls.UserControl
    {
        public MainPanelContentUserControl()
        {
            InitializeComponent();


        }

        private void UserControlLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            SetUserSettings();

            RefreshSchedulerListBox();

            DAL.UnitOfWork oUnitOfWork = new DAL.UnitOfWork();

            FundManagerNameLabel.Content = (Utility.CurrentFund != null) ? FundManagerNameLabel.Content : string.Empty;
            FundBalanceLabel.Content = (Utility.CurrentFund != null) ? FundBalanceLabel.Content : string.Empty;
            FundRemovalLimitLabel.Content = (Utility.CurrentFund != null) ? FundRemovalLimitLabel.Content : string.Empty;
            FundMembersCountLabel.Content = (Utility.CurrentFund != null) ? FundMembersCountLabel.Content : string.Empty;

            FundDetailsGroupBox.Header = (Utility.CurrentFund != null) ? Utility.CurrentFund.Name : string.Empty;
            FundManagerNameValue.Content = (Utility.CurrentFund != null) ? Utility.CurrentFund.ManagerName : string.Empty;
            FundBalanceValue.Content = (Utility.CurrentFund != null) ? Utility.CurrentFund.Balance.ToRialStringFormat() : string.Empty;
            FundRemovalLimitValue.Content = (Utility.CurrentFund != null) ? Utility.CurrentFund.RemovalLimit.ToRialStringFormat() : string.Empty;
            FundMembersCountValue.Content = (Utility.CurrentFund != null) ? string.Format("{0} نفر", oUnitOfWork.FundRepository.MembersCountByFund(Utility.CurrentFund).ToString()) : string.Empty;
        }

        public void RefreshSchedulerListBox()
        {
            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                int Year = MiniPersianSchedulerReminder.SelectedDateTime.Year;
                int Month = MiniPersianSchedulerReminder.SelectedDateTime.Month;
                int Day = MiniPersianSchedulerReminder.SelectedDateTime.Day;

                var varList = oUnitOfWork.RemainderRepository
                    .GetByPersianDate(Year, Month, Day)
                    .Select(current => new ViewModels.EventTypeViewModel()
                    {
                        Id = current.Id,
                        Description = current.Description,
                        EventType = current.EventType,
                    })
                    .OrderBy(current=>current.EventType)
                    .ToList();

                oUnitOfWork.Save();

                EventsListBox.ItemsSource = varList;

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

        private void MiniPersianSchedulerReminder_SelectedDateTimeChanged(object sender, System.EventArgs e)
        {
            RefreshSchedulerListBox();
        }

        private void SetUserSettings()
        {
            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                Models.User oUser = oUnitOfWork.UserRepository
                    .GetById(Utility.CurrentUser.Id);

                if (oUser.UserSetting.GridHeight.DatePicker == 0 &&
                    oUser.UserSetting.GridHeight.EventsReminder == 0 &&
                    oUser.UserSetting.GridHeight.FundDetails == 0)
                {
                    oUser.UserSetting.GridHeight.DatePicker = MainGrid.RowDefinitions[0].ActualHeight;
                    oUser.UserSetting.GridHeight.EventsReminder = MainGrid.RowDefinitions[2].ActualHeight;
                    oUser.UserSetting.GridHeight.FundDetails = MainGrid.RowDefinitions[4].ActualHeight;

                    oUnitOfWork.UserRepository.Update(oUser);
                }
                else
                {
                    MainGrid.RowDefinitions[0].Height = 
                        new System.Windows.GridLength(oUser.UserSetting.GridHeight.DatePicker, System.Windows.GridUnitType.Star);

                    MainGrid.RowDefinitions[2].Height =
                        new System.Windows.GridLength(oUser.UserSetting.GridHeight.EventsReminder, System.Windows.GridUnitType.Star);

                    MainGrid.RowDefinitions[4].Height =
                        new System.Windows.GridLength(oUser.UserSetting.GridHeight.FundDetails, System.Windows.GridUnitType.Star);
                }

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

        private void GridSplitter_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                Models.User oUser = oUnitOfWork.UserRepository
                    .GetById(Utility.CurrentUser.Id);


                oUser.UserSetting.GridHeight.DatePicker = MainGrid.RowDefinitions[0].ActualHeight;
                oUser.UserSetting.GridHeight.EventsReminder = MainGrid.RowDefinitions[2].ActualHeight;
                oUser.UserSetting.GridHeight.FundDetails = MainGrid.RowDefinitions[4].ActualHeight;

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

        private void MiniPersianSchedulerReminder_Refresh(object sender, System.EventArgs e)
        {
            RefreshSchedulerListBox();
        }
    }
}
