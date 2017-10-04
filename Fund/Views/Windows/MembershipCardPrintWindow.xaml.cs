using System.Linq;

namespace Fund
{
    public partial class MembershipCardPrintWindow : System.Windows.Window
    {
        private System.Guid CurrentId;

        public MembershipCardPrintWindow()
        {
            InitializeComponent();
        }

        private void DXWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                var varList = oUnitOfWork.MemberRepository
                    .Get()
                    .Where(current => current.FundId == Utility.CurrentFund.Id)
                    .OrderBy(current => current.FullName.LastName)
                    .ThenBy(current => current.FullName.FirstName)
                    .ToList();

                MembersListBox.ItemsSource = varList;
                MembersListBox.DisplayMemberPath = "FullName";
                MembersListBox.SelectedValuePath = "Id";

                ExportToPdfButton.IsEnabled = (varList.Count == 0) ? false : true;
                PrintButton.IsEnabled = (varList.Count == 0) ? false : true;

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

            MembersListBox.SelectedIndex = 0;
        }

        private void MembersListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Models.Member oMember = MembersListBox.SelectedItem as Models.Member;

            FullNameLabel.Content = oMember.FullName;
            FatherNameLabel.Content = string.Format("فرزند {0}", oMember.FatherName);
            NationalCodeLabel.Content = string.Format("کد ملی {0}", oMember.NationalCode);

            if (oMember.Picture != null)
            {
                MemberImage.Source = Utility.BytesToImage(oMember.Picture);
            }
            else
            {
                var uriSource = new System.Uri(@"/Fund;component/Resources/Images/MemberPicture.png", System.UriKind.Relative);
                MemberImage.Source = new System.Windows.Media.Imaging.BitmapImage(uriSource);
            }

            CurrentId = oMember.Id;
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

        private void closeButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Close();
        }

        private void ExportToImageButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ShowReport(Infrastructure.Report.ExportType.SaveAsImage);
        }

        private void ShowReport(Infrastructure.Report.ExportType reportType)
        {
            Stimulsoft.Report.StiReport oStiReport = new Stimulsoft.Report.StiReport();

            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                Models.Member oMember = oUnitOfWork.MemberRepository
                    .GetById(CurrentId);

                ViewModels.MembershipCardViewModel oViewModel =
                    new ViewModels.MembershipCardViewModel();

                oViewModel.FirstName = oMember.FullName.FirstName;
                oViewModel.LastName = oMember.FullName.LastName;
                oViewModel.FatherName = oMember.FatherName;
                oViewModel.Gender = oMember.Gender;
                oViewModel.NationalCode = oMember.NationalCode;
                oViewModel.EmailAddress = oMember.EmailAddress;
                oViewModel.FundName = oMember.Fund.Name;
                oViewModel.PhoneNumber = oMember.PhoneNumber;
                oViewModel.MembershipDate = oMember.MembershipDate;

                if (oMember.Picture != null)
                {
                    System.IO.MemoryStream oMemoryStream = new System.IO.MemoryStream(oMember.Picture);
                    System.Drawing.Image oImage = System.Drawing.Image.FromStream(oMemoryStream);
                    oViewModel.Picture = oImage;
                }
                else
                {
                    System.Drawing.Image oImage = Properties.Resources.ImageNull;
                    oViewModel.Picture = oImage;
                }

                var varData =
                    new
                    {
                        FirstName = oViewModel.FirstName,
                        LastName = oViewModel.LastName,
                        Picture = oViewModel.Picture,
                        FatherName = oViewModel.FatherName,
                        GenderDescription = oViewModel.GenderDescription,
                        FundName = oViewModel.FundName,
                        PhoneNumber = oViewModel.PhoneNumber,
                        PersianMembershipDate = oViewModel.PersianMembershipDate,
                        EmailAddress = oViewModel.EmailAddress,
                        NationalCode = oViewModel.NationalCode,
                    };

                oStiReport.Load(Properties.Resources.MembershipCardReport);
                oStiReport.RegBusinessObject("MembershipCard", varData);
                oStiReport.Compile();
                oStiReport.RenderWithWpf();

                oStiReport.DoAction(reportType, string.Format("{0} {1}", "کارت عضویت", oMember.FullName.FirstName + "" + oMember.FullName.LastName));

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
}
