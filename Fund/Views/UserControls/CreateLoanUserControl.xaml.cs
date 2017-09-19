using System.Linq;

namespace Fund
{
    public partial class CreateLoanUserControl : System.Windows.Controls.UserControl
    {
        public CreateLoanUserControl()
        {
            InitializeComponent();

            RefreshListBox();
        }

        private void RefreshListBox()
        {
            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                MembersListBox.ItemsSource = oUnitOfWork.MemberRepository
                    .Get()
                    .Where(current => current.FundId == Utility.CurrentFund.Id)
                    .OrderBy(current => current.FullName.LastName)
                    .ThenBy(current => current.FullName.FirstName)
                    .ToList();

                MembersListBox.DisplayMemberPath = "FullName";
                MembersListBox.SelectedValuePath = "Id";

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

            MembersListBox.SelectedIndex = 0;
        }

        private void MembersListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Models.Member oMember = MembersListBox.SelectedItem as Models.Member;

            MemberViewGrid.Visibility = System.Windows.Visibility.Visible;

            if (oMember != null)
            {
                MemberNameLabel.Content = string.Format("{0} فرزند {1}", oMember.FullName, oMember.FatherName);
                MemberNationalCodeLabel.Content = string.Format("کد ملی {0}", oMember.NationalCode);
                MembershipDateLabel.Content = string.Format("تاریخ عضویت {0}", oMember.MembershipDate.ToPersianDate());

                var uriSource = new System.Uri(@"/Fund;component/Resources/Images/MemberPicture.png", System.UriKind.Relative);
                MemberImage.Source = (oMember.Picture == null) ? new System.Windows.Media.Imaging.BitmapImage(uriSource) : Utility.BytesToImage(oMember.Picture);

                Utility.CurrentMember = oMember;

                RefreshGridControl();
            }
        }

        private void SearchMemberTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(((System.Windows.Controls.TextBox)sender).Text) == true)
            {
                RefreshListBox();
            }
            else
            {
                string searchValue = ((System.Windows.Controls.TextBox)sender).Text;

                DAL.UnitOfWork oUnitOfWork = null;

                try
                {
                    oUnitOfWork = new DAL.UnitOfWork();

                    var varData = oUnitOfWork.MemberRepository
                        .Get()
                        .Where(current => current.FundId == Utility.CurrentFund.Id)
                        .AsQueryable();

                    searchValue = searchValue.Trim();

                    while (searchValue.Contains("  "))
                    {
                        searchValue = searchValue.Replace("  ", " ");
                    }

                    var varKeywords = searchValue.Split(' ').Distinct();

                    foreach (string strKeyword in varKeywords)
                    {
                        varData =
                            varData
                            .Where(current => current.FullName.FirstName.Contains(strKeyword) || current.FullName.LastName.Contains(strKeyword))
                            ;
                    }

                    var varResult = varData
                        .OrderBy(current => current.FullName.LastName)
                        .ThenBy(current => current.FullName.FirstName)
                        .ToList();

                    MembersListBox.ItemsSource = varResult;
                    MembersListBox.DisplayMemberPath = "FullName";
                    MembersListBox.SelectedValuePath = "Id";

                    if (varResult.Count > 0)
                    {
                        MembersListBox.SelectedIndex = 0;
                        MemberViewGrid.Visibility = System.Windows.Visibility.Visible;
                    }
                    else
                    {
                        MemberViewGrid.Visibility = System.Windows.Visibility.Hidden;
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
            }
        }

        private void TextEdit_EditorActivated(object sender, System.Windows.RoutedEventArgs e)
        {
            ((DevExpress.Xpf.Editors.TextEdit)sender).Text =
                ((DevExpress.Xpf.Editors.TextEdit)sender).Text.Replace(" ریال", string.Empty).Replace(",", string.Empty);
        }

        private void TextEdit_Validate(object sender, DevExpress.Xpf.Editors.ValidationEventArgs e)
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
        }

        private void AcceptButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {

            #region Error Handling Messages

            if (string.IsNullOrWhiteSpace(InstallmentsCountTextBox.Text) == true)
            {
                Infrastructure.MessageBox.Show
                    (
                        caption: Infrastructure.MessageBoxCaption.Error,
                        text: "درج فیلد تعداد اقساط الزامی است"
                    );

                return;
            }

            if (LoanAmountTextBox.Text.StringToMoney() == 0)
            {
                Infrastructure.MessageBox.Show
                    (
                        caption: Infrastructure.MessageBoxCaption.Error,
                        text: "درج مبلغ وام الزامی است"
                    );

                return;
            }

            if ((LoanAmountTextBox.Text.StringToMoney()) % 100000 != 0)
            {
                Infrastructure.MessageBox.Show
                    (
                        caption: Infrastructure.MessageBoxCaption.Error,
                        text: "مبلغ بایست مضربی از 100,000 ریال باشد"
                    );

                return;
            }

            if (LoanAmountTextBox.Text.StringToMoney() > Utility.CurrentFund.RemovalLimit)
            {
                Infrastructure.MessageBox.Show
                    (
                        caption: Infrastructure.MessageBoxCaption.Error,
                        text: "مبلغ درخواست وام از سقف پرداخت وام صندوق بیشتر می‌باشد."
                    );

                return;
            }

            if (LoanAmountTextBox.Text.StringToMoney() > Utility.CurrentFund.Balance)
            {
                Infrastructure.MessageBox.Show
                    (
                        caption: Infrastructure.MessageBoxCaption.Error,
                        text: "میزان موجودی صندوق کافی نمی باشد. " + System.Environment.NewLine + "نسبت به افزایش موجودی صندوق اقدام  کنید."
                    );

                return;
            }

            #endregion

            System.Threading.Thread oThread = new System.Threading.Thread(new System.Threading.ThreadStart(Transaction));

            oThread.Start();
        }

        private void InstallmentsOfLoan_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ViewModels.LoanViewModel oViewModel = MembersLoanGridControl.SelectedItem as ViewModels.LoanViewModel;

            if (oViewModel != null)
            {
                DAL.UnitOfWork oUnitOfWork = null;

                oUnitOfWork = new DAL.UnitOfWork();

                Utility.CurrentLoan = oUnitOfWork.LoanRepository
                    .GetById(oViewModel.Id);

                ShowInstallmentsListPerLoanWindow oShowInstallmentsListPerLoanWindow =
                    new ShowInstallmentsListPerLoanWindow();

                oShowInstallmentsListPerLoanWindow.ShowDialog();
            }
        }

        private void RefreshGridControl()
        {
            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                var varList = oUnitOfWork.LoanRepository
                    .Get()
                    .Where(current => current.MemberId == Utility.CurrentMember.Id)
                    .OrderBy(current => current.StartDate)
                    .Select(current => new ViewModels.LoanViewModel()
                    {
                        Id = current.Id,
                        StartDate = current.StartDate,
                        EndDate = current.EndDate,
                        IsPayed = current.IsPayed,
                        LoanAmount = current.LoanAmount,
                        RefundAmount = current.RefundAmount,
                        InstallmentsCount = current.InstallmentsCount,
                        Description = current.Description,
                        MemberId = current.MemberId,
                    })
                    .ToList();

                MembersLoanGridControl.ItemsSource = varList;

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


        private ViewModels.CreateLoanViewModel CalculateLoanParameters(long LoanAmount, int InstallmentsCount, int Percent = 0)
        {
            ViewModels.CreateLoanViewModel oCreateLoanViewModel = new ViewModels.CreateLoanViewModel();

            oCreateLoanViewModel.InstallmentsCount = InstallmentsCount;
            oCreateLoanViewModel.LoanAmount = LoanAmount;
            oCreateLoanViewModel.CommissionAmount = (Percent == 0) ? 0 : (long)System.Math.Round(((double)Percent / 100) * LoanAmount);
            oCreateLoanViewModel.RefundAmount = oCreateLoanViewModel.LoanAmount + oCreateLoanViewModel.CommissionAmount;

            LoanAmount += oCreateLoanViewModel.CommissionAmount;

            long InstallmentValue = 0;

            decimal temp = 0;

            if (LoanAmount % InstallmentsCount == 0)
            {
                temp = LoanAmount / InstallmentsCount;

                while (LoanAmount >= temp)
                {
                    oCreateLoanViewModel.Installments.Add((long)temp);

                    LoanAmount -= (long)temp;
                }
            }
            else
            {
                temp = LoanAmount / InstallmentsCount;

                InstallmentValue = (long)System.Math.Round((System.Math.Round(temp) / 10000)) * 10000;

                while (LoanAmount >= InstallmentValue)
                {
                    oCreateLoanViewModel.Installments.Add(InstallmentValue);

                    LoanAmount -= InstallmentValue;
                }

                oCreateLoanViewModel.Installments.Add(LoanAmount);
            }

            return oCreateLoanViewModel;
        }

        private void Transaction()
        {
            App.Current.Dispatcher.Invoke((System.Action)(() =>
            {
                DAL.UnitOfWork oUnitOfWork = null;

                try
                {
                    oUnitOfWork = new DAL.UnitOfWork();

                    Models.Fund oFund = oUnitOfWork.FundRepository
                        .GetById(Utility.CurrentFund.Id);

                    Models.Loan oLoan = new Models.Loan();

                    oLoan.LoanAmount = LoanAmountTextBox.Text.StringToMoney();
                    oLoan.IsActive = true;
                    oLoan.IsPayed = false;
                    oLoan.MemberId = Utility.CurrentMember.Id;
                    oLoan.InstallmentsCount = System.Convert.ToInt32(InstallmentsCountTextBox.Text.Trim());
                    oLoan.RefundAmount = LoanAmountTextBox.Text.StringToMoney();
                    oLoan.StartDate = LoanDateTimeDatePicker.SelectedDateTime;
                    oLoan.EndDate = LoanDateTimeDatePicker.SelectedDateTime;
                    oLoan.Description = DescriptionTextBox.Text.Trim();

                    oUnitOfWork.LoanRepository.Insert(oLoan);

                    oFund.Balance -= oLoan.LoanAmount;

                    oUnitOfWork.FundRepository.Update(oFund);

                    oUnitOfWork.Save();

                    Utility.CurrentFund = oFund;

                    Models.Transaction oTransaction = new Models.Transaction();

                    oTransaction.Amount = oLoan.LoanAmount;
                    oTransaction.Balance = oFund.Balance;
                    oTransaction.Date = System.DateTime.Now;
                    oTransaction.Description = string.Format("پرداخت وام به مبلغ {0} ریال به {1}", oLoan.LoanAmount, Utility.CurrentMember.FullName);
                    oTransaction.FundId = Utility.CurrentFund.Id;
                    oTransaction.TransactionType = Models.TransactionType.Loan;
                    oTransaction.MemberId = Utility.CurrentMember.Id;

                    oUnitOfWork.TransactionRepository.Insert(oTransaction);

                    int percent = (CalculatePercentCheckBox.IsChecked == true) ? Utility.CurrentFund.Percent : 0;

                    ViewModels.CreateLoanViewModel oCreateLoanViewModel = CalculateLoanParameters(oLoan.LoanAmount, oLoan.InstallmentsCount, percent);

                    for (int index = 0; index < oLoan.InstallmentsCount; index++)
                    {
                        Models.Installment oInstallment = new Models.Installment();

                        oInstallment.PaymentAmount = oCreateLoanViewModel.Installments.ElementAt(index);
                        oInstallment.IsPayed = false;
                        oInstallment.InstallmentDate = oLoan.StartDate.AddMonths(index + 1);
                        oInstallment.PaymentDate = null;
                        oInstallment.LoanId = oLoan.Id;

                        oUnitOfWork.InstallmentRepository.Insert(oInstallment);

                        Models.Reminder oReminder = new Models.Reminder();

                        FarsiLibrary.Utils.PersianDate oPersianDate = new FarsiLibrary.Utils.PersianDate(oInstallment.InstallmentDate);

                        oReminder.DateTime = oInstallment.InstallmentDate;
                        oReminder.PersianDate.Year = oPersianDate.Year;
                        oReminder.PersianDate.Month = oPersianDate.Month;
                        oReminder.PersianDate.Day = oPersianDate.Day;
                        oReminder.EventType = Models.Event.Installment;
                        oReminder.Description = string.Format("قسط {0} وام {1} به مبلغ {2}",
                            FarsiLibrary.Utils.ToWords.ToString(index + 1), Utility.CurrentMember.FullName, oInstallment.PaymentAmount.ToRialStringFormat());
                        oReminder.InstallmentId = oInstallment.Id;
                        oReminder.FundId = Utility.CurrentFund.Id;

                        oUnitOfWork.RemainderRepository.Insert(oReminder);

                        if (index == oLoan.InstallmentsCount - 1)
                        {
                            oLoan.EndDate = oInstallment.InstallmentDate;
                            oLoan.RefundAmount = oCreateLoanViewModel.RefundAmount;

                            oUnitOfWork.LoanRepository.Update(oLoan);
                        }

                        oUnitOfWork.Save();
                    }


                    (Utility.MainWindow.SthPanel.Children[0] as MainPanelContentUserControl).MiniPersianSchedulerReminder.RefreshMonth();
                    (Utility.MainWindow.SthPanel.Children[0] as MainPanelContentUserControl).RefreshSchedulerListBox();

                    Infrastructure.MessageBox.Show
                        (
                            caption: Infrastructure.MessageBoxCaption.Information,
                            text: "اطلاعات وام با موفقیت در سیستم ثبت گردید."
                        );
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
            }));

        }

        private void CancelButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
