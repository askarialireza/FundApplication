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

            FundDetailsGroupBox.Header = Utility.CurrentFund.Name;

            FundManagerNameValue.Content = Utility.CurrentFund.ManagerName;

            FundBalanceValue.Content = Utility.CurrentFund.Balance.ToRialStringFormat();

            FundRemovalLimitValue.Content = Utility.CurrentFund.RemovalLimit.ToRialStringFormat();

            FundMembersCountValue.Content = string.Format("{0} نفر", oUnitOfWork.FundRepository.MembersCountByFund(Utility.CurrentFund));

            FundPercentValue.Content = string.Format("{0} درصد", Utility.CurrentFund.Percent);

            LoansCountValue.Content = string.Format("{0} وام", oUnitOfWork.LoanRepository.Get().Where(current => current.Member.FundId == Utility.CurrentFund.Id).Count());

            PayedLoansCountValue.Content = string.Format("{0} وام", oUnitOfWork.LoanRepository.Get().Where(current => current.Member.FundId == Utility.CurrentFund.Id).Where(current=>current.IsPayed==true).Count());

            CurrentLoansCountValue.Content = string.Format("{0} وام", oUnitOfWork.LoanRepository.Get().Where(current => current.Member.FundId == Utility.CurrentFund.Id).Where(current => current.IsPayed == false).Count());
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
                Infrastructure.MessageBox.Show(ex.Message);;
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
                Infrastructure.MessageBox.Show(ex.Message);;
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
                Infrastructure.MessageBox.Show(ex.Message);;
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
