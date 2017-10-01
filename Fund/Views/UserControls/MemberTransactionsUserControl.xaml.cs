using System.Linq;

namespace Fund
{

    public partial class MemberTransactionsUserControl : System.Windows.Controls.UserControl
    {
        public MemberTransactionsUserControl()
        {
            InitializeComponent();
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
                        GridControl.ItemsSource = null;
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

                LoadGridControl();
            }
        }

        private void CancelButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Close();
        }

        private void PrintButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ShowReport(reportType: Infrastructure.Report.ExportType.Print);
        }

        private void ExportToPdfButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ShowReport(reportType: Infrastructure.Report.ExportType.ExportToPDF);
        }

        private void ShowReport(Infrastructure.Report.ExportType reportType)
        {
            var varList =
                (GridControl.ItemsSource as System.Collections.Generic.List<ViewModels.MemberTransactionViewModel>)
                .OrderBy(current => current.Date)
                .Select(current => new
                {
                    current.AmountRialFormat,
                    current.PersianDate,
                    current.TransactionDescription,
                })
                .ToList();

            Stimulsoft.Report.StiReport oStiReport = new Stimulsoft.Report.StiReport();

            oStiReport.Load(Properties.Resources.MemberTransactionsReport);
            oStiReport.Dictionary.Variables.Add("Today", System.DateTime.Now.ToPersianDate());
            oStiReport.Dictionary.Variables.Add("FundName", Utility.CurrentFund.Name);
            oStiReport.Dictionary.Variables.Add("FundManagerName", Utility.CurrentFund.ManagerName);
            oStiReport.Dictionary.Variables.Add("MemberName", Utility.CurrentMember.FullName.ToString());
            oStiReport.RegBusinessObject("Transactions", varList);
            oStiReport.Compile();
            oStiReport.RenderWithWpf();
            oStiReport.DoAction(action: reportType, fileName: "گزارش ریز حساب اعضا");
        }

        private void LoadGridControl()
        {
            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                var varList = oUnitOfWork.TransactionRepository
                    .Get()
                    .Where(current => current.FundId == Utility.CurrentFund.Id)
                    .Where(current => current.MemberId == Utility.CurrentMember.Id)
                    .OrderBy(current => current.Date)
                    .Select(current => new ViewModels.MemberTransactionViewModel()
                    {
                        Id = current.Id,
                        TransactionType = current.TransactionType,
                        Amount = current.Amount,
                        Balance = current.Balance,
                        Description = current.Description,
                        Date = current.Date,
                        FundId = current.FundId,
                        MemberId = current.MemberId,
                    })
                    .ToList();

                GridControl.ItemsSource = varList;

                ExportToPdfButton.IsEnabled = (varList.Count == 0) ? false : true;
                PrintButton.IsEnabled = (varList.Count == 0) ? false : true;

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

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            RefreshListBox();
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

                var varList =
                    (GridControl.ItemsSource as System.Collections.Generic.List<ViewModels.MemberTransactionViewModel>)
                    .OrderBy(current => current.Date)
                    .Select(current => new
                    {
                        current.AmountRialFormat,
                        current.PersianDate,
                        current.TransactionDescription,
                    })
                    .ToList();

                Stimulsoft.Report.StiReport oStiReport = new Stimulsoft.Report.StiReport();

                oStiReport.Load(Properties.Resources.MemberTransactionsReport);
                oStiReport.Dictionary.Variables.Add("Today", System.DateTime.Now.ToPersianDate());
                oStiReport.Dictionary.Variables.Add("FundName", Utility.CurrentFund.Name);
                oStiReport.Dictionary.Variables.Add("FundManagerName", Utility.CurrentFund.ManagerName);
                oStiReport.Dictionary.Variables.Add("MemberName", Utility.CurrentMember.FullName.ToString());
                oStiReport.RegBusinessObject("Transactions", varList);
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
                    subject: Utility.CurrentFund.Name + " | " + "لیست تراکنش های " + Utility.CurrentMember.FullName.ToString(),
                    body: "لیست تراکنش های " + Utility.CurrentMember.FullName.ToString() +
                            System.Environment.NewLine +
                            new FarsiLibrary.Utils.PersianDate(System.DateTime.Now).ToString(),
                    attachment: oMemoryStream,
                    attachmentName: "لیست تراکنش های " + Utility.CurrentMember.FullName.ToString()
                );
            });
        }
    }
}
