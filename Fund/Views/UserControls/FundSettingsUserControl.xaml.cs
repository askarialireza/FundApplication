using System.Linq;

namespace Fund
{
    public partial class FundSettingsUserControl : System.Windows.Controls.UserControl
    {
        public FundSettingsUserControl()
        {
            InitializeComponent();

            LoadData();
        }

        private void ToggleSwitch_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            DevExpress.Xpf.Editors.ToggleSwitch oToggleSwitch = ((DevExpress.Xpf.Editors.ToggleSwitch)sender);

            int row = System.Windows.Controls.Grid.GetRow(oToggleSwitch);

            System.Windows.Controls.RowDefinition oRowDefinition = MainGrid.RowDefinitions[row + 1];

            oRowDefinition.Height = new System.Windows.GridLength(40);
        }

        private void ToggleSwitch_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            DevExpress.Xpf.Editors.ToggleSwitch oToggleSwitch = ((DevExpress.Xpf.Editors.ToggleSwitch)sender);

            int row = System.Windows.Controls.Grid.GetRow(oToggleSwitch);

            System.Windows.Controls.RowDefinition oRowDefinition = MainGrid.RowDefinitions[row + 1];

            oRowDefinition.Height = new System.Windows.GridLength(0);
        }

        private void TextBoxEditorActivated(object sender, System.Windows.RoutedEventArgs e)
        {
            ((DevExpress.Xpf.Editors.TextEdit)sender).Text =
                ((DevExpress.Xpf.Editors.TextEdit)sender).Text.Replace(" ریال", string.Empty).Replace(",", string.Empty);
        }

        private void TextBoxValidate(object sender, DevExpress.Xpf.Editors.ValidationEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(((DevExpress.Xpf.Editors.TextEdit)sender).Text) == false)
            {
                long value = System.Convert.ToInt64(((DevExpress.Xpf.Editors.TextEdit)sender).Text.Replace(" ریال", string.Empty).Replace(",", string.Empty));

                ((DevExpress.Xpf.Editors.TextEdit)sender).Text = value.ToRialStringFormat();
            }
            else
            {
                long zero = 0;
                ((DevExpress.Xpf.Editors.TextEdit)sender).Text = zero.ToRialStringFormat();
            }

            RefreshLabels();
        }

        private void LoadData()
        {
            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                Models.Fund oFund = oUnitOfWork.FundRepository
                    .GetById(Utility.CurrentFund.Id);

                FundNameTextBox.Text = oFund.Name;
                FundManagerNameTextBox.Text = oFund.ManagerName;
                FundBuildYearTextBox.Text = oFund.FoundationYear.ToString();
                FundDepositBalanceTextBox.Text = Utility.ToRialStringFormat(FundDepositBalanceTextBox.Text.StringToMoney());
                FundRemovalLimitTextBox.Text = oFund.RemovalLimit.ToRialStringFormat();

                RefreshLabels();

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

        private void RefreshLabels()
        {
            FundBalanceDescriptionLabel.Content =
                string.Format("موجودی قبلی : {0} , موجودی جدید : {1}",
                (Utility.CurrentFund.Balance.ToRialStringFormat()), ((Utility.CurrentFund.Balance + FundDepositBalanceTextBox.Text.StringToMoney()).ToRialStringFormat()));
        }

        private void PasswordToggleSwitch_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            DevExpress.Xpf.Editors.ToggleSwitch oToggleSwitch = ((DevExpress.Xpf.Editors.ToggleSwitch)sender);

            int row = System.Windows.Controls.Grid.GetRow(oToggleSwitch);

            System.Windows.Controls.RowDefinition oRowDefinition0 = MainGrid.RowDefinitions[row + 1];
            System.Windows.Controls.RowDefinition oRowDefinition1 = MainGrid.RowDefinitions[row + 2];
            System.Windows.Controls.RowDefinition oRowDefinition2 = MainGrid.RowDefinitions[row + 3];

            oRowDefinition0.Height = new System.Windows.GridLength(40);
            oRowDefinition1.Height = new System.Windows.GridLength(40);
            oRowDefinition2.Height = new System.Windows.GridLength(40);
        }

        private void PasswordToggleSwitch_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            DevExpress.Xpf.Editors.ToggleSwitch oToggleSwitch = ((DevExpress.Xpf.Editors.ToggleSwitch)sender);

            int row = System.Windows.Controls.Grid.GetRow(oToggleSwitch);

            System.Windows.Controls.RowDefinition oRowDefinition0 = MainGrid.RowDefinitions[row + 1];
            System.Windows.Controls.RowDefinition oRowDefinition1 = MainGrid.RowDefinitions[row + 2];
            System.Windows.Controls.RowDefinition oRowDefinition2 = MainGrid.RowDefinitions[row + 3];

            oRowDefinition0.Height = new System.Windows.GridLength(0);
            oRowDefinition1.Height = new System.Windows.GridLength(0);
            oRowDefinition2.Height = new System.Windows.GridLength(0);
        }

        private void CloseButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Close();
        }

        private void AcceptButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            EditFundProperties();
        }

        private void EditFundProperties()
        {
            #region Error Handling Messages

            if (string.IsNullOrWhiteSpace(FundNameTextBox.Text) == true)
            {
                Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: "تکمیل فیلد نام صندوق الزامی است.");

                return;
            }

            if (string.IsNullOrWhiteSpace(FundManagerNameTextBox.Text) == true)
            {
                Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: "تکمیل فیلد نام مدیر صندوق الزامی می‌باشد.");

                return;
            }

            if (string.IsNullOrWhiteSpace(FundBuildYearTextBox.Text) == true)
            {
                Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: "تکمیل فیلد سال تاسیس الزامی می‌باشد.");

                return;
            }

            if (PasswordToggleSwitch.IsChecked == true)
            {
                if (string.IsNullOrWhiteSpace(CurrentPasswordBox.Password) == true)
                {
                    Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: "تکمیل فیلد رمز عبور فعلی الزامی می‌باشد.");

                    return;
                }

                if (string.IsNullOrWhiteSpace(NewPasswordBox.Password) == true)
                {
                    Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: "تکمیل فیلد رمز عبور جدید الزامی می‌باشد.");

                    return;
                }

                if (string.IsNullOrWhiteSpace(ConfirmPasswordBox.Password) == true)
                {
                    Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: "تکمیل فیلد تایید رمز عبور جدید الزامی می‌باشد.");

                    return;
                }

            }

            if (DepositBalanceToggleSwitch.IsChecked == true)
            {
                if (string.IsNullOrWhiteSpace(FundDepositBalanceTextBox.Text) == true)
                {
                    Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: "در صورت فعال بودن گزینه افزایش موجودی، تکمیل فیلد افزایش موجودی الزامی است.");

                    return;
                }
            }

            if (RemovalLimitToggleSwitch.IsChecked == true)
            {
                if (string.IsNullOrWhiteSpace(FundRemovalLimitTextBox.Text) == true)
                {
                    Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: "در صورت فعال بودن گزینه تغییر سقف برداشت وام، تکمیل فیلد تغییر سقف پرداخت وام الزامی است.");

                    return;
                }
            }

            if (PercentToggleSwitch.IsChecked == true)
            {
                if (string.IsNullOrWhiteSpace(FundPercentTextBox.Text.Trim()) == true)
                {
                    Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: "در صورت فعال بودن گزینه کارمزد، تکمیل فیلد کارمزد الزامی است.");

                    return;
                }
            }

            #endregion

            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                Models.Fund oFund = oUnitOfWork.FundRepository
                    .GetById(Utility.CurrentFund.Id);

                if (oFund != null)
                {
                    oFund.Name = FundNameTextBox.Text;
                    oFund.ManagerName = FundManagerNameTextBox.Text;
                    oFund.FoundationYear = System.Convert.ToInt32(FundBuildYearTextBox.Text.Trim());

                    if (PasswordToggleSwitch.IsChecked == true)
                    {
                        if (Dtx.Security.Hashing.GetMD5(CurrentPasswordBox.Password.Trim()) != oFund.ManagerPassword)
                        {
                            Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: "رمز عبور درج شده صحیح نمی‌باشد.");

                            return;
                        }
                        if (NewPasswordBox.Password.Trim() != ConfirmPasswordBox.Password.Trim())
                        {
                            Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: "رمزهای عبور جدید درج شده دارای مطابقت نمی‌باشد.");

                            return;
                        }

                        oFund.ManagerPassword = Dtx.Security.Hashing.GetMD5(NewPasswordBox.Password);
                    }

                    if (DepositBalanceToggleSwitch.IsChecked == true)
                    {
                        oFund.Balance += (FundDepositBalanceTextBox.Text.StringToMoney());

                        Models.Transaction oTransaction = new Models.Transaction();

                        oTransaction.Amount = FundDepositBalanceTextBox.Text.StringToMoney();
                        oTransaction.Balance = oFund.Balance;
                        oTransaction.Date = System.DateTime.Now;
                        oTransaction.FundId = oFund.Id;
                        oTransaction.MemberId = null;
                        oTransaction.TransactionType = Models.TransactionType.Deposit;
                        oTransaction.Description = string.Format("افزایش موجودی صندوق به مبلغ {0}", oTransaction.Amount.ToRialStringFormat());

                        oUnitOfWork.TransactionRepository.Insert(oTransaction);

                        oUnitOfWork.Save();
                    }

                    if (RemovalLimitToggleSwitch.IsChecked == true)
                    {
                        oFund.RemovalLimit = FundRemovalLimitTextBox.Text.StringToMoney();
                    }

                    if (PercentToggleSwitch.IsChecked == true)
                    {
                        oFund.Percent = System.Convert.ToInt32(FundPercentTextBox.Text.Trim());
                    }

                    oUnitOfWork.FundRepository.Update(oFund);
                }

                oUnitOfWork.Save();

                Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Information, text: "اطلاعات صندوق با موفقیت ویرایش گردید");

                Utility.CurrentFund = oFund;

                Utility.MainWindow.RefreshUserInterface();
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

        private void DeleteMembers_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                System.Windows.MessageBoxResult oResult =
                    Infrastructure.MessageBox.Show
                    (
                        caption: Infrastructure.MessageBox.Caption.Question,
                        text: "آیا مطمئن به حذف اعضای صندوق می‌باشید؟"
                    );

                if (oResult == System.Windows.MessageBoxResult.Yes)
                {
                    var varList = oUnitOfWork.MemberRepository
                        .Get()
                        .Where(current => current.FundId == Utility.CurrentFund.Id)
                        .ToList();

                    var varList2 = oUnitOfWork.RemainderRepository
                        .Get()
                        .Where(current => current.FundId == Utility.CurrentFund.Id)
                        .Where(current => current.EventType == Models.Event.Installment)
                        .ToList();

                    var varList3 = oUnitOfWork.TransactionRepository
                        .Get()
                        .Where(current => current.FundId == Utility.CurrentFund.Id)
                        .Where(current => current.TransactionType != Models.TransactionType.Deposit)
                        .ToList();

                    if (varList != null)
                    {
                        foreach (Models.Member oMember in varList)
                        {
                            oUnitOfWork.MemberRepository.Delete(oMember);

                            oUnitOfWork.Save();
                        }

                        Infrastructure.MessageBox.Show
                            (
                                caption: Infrastructure.MessageBox.Caption.Information,
                                text: "اعضای صندوق با موفقیت حذف شدند."
                            );
                    }
                    if (varList2 != null)
                    {
                        foreach (Models.Reminder oReminder in varList2)
                        {
                            oUnitOfWork.RemainderRepository.Delete(oReminder);

                            oUnitOfWork.Save();
                        }
                    }
                    if (varList3 != null)
                    {
                        foreach (Models.Transaction oTransaction in varList3)
                        {
                            oUnitOfWork.TransactionRepository.Delete(oTransaction);

                            oUnitOfWork.Save();
                        }
                    }

                }
                if (oResult == System.Windows.MessageBoxResult.No)
                {
                    return;
                }

                oUnitOfWork.Save();

                Utility.CurrentMember = null;
                Utility.CurrentLoan = null;

                Utility.MainWindow.RefreshUserInterface();
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

        private void DeleteLoans_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                System.Windows.MessageBoxResult oResult =
                    Infrastructure.MessageBox.Show
                    (
                        caption: Infrastructure.MessageBox.Caption.Question,
                        text: "آیا مطمئن به حذف وام‌های صندوق می‌باشید؟"
                    );

                if (oResult == System.Windows.MessageBoxResult.Yes)
                {
                    var varList = oUnitOfWork.LoanRepository
                        .Get()
                        .Where(current => current.Member.FundId == Utility.CurrentFund.Id)
                        .ToList();

                    var varList2 = oUnitOfWork.RemainderRepository
                        .Get()
                        .Where(current => current.FundId == Utility.CurrentFund.Id)
                        .Where(current => current.EventType == Models.Event.Installment)
                        .ToList();

                    var varTransactions = oUnitOfWork.TransactionRepository
                        .Get()
                        .Where(current => current.FundId == Utility.CurrentFund.Id)
                        .Where(current => current.TransactionType != Models.TransactionType.Deposit)
                        .ToList();

                    if (varList != null)
                    {
                        foreach (Models.Loan oLoan in varList)
                        {
                            oUnitOfWork.LoanRepository.Delete(oLoan);

                            oUnitOfWork.Save();
                        }

                        Infrastructure.MessageBox.Show
                            (
                                caption: Infrastructure.MessageBox.Caption.Information,
                                text: "وام های پرداخت شده صندوق با موفقیت حذف شدند."
                            );
                    }
                    if (varList2 != null)
                    {
                        foreach (Models.Reminder oReminder in varList2)
                        {
                            oUnitOfWork.RemainderRepository.Delete(oReminder);

                            oUnitOfWork.Save();
                        }
                    }
                    if (varTransactions != null)
                    {
                        foreach (Models.Transaction oTransaction in varTransactions)
                        {
                            oUnitOfWork.TransactionRepository.Delete(oTransaction);

                            oUnitOfWork.Save();
                        }

                    }
                }
                if (oResult == System.Windows.MessageBoxResult.No)
                {
                    return;
                }

                oUnitOfWork.Save();

                Utility.MainWindow.RefreshUserInterface();
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

        private void DeleteEvents_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                System.Windows.MessageBoxResult oResult =
                    Infrastructure.MessageBox.Show
                    (
                        caption: Infrastructure.MessageBox.Caption.Question,
                        text: "آیا مطمئن به حذف رویدادهای صندوق می‌باشید؟"
                    );

                if (oResult == System.Windows.MessageBoxResult.Yes)
                {
                    var varList = oUnitOfWork.RemainderRepository
                        .Get()
                        .Where(current => current.FundId == Utility.CurrentFund.Id)
                        .Where(current => current.EventType == Models.Event.Text)
                        .ToList();

                    if (varList != null)
                    {
                        foreach (Models.Reminder oReminder in varList)
                        {
                            oUnitOfWork.RemainderRepository.Delete(oReminder);

                            oUnitOfWork.Save();
                        }

                        Infrastructure.MessageBox.Show
                            (
                                caption: Infrastructure.MessageBox.Caption.Information,
                                text: "رویدادهای متنی اضافه شده به صندوق با موفقیت حذف شدند."
                        );

                        Utility.MainWindow.RefreshUserInterface();

                    }
                }
                if (oResult == System.Windows.MessageBoxResult.No)
                {
                    return;
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

        private void DeleteFund_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                System.Windows.MessageBoxResult oResult =
                    Infrastructure.MessageBox.Show
                    (
                        caption: Infrastructure.MessageBox.Caption.Question,
                        text: "آیا مطمئن به حذف صندوق می‌باشید؟"
                    );

                if (oResult == System.Windows.MessageBoxResult.Yes)
                {
                    Models.Fund oFund = oUnitOfWork.FundRepository
                        .GetById(Utility.CurrentFund.Id);

                    if (oFund != null)
                    {
                        oUnitOfWork.FundRepository.Delete(oFund);

                        oUnitOfWork.Save();

                        Infrastructure.MessageBox.Show
                            (
                                caption: Infrastructure.MessageBox.Caption.Information,
                                text: "صندوق با موفقیت حذف گردید."
                            );

                        Utility.CurrentFund = null;

                        Utility.MainWindow.RefreshUserInterface();
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

            this.Close();
        }
    }
}
