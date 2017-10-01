using System.Linq;

namespace Fund
{
    public partial class MemberBalanceWindow : DevExpress.Xpf.Core.DXWindow
    {

        public MemberBalanceWindow()
        {
            InitializeComponent();
        }

        private void PrintButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ShowReport(Infrastructure.Report.ExportType.Print);

            this.Close();
        }

        private void ExportToPdfButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ShowReport(Infrastructure.Report.ExportType.ExportToPDF);

            this.Close();
        }

        private void ShowReport(Infrastructure.Report.ExportType reportType)
        {
            var varObject = new
            {
                LoansCount = LoansCountLabel.Content,
                LoansAmount = LoansAmountLabel.Content,
                PayedLoansCount = PayedLoansCountLabel.Content,
                PayedLoansAmount = PayedLoansAmountLabel.Content,
                InstallmentsCount = InstallmentsCountLabel.Content,
                InstallmentsAmount = InstallmentsAmountLabel.Content,
                PayedInstallmentsCount = PayedInstallmentsCountLabel.Content,
                PayedInstallmentsAmount = PayedInstallmentsAmountLabel.Content,
            };

            Stimulsoft.Report.StiReport oStiReport = new Stimulsoft.Report.StiReport();

            oStiReport.Load(Properties.Resources.MemberBalanceReport);

            oStiReport.Dictionary.Variables.Add("Today", System.DateTime.Now.ToPersianDate());
            oStiReport.Dictionary.Variables.Add("FundName", Utility.CurrentFund.Name);
            oStiReport.Dictionary.Variables.Add("FundManagerName", Utility.CurrentFund.ManagerName);
            oStiReport.Dictionary.Variables.Add("MemberName", Utility.CurrentMember.FullName.ToString());

            oStiReport.RegBusinessObject("MemberBalance", varObject);
            oStiReport.Compile();
            oStiReport.RenderWithWpf();

            oStiReport.DoAction(action: reportType, fileName: "تراز مالی " + Utility.CurrentMember.FullName.ToString());
        }

        private void SendEmailButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            bool internetConnected = Utility.CheckForInternetConnection();

            if (internetConnected == true)
            {
                EnterEmailPasswordWindow oEnterEmailPasswordWindow = new EnterEmailPasswordWindow();

                if (oEnterEmailPasswordWindow.ShowDialog() == true)
                {
                    Utility.MainWindow.MainProgressBar.IsIndeterminate = true;
                    Utility.MainWindow.MainProgressBar.Visibility = System.Windows.Visibility.Visible;

                    System.ComponentModel.BackgroundWorker oBackgroundWorker = new System.ComponentModel.BackgroundWorker();

                    oBackgroundWorker.DoWork += OBackgroundWorker_DoWork;
                    oBackgroundWorker.RunWorkerAsync(oEnterEmailPasswordWindow);
                }
                else
                {
                    Utility.MainWindow.MainProgressBar.IsIndeterminate = false;
                    Utility.MainWindow.MainProgressBar.Visibility = System.Windows.Visibility.Hidden;

                    return;
                }
            }
            else
            {
                Utility.MainWindow.MainProgressBar.IsIndeterminate = false;
                Utility.MainWindow.MainProgressBar.Visibility = System.Windows.Visibility.Hidden;

                Infrastructure.MessageBox.Show
                    (
                        caption: Infrastructure.MessageBox.Caption.Error,
                        text: "اتصال به اینترنت برقرار نمی‌باشد. از اتصال دستگاه خود با اینترنت اطمینان حاصل فرمایید."
                    );

                return;
            }
        }

        private void OBackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                EnterEmailPasswordWindow oEnterEmailPasswordWindow = e.Argument as EnterEmailPasswordWindow;

                var varObject = new
                {
                    LoansCount = LoansCountLabel.Content,
                    LoansAmount = LoansAmountLabel.Content,
                    PayedLoansCount = PayedLoansCountLabel.Content,
                    PayedLoansAmount = PayedLoansAmountLabel.Content,
                    InstallmentsCount = InstallmentsCountLabel.Content,
                    InstallmentsAmount = InstallmentsAmountLabel.Content,
                    PayedInstallmentsCount = PayedInstallmentsCountLabel.Content,
                    PayedInstallmentsAmount = PayedInstallmentsAmountLabel.Content,
                };

                Stimulsoft.Report.StiReport oStiReport = new Stimulsoft.Report.StiReport();

                oStiReport.Load(Properties.Resources.MemberBalanceReport);

                oStiReport.Dictionary.Variables.Add("Today", System.DateTime.Now.ToPersianDate());
                oStiReport.Dictionary.Variables.Add("FundName", Utility.CurrentFund.Name);
                oStiReport.Dictionary.Variables.Add("FundManagerName", Utility.CurrentFund.ManagerName);
                oStiReport.Dictionary.Variables.Add("MemberName", Utility.CurrentMember.FullName.ToString());

                oStiReport.RegBusinessObject("MemberBalance", varObject);
                oStiReport.Compile();
                oStiReport.RenderWithWpf();

                System.IO.MemoryStream oMemoryStream = new System.IO.MemoryStream();

                oStiReport.ExportDocument(Stimulsoft.Report.StiExportFormat.Pdf, oMemoryStream);
                oMemoryStream.Seek(0, System.IO.SeekOrigin.Begin);

                Utility.SendEmail
                (
                    senderEmail: Utility.CurrentUser.EmailAddress,
                    senderPassword: oEnterEmailPasswordWindow.EmailPassword,
                    displayName: Utility.CurrentUser.FullName.ToString(),
                    receiverEmail: Utility.CurrentMember.EmailAddress,
                    subject: Utility.CurrentFund.Name + " | " + "تراز مالی عضو  " + Utility.CurrentMember.FullName.ToString(),
                    body: "تراز مالی " + Utility.CurrentMember.FullName.ToString() +
                            System.Environment.NewLine +
                            new FarsiLibrary.Utils.PersianDate(System.DateTime.Now).ToString(),
                    attachment: oMemoryStream,
                    attachmentName: "تراز مالی " + Utility.CurrentMember.FullName.ToString()
                );
            });
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
                System.Windows.MessageBox.Show(ex.Message); ;
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

        private void ShowInformation()
        {
            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                ViewModels.MemberBalanceViewModel oViewModel = new ViewModels.MemberBalanceViewModel();

                oViewModel.LoansCount = oUnitOfWork.LoanRepository
                    .Get()
                    .Where(current => current.MemberId == Utility.CurrentMember.Id)
                    .Count();

                oViewModel.PayedLoansCount = oUnitOfWork.LoanRepository
                    .Get()
                    .Where(current => current.MemberId == Utility.CurrentMember.Id)
                    .Where(current => current.IsPayed == true)
                    .Count();

                oViewModel.InstallmentsCount = oUnitOfWork.InstallmentRepository
                    .Get()
                    .Where(current => current.Loan.MemberId == Utility.CurrentMember.Id)
                    .Count();

                oViewModel.PayedInstallmentsCount = oUnitOfWork.InstallmentRepository
                    .Get()
                    .Where(current => current.Loan.MemberId == Utility.CurrentMember.Id)
                    .Where(current => current.IsPayed == true)
                    .Count();

                oViewModel.LoansAmount = oUnitOfWork.LoanRepository
                    .Get()
                    .Where(current => current.MemberId == Utility.CurrentMember.Id)
                    .Select(current => current.LoanAmount)
                    .DefaultIfEmpty(0)
                    .Sum();

                oViewModel.PayedLoansAmount = oUnitOfWork.LoanRepository
                    .Get()
                    .Where(current => current.MemberId == Utility.CurrentMember.Id)
                    .Where(current => current.IsPayed == true)
                    .Select(current => current.LoanAmount)
                    .DefaultIfEmpty(0)
                    .Sum();

                oViewModel.InstallmentsAmount = oUnitOfWork.InstallmentRepository
                    .Get()
                    .Where(current => current.Loan.MemberId == Utility.CurrentMember.Id)
                    .Select(current => current.PaymentAmount)
                    .DefaultIfEmpty(0)
                    .Sum();

                oViewModel.PayedInstallmentsAmount = oUnitOfWork.InstallmentRepository
                    .Get()
                    .Where(current => current.Loan.MemberId == Utility.CurrentMember.Id)
                    .Where(current => current.IsPayed == true)
                    .Select(current => current.PaymentAmount)
                    .DefaultIfEmpty(0)
                    .Sum();

                LoansCountLabel.Content = string.Format("{0} وام", oViewModel.LoansCount);
                LoansAmountLabel.Content = oViewModel.LoansAmount.ToRialStringFormat();
                PayedLoansCountLabel.Content = string.Format("{0} وام", oViewModel.PayedLoansCount);
                PayedLoansAmountLabel.Content = oViewModel.PayedLoansAmount.ToRialStringFormat();
                InstallmentsCountLabel.Content = string.Format("{0} قسط", oViewModel.InstallmentsCount);
                InstallmentsAmountLabel.Content = oViewModel.InstallmentsAmount.ToRialStringFormat();
                PayedInstallmentsCountLabel.Content = string.Format("{0} قسط", oViewModel.PayedInstallmentsCount);
                PayedInstallmentsAmountLabel.Content = oViewModel.PayedInstallmentsAmount.ToRialStringFormat();

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

                ShowInformation();
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

                        ExportToPdfButton.IsEnabled = true;
                        PrintButton.IsEnabled = true;
                        SendEmailButton.IsEnabled = true;
                    }
                    else
                    {
                        MemberViewGrid.Visibility = System.Windows.Visibility.Hidden;

                        LoansCountLabel.Content = string.Empty;
                        LoansAmountLabel.Content = string.Empty;
                        PayedLoansCountLabel.Content = string.Empty;
                        PayedLoansAmountLabel.Content = string.Empty;
                        InstallmentsCountLabel.Content = string.Empty;
                        InstallmentsAmountLabel.Content = string.Empty;
                        PayedInstallmentsCountLabel.Content = string.Empty;
                        PayedInstallmentsAmountLabel.Content = string.Empty;

                        ExportToPdfButton.IsEnabled = false;
                        PrintButton.IsEnabled = false;
                        SendEmailButton.IsEnabled = false;
                    }

                    oUnitOfWork.Save();
                }
                catch (System.Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message); ;
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

        private void DXWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            RefreshListBox();
        }
    }
}
