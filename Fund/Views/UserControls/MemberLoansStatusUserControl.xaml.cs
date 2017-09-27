using System.Linq;

namespace Fund
{
    public partial class MemberLoansStatusUserControl : System.Windows.Controls.UserControl
    {
        public MemberLoansStatusUserControl()
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

        private void CancelButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Close();
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

                RefreshGridControl(typeIndex: 0, memberId: Utility.CurrentMember.Id);
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

        private void RefreshGridControl(int typeIndex, System.Guid memberId)
        {
            LoanStatusComboBox.SelectedIndex = typeIndex;

            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                var varData = oUnitOfWork.LoanRepository
                    .Get()
                    .Where(current => current.MemberId == memberId)
                    .OrderBy(current => current.StartDate)
                    .AsQueryable();

                switch (typeIndex)
                {
                    case 0:
                        {
                            break;
                        }
                    case 1:
                        {
                            varData = varData
                                .Where(current => current.IsActive == true)
                                .Where(current => current.IsPayed == false)
                                ;

                            break;
                        }
                    case 2:
                        {
                            varData = varData
                                .Where(current => current.IsActive == false)
                                .Where(current => current.IsPayed == true)
                                ;

                            break;
                        }
                }

                var varList = varData
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

                ExportToPdfButton.IsEnabled = (varList.Count == 0) ? false : true;
                PrintButton.IsEnabled = (varList.Count == 0) ? false : true;

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

        private void LoanStatusComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (Utility.CurrentMember != null)
            {
                RefreshGridControl(LoanStatusComboBox.SelectedIndex, Utility.CurrentMember.Id);
            }
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

        private void PrintButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ShowReport(reportType: Infrastructure.ReportType.Print);
        }

        private void ExportToPdfButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ShowReport(reportType: Infrastructure.ReportType.ExportToPDF);
        }

        private void ShowReport(Infrastructure.ReportType reportType)
        {
            var varList = (MembersLoanGridControl.ItemsSource as System.Collections.Generic.List<ViewModels.LoanViewModel>)
                .OrderBy(current => current.StartDate)
                .Select(current => new
                {
                    current.LoanAmountRialFormat,
                    current.RefundAmountRialFormat,
                    current.PersianEndDate,
                    current.PersianStartDate,
                    current.InstallmentsCount,
                    current.IsPayedDescrption,
                    current.Description,
                })
                .ToList();

            if (varList.Count == 0)
            {
                Infrastructure.MessageBox.Show
                    (
                        caption: Infrastructure.Caption.Error,
                        text: "اطلاعاتی برای تهیه گزارش در جدول موجود نمی‌باشد. "
                    );

                return;
            }

            Stimulsoft.Report.StiReport oStiReport = new Stimulsoft.Report.StiReport();

            oStiReport.Load(Properties.Resources.MembersLoansListReport);

            oStiReport.Dictionary.Variables.Add("Today", System.DateTime.Now.ToPersianDate());
            oStiReport.Dictionary.Variables.Add("FundName", Utility.CurrentFund.Name);
            oStiReport.Dictionary.Variables.Add("FundManagerName", Utility.CurrentFund.ManagerName);
            oStiReport.Dictionary.Variables.Add("MemberName", Utility.CurrentMember.FullName.ToString());
            oStiReport.Dictionary.Variables.Add("LoanStatus", LoanStatusComboBox.Text);
            oStiReport.RegBusinessObject("Loans", varList);
            oStiReport.Compile();
            oStiReport.RenderWithWpf();

            string fileName = string.Format("گزارش {0} {1}", LoanStatusComboBox.Text, Utility.CurrentMember.FullName.ToString());

            oStiReport.DoAction(action: reportType, fileName: fileName);
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
                        caption: Infrastructure.Caption.Error,
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

                var varList = (MembersLoanGridControl.ItemsSource as System.Collections.Generic.List<ViewModels.LoanViewModel>)
                .OrderBy(current => current.StartDate)
                .Select(current => new
                {
                    current.LoanAmountRialFormat,
                    current.RefundAmountRialFormat,
                    current.PersianEndDate,
                    current.PersianStartDate,
                    current.InstallmentsCount,
                    current.IsPayedDescrption,
                    current.Description,
                })
                .ToList();

                if (varList.Count == 0)
                {
                    Infrastructure.MessageBox.Show
                        (
                            caption: Infrastructure.Caption.Error,
                            text: "اطلاعاتی برای ارسال توسط پست الکترونیکی موجود نمی‌باشد. "
                        );

                    return;
                }

                Stimulsoft.Report.StiReport oStiReport = new Stimulsoft.Report.StiReport();

                oStiReport.Load(Properties.Resources.MembersLoansListReport);

                oStiReport.Dictionary.Variables.Add("Today", System.DateTime.Now.ToPersianDate());
                oStiReport.Dictionary.Variables.Add("FundName", Utility.CurrentFund.Name);
                oStiReport.Dictionary.Variables.Add("FundManagerName", Utility.CurrentFund.ManagerName);
                oStiReport.Dictionary.Variables.Add("MemberName", Utility.CurrentMember.FullName.ToString());
                oStiReport.Dictionary.Variables.Add("LoanStatus", LoanStatusComboBox.Text);
                oStiReport.RegBusinessObject("Loans", varList);
                oStiReport.Compile();
                oStiReport.RenderWithWpf();

                string fileName = string.Format("گزارش {0} {1}", LoanStatusComboBox.Text, Utility.CurrentMember.FullName.ToString());

                System.IO.MemoryStream oMemoryStream = new System.IO.MemoryStream();

                oStiReport.ExportDocument(Stimulsoft.Report.StiExportFormat.Pdf, oMemoryStream);
                oMemoryStream.Seek(0, System.IO.SeekOrigin.Begin);

                Utility.SendEmail
                (
                    senderEmail: Utility.CurrentUser.EmailAddress,
                    senderPassword: oEnterEmailPasswordWindow.EmailPassword,
                    displayName: Utility.CurrentUser.FullName.ToString(),
                    receiverEmail: Utility.CurrentMember.EmailAddress,
                    subject: Utility.CurrentFund.Name + " | " + LoanStatusComboBox.Text + " " + Utility.CurrentMember.FullName.ToString(),
                    body: LoanStatusComboBox.Text + Utility.CurrentMember.FullName.ToString() +
                            System.Environment.NewLine +
                            new FarsiLibrary.Utils.PersianDate(System.DateTime.Now).ToString(),
                    attachment: oMemoryStream,
                    attachmentName: fileName
                );
            });
        }
    }
}
