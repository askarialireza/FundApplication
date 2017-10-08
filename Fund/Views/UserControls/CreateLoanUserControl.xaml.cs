using System.Linq;

namespace Fund
{
    public partial class CreateLoanUserControl : System.Windows.Controls.UserControl
    {
        public CreateLoanUserControl()
        {
            InitializeComponent();

            RefreshListBox();

            IsConnectedToInternet = false;
        }

        private bool IsConnectedToInternet;

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
                        AcceptButton.IsEnabled = true;
                    }
                    else
                    {
                        MemberViewGrid.Visibility = System.Windows.Visibility.Hidden;
                        MembersLoanGridControl.ItemsSource = null;
                        AcceptButton.IsEnabled = false;

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

        private void AcceptButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            #region Error Handling Messages

            if (string.IsNullOrWhiteSpace(InstallmentsCountTextBox.Text) == true)
            {
                Infrastructure.MessageBox.Show
                    (
                        caption: Infrastructure.MessageBox.Caption.Error,
                        text: "درج فیلد تعداد اقساط الزامی است"
                    );

                return;
            }

            if (LoanAmountTextBox.Text.StringToMoney() == 0)
            {
                Infrastructure.MessageBox.Show
                    (
                        caption: Infrastructure.MessageBox.Caption.Error,
                        text: "درج مبلغ وام الزامی است"
                    );

                return;
            }

            if ((LoanAmountTextBox.Text.StringToMoney()) % 100000 != 0)
            {
                Infrastructure.MessageBox.Show
                    (
                        caption: Infrastructure.MessageBox.Caption.Error,
                        text: "مبلغ بایست مضربی از 100,000 ریال باشد"
                    );

                return;
            }

            if (LoanAmountTextBox.Text.StringToMoney() > Utility.CurrentFund.RemovalLimit)
            {
                Infrastructure.MessageBox.Show
                    (
                        caption: Infrastructure.MessageBox.Caption.Error,
                        text: "مبلغ درخواست وام از سقف پرداخت وام صندوق بیشتر می‌باشد."
                    );

                return;
            }

            if (LoanAmountTextBox.Text.StringToMoney() > Utility.CurrentFund.Balance)
            {
                Infrastructure.MessageBox.Show
                    (
                        caption: Infrastructure.MessageBox.Caption.Error,
                        text: "میزان موجودی صندوق کافی نمی باشد. " + System.Environment.NewLine + "نسبت به افزایش موجودی صندوق اقدام  کنید."
                    );

                return;
            }

            DAL.UnitOfWork oUnitOfWork = new DAL.UnitOfWork();

            var varList = oUnitOfWork.LoanRepository
                .Get()
                .Where(current => current.MemberId == Utility.CurrentMember.Id)
                .Where(current => current.IsActive == true)
                .Where(current => current.StartDate <= LoanDateTimeDatePicker.SelectedDateTime)
                .ToList();

            if (varList.Count != 0)
            {
                Infrastructure.MessageBox.Show
                    (
                        caption: Infrastructure.MessageBox.Caption.Error,
                        text: "پرداخت وام امکان پذیر نمی‌باشد ." + System.Environment.NewLine + "نسبت به تسویه کامل اقساط وام‌های قبلی اقدام نمایید."
                    );

                return;
            }

            #endregion

            Utility.MainWindow.MainProgressBar.Visibility = System.Windows.Visibility.Visible;

            System.ComponentModel.BackgroundWorker oBackgroundWorker = new System.ComponentModel.BackgroundWorker();

            oBackgroundWorker.WorkerReportsProgress = true;

            oBackgroundWorker.DoWork += OBackgroundWorker_DoWork;

            oBackgroundWorker.ProgressChanged += OBackgroundWorker_ProgressChanged;

            oBackgroundWorker.RunWorkerCompleted += OBackgroundWorker_RunWorkerCompleted;

            oBackgroundWorker.RunWorkerAsync();
        }

        private void OBackgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            System.ComponentModel.BackgroundWorker oEmailBackgroundWorker = new System.ComponentModel.BackgroundWorker();

            oEmailBackgroundWorker.WorkerReportsProgress = true;

            oEmailBackgroundWorker.DoWork += OEmailBackgroundWorker_DoWork;

            oEmailBackgroundWorker.RunWorkerAsync();

            Utility.MainWindow.MainProgressBar.IsIndeterminate = true;

            Utility.MainWindow.MainProgressBar.Visibility = System.Windows.Visibility.Visible;

            (Utility.MainWindow.SthPanel.Children[0] as MainPanelContentUserControl).MiniPersianSchedulerReminder.RefreshMonth();
            (Utility.MainWindow.SthPanel.Children[0] as MainPanelContentUserControl).RefreshSchedulerListBox();

            Infrastructure.MessageBox.Show
                (
                    caption: Infrastructure.MessageBox.Caption.Information,
                    text: "اطلاعات وام با موفقیت در سیستم ثبت گردید."
                );

            Utility.MainWindow.RefreshUserInterface();

            RefreshGridControl();
        }

        private void OEmailBackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                DAL.UnitOfWork oUnitOfWork = new DAL.UnitOfWork();

                if (SendEmailToggleSwitch.IsChecked == true && IsConnectedToInternet == true)
                {
                    EnterEmailPasswordWindow oEnterEmailPasswordWindow = new EnterEmailPasswordWindow();

                    if (oEnterEmailPasswordWindow.ShowDialog() == true)
                    {
                        var varList = oUnitOfWork.InstallmentRepository
                        .Get()
                        .Where(current => current.LoanId == Utility.CurrentLoan.Id)
                        .Select(current => new ViewModels.InstallmentViewModel()
                        {
                            Id = current.Id,
                            Amount = current.PaymentAmount,
                            InstallmentDate = current.InstallmentDate,
                            PaymentDate = current.PaymentDate,
                            IsActive = !(current.IsPayed),
                            IsPayed = current.IsPayed,
                        })
                        .OrderBy(current => current.InstallmentDate)
                        .ToList()
                        .Select(current => new
                        {
                            current.AmountRialFormat,
                            current.PersianInstallmentDate,
                            current.PersianPaymentDate,
                            current.IsPayedDescription,
                        })
                        .ToList();

                        Stimulsoft.Report.StiReport oStiReport = new Stimulsoft.Report.StiReport();

                        oStiReport.Load(Properties.Resources.InstallmentListReport);

                        oStiReport.Dictionary.Variables.Add("Today", System.DateTime.Now.ToPersianDate());
                        oStiReport.Dictionary.Variables.Add("FundName", Utility.CurrentFund.Name);
                        oStiReport.Dictionary.Variables.Add("FundManagerName", Utility.CurrentFund.ManagerName);
                        oStiReport.Dictionary.Variables.Add("MemberName", Utility.CurrentMember.FullName.ToString());
                        oStiReport.Dictionary.Variables.Add("LoanAmount", (new ViewModels.LoanViewModel { LoanAmount = Utility.CurrentLoan.LoanAmount }).LoanAmountRialFormat);
                        oStiReport.Dictionary.Variables.Add("RefundAmount", (new ViewModels.LoanViewModel { RefundAmount = Utility.CurrentLoan.RefundAmount }).RefundAmountRialFormat);

                        oStiReport.RegBusinessObject("InstallmentsList", varList);
                        oStiReport.Compile();
                        oStiReport.RenderWithWpf(); oStiReport.WriteToReportRenderingMessages("در حال تهیه گزارش ...");

                        System.IO.MemoryStream oMemoryStream = new System.IO.MemoryStream();

                        oStiReport.ExportDocument(Stimulsoft.Report.StiExportFormat.Pdf, oMemoryStream);
                        oMemoryStream.Seek(0, System.IO.SeekOrigin.Begin);

                        Utility.SendEmail
                        (
                            senderEmail: Utility.CurrentUser.EmailAddress,
                            senderPassword: oEnterEmailPasswordWindow.EmailPassword,
                            displayName: Utility.CurrentUser.FullName.ToString(),
                            receiverEmail: Utility.CurrentMember.EmailAddress,
                            subject: Utility.CurrentFund.Name + " | " + "پرداخت وام" + Utility.CurrentMember.FullName.ToString(),
                            body: "لیست اقساط وام پرداخت شده توسط صندوق به " + Utility.CurrentMember.FullName.ToString() +
                            System.Environment.NewLine +
                            new FarsiLibrary.Utils.PersianDate(System.DateTime.Now).ToString(),
                            attachment: oMemoryStream,
                            attachmentName: "لیست اقساط وام پرداخت شده"
                        );
                    }
                    else
                    {
                        Utility.MainWindow.MainProgressBar.IsIndeterminate = false;
                        Utility.MainWindow.MainProgressBar.Visibility = System.Windows.Visibility.Hidden;
                    }
                }
                else
                {
                    Utility.MainWindow.MainProgressBar.IsIndeterminate = false;
                    Utility.MainWindow.MainProgressBar.Visibility = System.Windows.Visibility.Hidden;
                }
            });
        }

        private void OBackgroundWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            Utility.MainWindow.MainProgressBar.Value = e.ProgressPercentage;
        }

        private void OBackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            DAL.UnitOfWork oUnitOfWork = null;

            oUnitOfWork = new DAL.UnitOfWork();

            Models.Fund oFund = oUnitOfWork.FundRepository
                .GetById(Utility.CurrentFund.Id);

            Models.Loan oLoan = new Models.Loan();

            this.Dispatcher.Invoke(() =>
            {
                oLoan.LoanAmount = LoanAmountTextBox.Text.StringToMoney();
                oLoan.IsActive = true;
                oLoan.IsPayed = false;
                oLoan.MemberId = Utility.CurrentMember.Id;
                oLoan.InstallmentsCount = System.Convert.ToInt32(InstallmentsCountTextBox.Text.Trim());
                oLoan.RefundAmount = LoanAmountTextBox.Text.StringToMoney();
                oLoan.StartDate = LoanDateTimeDatePicker.SelectedDateTime;
                oLoan.EndDate = LoanDateTimeDatePicker.SelectedDateTime;
                oLoan.Description = DescriptionTextBox.Text.Trim();
            });

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
            oTransaction.LoanId = oLoan.Id;

            oUnitOfWork.TransactionRepository.Insert(oTransaction);

            int percent = 0;

            this.Dispatcher.Invoke(() =>
            {
                percent = (CalculatePercentCheckBox.IsChecked == true) ? Utility.CurrentFund.Percent : 0;
            });

            ViewModels.CreateLoanViewModel oCreateLoanViewModel = CalculateLoanParameters(oLoan.LoanAmount, oLoan.InstallmentsCount, percent);

            for (int index = 0; index < oLoan.InstallmentsCount; index++)
            {
                int progressBarPercent = System.Convert.ToInt32(((double)(index + 1) / oLoan.InstallmentsCount) * 100);

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

                (sender as System.ComponentModel.BackgroundWorker).ReportProgress(progressBarPercent);

            }

            Utility.CurrentLoan = oLoan;

            oUnitOfWork.Save();
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

        private void CancelButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Close();
        }

        private void SendEmailToggleSwitch_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            bool isConnected = Utility.CheckForInternetConnection();

            if (isConnected == true)
            {
                IsConnectedToInternet = true;
            }
            else
            {
                IsConnectedToInternet = false;

                Infrastructure.MessageBox.Show
                    (
                        caption: Infrastructure.MessageBox.Caption.Error,
                        text: "اتصال به اینترنت برقرار نمی‌باشد." + System.Environment.NewLine + "برای ارسال ایمیل به اعضا نیاز به اینترنت می‌باشد."
                    );

                ((System.Windows.Controls.CheckBox)sender).IsChecked = false;
            }
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            CalculatePercentCheckBox.IsEnabled = (Utility.CurrentFund.Percent > 0) ? true : false;
        }
    }
}
