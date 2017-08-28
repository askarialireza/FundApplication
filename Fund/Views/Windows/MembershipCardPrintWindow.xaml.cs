using System.Linq;

namespace Fund
{
    public partial class MembershipCardPrintWindow : DevExpress.Xpf.Core.DXWindow
    {
        private System.Guid _currentId;

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


                oUnitOfWork.Save();
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

            _currentId = oMember.Id;
        }

        private void PrintButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Stimulsoft.Report.StiReport oStiReport = new Stimulsoft.Report.StiReport();

            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                Models.Member oMember = oUnitOfWork.MemberRepository
                    .GetById(_currentId);

                ViewModels.MembershipCardViewModel oViewModel =
                    new ViewModels.MembershipCardViewModel();

                oViewModel.FirstName = oMember.FullName.FirstName;
                oViewModel.LastName = oMember.FullName.LastName;
                oViewModel.FatherName = oMember.FatherName;
                oViewModel.Gender = oMember.GenderToString;
                oViewModel.NationalCode = oMember.NationalCode;
                oViewModel.EmailAddress = oMember.EmailAddress;
                oViewModel.FundName = oMember.Fund.Name;
                oViewModel.PhoneNumber = oMember.PhoneNumber;

                if(oMember.Picture!=null)
                {
                    System.IO.MemoryStream oMemoryStream = new System.IO.MemoryStream(oMember.Picture);
                    System.Drawing.Image oImage = System.Drawing.Image.FromStream(oMemoryStream);
                    oViewModel.Picture = oImage;
                }
                else
                {
                    string imagePath = System.Environment.CurrentDirectory + @"\Files\Temp\ImageNull.png";
                    System.Drawing.Image oImage = System.Drawing.Image.FromFile(imagePath);
                    oViewModel.Picture = oImage;
                }


                oStiReport.Load(System.Environment.CurrentDirectory + "\\Files\\Reports\\MembershipCardReport.mrt");

                oStiReport.RegBusinessObject("MembershipCard", oViewModel);
                oStiReport.Compile();
                oStiReport.RenderWithWpf();
                oStiReport.ShowWithWpfRibbonGUI();

                oUnitOfWork.Save();
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

        private void ExportToPdfButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Stimulsoft.Report.StiReport oStiReport = new Stimulsoft.Report.StiReport();

            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                Models.Member oMember = oUnitOfWork.MemberRepository
                    .GetById(_currentId);

                ViewModels.MembershipCardViewModel oViewModel =
                    new ViewModels.MembershipCardViewModel();

                oViewModel.FirstName = oMember.FullName.FirstName;
                oViewModel.LastName = oMember.FullName.LastName;
                oViewModel.FatherName = oMember.FatherName;
                oViewModel.Gender = oMember.GenderToString;
                oViewModel.NationalCode = oMember.NationalCode;
                oViewModel.EmailAddress = oMember.EmailAddress;
                oViewModel.FundName = oMember.Fund.Name;
                oViewModel.PhoneNumber = oMember.PhoneNumber;

                if (oMember.Picture != null)
                {
                    System.IO.MemoryStream oMemoryStream = new System.IO.MemoryStream(oMember.Picture);
                    System.Drawing.Image oImage = System.Drawing.Image.FromStream(oMemoryStream);
                    oViewModel.Picture = oImage;
                }
                else
                {
                    string imagePath = System.Environment.CurrentDirectory + @"\Files\Temp\ImageNull.png";
                    System.Drawing.Image oImage = System.Drawing.Image.FromFile(imagePath);
                    oViewModel.Picture = oImage;
                }


                oStiReport.Load(System.Environment.CurrentDirectory + "\\Files\\Reports\\MembershipCardReport.mrt");

                oStiReport.RegBusinessObject("MembershipCard", oViewModel);
                oStiReport.Compile();
                oStiReport.RenderWithWpf();
                oStiReport.ExportToPdf(string.Format("{0} {1}", "کارت عضویت", oMember.FullName.FirstName + "" + oMember.FullName.LastName));

                oUnitOfWork.Save();
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

        private void closeButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Close();
        }

        private void ExportToImageButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Stimulsoft.Report.StiReport oStiReport = new Stimulsoft.Report.StiReport();

            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                Models.Member oMember = oUnitOfWork.MemberRepository
                    .GetById(_currentId);

                ViewModels.MembershipCardViewModel oViewModel =
                    new ViewModels.MembershipCardViewModel();

                oViewModel.FirstName = oMember.FullName.FirstName;
                oViewModel.LastName = oMember.FullName.LastName;
                oViewModel.FatherName = oMember.FatherName;
                oViewModel.Gender = oMember.GenderToString;
                oViewModel.NationalCode = oMember.NationalCode;
                oViewModel.EmailAddress = oMember.EmailAddress;
                oViewModel.FundName = oMember.Fund.Name;
                oViewModel.PhoneNumber = oMember.PhoneNumber;

                if (oMember.Picture != null)
                {
                    System.IO.MemoryStream oMemoryStream = new System.IO.MemoryStream(oMember.Picture);
                    System.Drawing.Image oImage = System.Drawing.Image.FromStream(oMemoryStream);
                    oViewModel.Picture = oImage;
                }
                else
                {
                    string imagePath = System.Environment.CurrentDirectory + @"\Files\Temp\ImageNull.png";
                    System.Drawing.Image oImage = System.Drawing.Image.FromFile(imagePath);
                    oViewModel.Picture = oImage;
                }


                oStiReport.Load(System.Environment.CurrentDirectory + "\\Files\\Reports\\MembershipCardReport.mrt");

                oStiReport.RegBusinessObject("MembershipCard", oViewModel);
                oStiReport.Compile();
                oStiReport.RenderWithWpf();
                oStiReport.ExportToImage(string.Format("{0} {1}", "کارت عضویت", oMember.FullName.FirstName + "" + oMember.FullName.LastName));

                oUnitOfWork.Save();
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
    }
}
