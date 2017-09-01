using System.Linq;
using Infrastructure;

namespace Fund
{

    public partial class MembersManagementUserControl : System.Windows.Controls.UserControl
    {
        private System.Guid CurrentId;

        private bool IsPictureDeleted;

        public MembersManagementUserControl()
        {
            InitializeComponent();

            GendersCombobox.ItemsSource = Infrastructure.Gender.GendersList;
        }

        private void UserControlLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            MainGroupBoxGrid.BlurApply(5);

            ButtonsGrid.BlurApply(5);

            EditItemsToggleSwitch.IsChecked = false;

            LoadGridControl();
        }

        private void ExportToPDFClick(object sender, System.Windows.RoutedEventArgs e)
        {

            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                var varList = oUnitOfWork.MemberRepository
                    .MembersToReport()
                    .ToList();

                Stimulsoft.Report.StiReport usersReport = new Stimulsoft.Report.StiReport();

                usersReport.Load(Properties.Resources.MembersViewReport);
                usersReport.Dictionary.Variables.Add("Today", System.DateTime.Now.ToPersianDate());
                usersReport.Dictionary.Variables.Add("FundName", Utility.CurrentFund.Name);
                usersReport.Dictionary.Variables.Add("FundManagerName", Utility.CurrentFund.ManagerName);
                usersReport.RegBusinessObject("Members", varList);
                usersReport.Compile();
                usersReport.RenderWithWpf();

                usersReport.ExportToPdf(string.Format("گزارش اعضا ({0}) ", Utility.CurrentFund.Name));

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

        private void PrintClick(object sender, System.Windows.RoutedEventArgs e)
        {
            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                var varList = oUnitOfWork.MemberRepository
                    .MembersToReport()
                    .ToList();

                Stimulsoft.Report.StiReport usersReport = new Stimulsoft.Report.StiReport();

                usersReport.Load(Properties.Resources.MembersViewReport);
                usersReport.Dictionary.Variables.Add("Today", System.DateTime.Now.ToPersianDate());
                usersReport.Dictionary.Variables.Add("FundName", Utility.CurrentFund.Name);
                usersReport.Dictionary.Variables.Add("FundManagerName", Utility.CurrentFund.ManagerName);
                usersReport.RegBusinessObject("Members", varList);
                usersReport.Compile();
                usersReport.RenderWithWpf();
                usersReport.Print();

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

        private void CloseClick(object sender, System.Windows.RoutedEventArgs e)
        {
            System.Windows.Controls.Panel oPanel = this.Parent as System.Windows.Controls.Panel;
            oPanel.Children.Remove(this);
        }

        private void GridControlItemChanged(object sender, DevExpress.Xpf.Grid.SelectedItemChangedEventArgs e)
        {
            ViewModels.MembersManagementViewModel oViewModel = MembersGridControl.SelectedItem as ViewModels.MembersManagementViewModel;

            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                Models.Member oMember = oUnitOfWork.MemberRepository
                    .GetById(oViewModel.Id);

                if (oMember != null)
                {
                    FirstNameTextBox.Text = oMember.FullName.FirstName;
                    LastNameTextBox.Text = oMember.FullName.LastName;
                    FatherNameTextBox.Text = oMember.FatherName;
                    NationalCodeTextBox.Text = oMember.NationalCode;
                    phoneNumberTextBox.Text = oMember.PhoneNumber;
                    emailAddressTextBox.Text = oMember.EmailAddress;
                    GendersCombobox.SelectedItem = (GendersCombobox.ItemsSource as System.Collections.Generic.List<ViewModels.GenderViewModel>)
                        .Where(current => current.Gender == oMember.GenderType)
                        .FirstOrDefault();

                    var uriSource = new System.Uri(@"/Fund;component/Resources/Images/MemberPicture.png", System.UriKind.Relative);
                    MemberImage.Source = (oMember.Picture == null) ? new System.Windows.Media.Imaging.BitmapImage(uriSource) : Utility.BytesToImage(oMember.Picture);
                    CurrentId = oMember.Id;
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

        private void EditItemsToggleSwitchChecked(object sender, System.Windows.RoutedEventArgs e)
        {
            AcceptButton.IsEnabled = true;
            CancelButton.IsEnabled = true;
            DeleteButton.IsEnabled = true;
            PickImageButton.IsEnabled = true;
            DeleteImageButton.IsEnabled = true;

            FirstNameLabel.IsEnabled = true;
            LastNameLabel.IsEnabled = true;
            FatherNameLabel.IsEnabled = true;
            NationalCodeLabel.IsEnabled = true;
            GenderTypeLabel.IsEnabled = true;
            PhoneNumberLabel.IsEnabled = true;
            EmailAddressLabel.IsEnabled = true;
            PictureLabel.IsEnabled = true;

            FirstNameTextBox.IsEnabled = true;
            LastNameTextBox.IsEnabled = true;
            FatherNameTextBox.IsEnabled = true;
            NationalCodeTextBox.IsEnabled = true;
            GendersCombobox.IsEnabled = true;
            phoneNumberTextBox.IsEnabled = true;
            emailAddressTextBox.IsEnabled = true;

            MemberImage.IsEnabled = true;

            MainGroupBoxGrid.BlurDisable();
            ButtonsGrid.BlurDisable();
        }

        private void EditItemsToggleSwitchUnchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            AcceptButton.IsEnabled = false;
            CancelButton.IsEnabled = false;
            DeleteButton.IsEnabled = false;
            PickImageButton.IsEnabled = false;
            DeleteImageButton.IsEnabled = false;

            FirstNameLabel.IsEnabled = false;
            LastNameLabel.IsEnabled = false;
            FatherNameLabel.IsEnabled = false;
            NationalCodeLabel.IsEnabled = false;
            GenderTypeLabel.IsEnabled = false;
            PhoneNumberLabel.IsEnabled = false;
            EmailAddressLabel.IsEnabled = false;
            PictureLabel.IsEnabled = false;

            FirstNameTextBox.IsEnabled = false;
            LastNameTextBox.IsEnabled = false;
            FatherNameTextBox.IsEnabled = false;
            NationalCodeTextBox.IsEnabled = false;
            GendersCombobox.IsEnabled = false;
            phoneNumberTextBox.IsEnabled = false;
            emailAddressTextBox.IsEnabled = false;

            MemberImage.IsEnabled = false;

            MainGroupBoxGrid.BlurApply(5);
            ButtonsGrid.BlurApply(5);

        }

        private void AcceptClick(object sender, System.Windows.RoutedEventArgs e)
        {
            #region Error Handling Messages

            if (string.IsNullOrWhiteSpace(FirstNameTextBox.Text) == true)
            {
                DevExpress.Xpf.Core.DXMessageBox.Show
                (
                    caption: Infrastructure.MessageBoxCaption.Error,
                    messageBoxText: "تکمیل فیلد نام الزامی است.",
                    button: System.Windows.MessageBoxButton.OK,
                    icon: System.Windows.MessageBoxImage.Error,
                    defaultResult: System.Windows.MessageBoxResult.OK,
                    options: System.Windows.MessageBoxOptions.RightAlign | System.Windows.MessageBoxOptions.RtlReading

                );

                return;
            }

            if (string.IsNullOrWhiteSpace(LastNameTextBox.Text) == true)
            {
                DevExpress.Xpf.Core.DXMessageBox.Show
                (
                    caption: Infrastructure.MessageBoxCaption.Error,
                    messageBoxText: "تکمیل فیلد نام خانوادگی الزامی است.",
                    button: System.Windows.MessageBoxButton.OK,
                    icon: System.Windows.MessageBoxImage.Error,
                    defaultResult: System.Windows.MessageBoxResult.OK,
                    options: System.Windows.MessageBoxOptions.RightAlign | System.Windows.MessageBoxOptions.RtlReading

                );

                return;
            }

            if (string.IsNullOrWhiteSpace(FatherNameTextBox.Text) == true)
            {
                DevExpress.Xpf.Core.DXMessageBox.Show
                (
                    caption: Infrastructure.MessageBoxCaption.Error,
                    messageBoxText: "تکمیل فیلد نام پدر الزامی است.",
                    button: System.Windows.MessageBoxButton.OK,
                    icon: System.Windows.MessageBoxImage.Error,
                    defaultResult: System.Windows.MessageBoxResult.OK,
                    options: System.Windows.MessageBoxOptions.RightAlign | System.Windows.MessageBoxOptions.RtlReading

                );

                return;
            }

            if (string.IsNullOrWhiteSpace(NationalCodeTextBox.Text) == true)
            {
                DevExpress.Xpf.Core.DXMessageBox.Show
                (
                    caption: Infrastructure.MessageBoxCaption.Error,
                    messageBoxText: "تکمیل فیلد کد ملی الزامی است.",
                    button: System.Windows.MessageBoxButton.OK,
                    icon: System.Windows.MessageBoxImage.Error,
                    defaultResult: System.Windows.MessageBoxResult.OK,
                    options: System.Windows.MessageBoxOptions.RightAlign | System.Windows.MessageBoxOptions.RtlReading

                );

                return;
            }

            if (string.IsNullOrWhiteSpace(emailAddressTextBox.Text) == true)
            {
                DevExpress.Xpf.Core.DXMessageBox.Show
                (
                    caption: Infrastructure.MessageBoxCaption.Error,
                    messageBoxText: "تکمیل فیلد پست الکترونیکی الزامی است.",
                    button: System.Windows.MessageBoxButton.OK,
                    icon: System.Windows.MessageBoxImage.Error,
                    defaultResult: System.Windows.MessageBoxResult.OK,
                    options: System.Windows.MessageBoxOptions.RightAlign | System.Windows.MessageBoxOptions.RtlReading

                );

                return;
            }

            if (string.IsNullOrWhiteSpace(phoneNumberTextBox.Text) == true)
            {
                DevExpress.Xpf.Core.DXMessageBox.Show
                (
                    caption: Infrastructure.MessageBoxCaption.Error,
                    messageBoxText: "تکمیل فیلد شماره تلفن الزامی است.",
                    button: System.Windows.MessageBoxButton.OK,
                    icon: System.Windows.MessageBoxImage.Error,
                    defaultResult: System.Windows.MessageBoxResult.OK,
                    options: System.Windows.MessageBoxOptions.RightAlign | System.Windows.MessageBoxOptions.RtlReading

                );

                return;
            }

            #endregion

            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                Models.Member oMember = oUnitOfWork.MemberRepository
                    .GetById(CurrentId);

                if (oMember != null)
                {

                    oMember.FullName.FirstName = FirstNameTextBox.Text.Trim();
                    oMember.FullName.LastName = LastNameTextBox.Text.Trim();
                    oMember.FatherName = FatherNameTextBox.Text.Trim();
                    oMember.GenderType = (GendersCombobox.SelectedItem as ViewModels.GenderViewModel).Gender;
                    oMember.GenderToString = (oMember.GenderType == Models.Gender.Male) ? "آقا" : "خانم";
                    oMember.NationalCode = NationalCodeTextBox.Text.Trim();
                    oMember.EmailAddress = emailAddressTextBox.Text.Trim();
                    oMember.PhoneNumber = phoneNumberTextBox.Text.Trim();
                    System.Windows.Media.Imaging.BmpBitmapEncoder oBmpBitmapEncoder = new System.Windows.Media.Imaging.BmpBitmapEncoder();
                    oMember.Picture = (IsPictureDeleted == true) ? null : Utility.ImageToBytes(encoder: oBmpBitmapEncoder, imageSource: MemberImage.Source);

                    oUnitOfWork.MemberRepository.Update(oMember);
                    oUnitOfWork.Save();

                    DevExpress.Xpf.Core.DXMessageBox.Show
                        (
                            caption: Infrastructure.MessageBoxCaption.Information,
                            messageBoxText: "مشخصات عضو صندوق با موفقیت ویرایش گردید.",
                            button: System.Windows.MessageBoxButton.OK,
                            icon: System.Windows.MessageBoxImage.Information,
                            defaultResult: System.Windows.MessageBoxResult.OK,
                            options: System.Windows.MessageBoxOptions.RightAlign | System.Windows.MessageBoxOptions.RtlReading
                        );
                }
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

            LoadGridControl();
        }

        private void CancelClick(object sender, System.Windows.RoutedEventArgs e)
        {
            EditItemsToggleSwitch.IsChecked = false;

            GridControlItemChanged(null, null);
        }

        private void DeleteUserClick(object sender, System.Windows.RoutedEventArgs e)
        {
            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                System.Windows.MessageBoxResult oResult =
                        DevExpress.Xpf.Core.DXMessageBox.Show
                        (
                            caption: Infrastructure.MessageBoxCaption.Question,
                            messageBoxText: "آیا مطمئن به حذف عضو صندوق از سیستم هستید ؟",
                            button: System.Windows.MessageBoxButton.YesNo,
                            icon: System.Windows.MessageBoxImage.Question,
                            defaultResult: System.Windows.MessageBoxResult.No,
                            options: System.Windows.MessageBoxOptions.RightAlign | System.Windows.MessageBoxOptions.RtlReading
                        );

                if (oResult == System.Windows.MessageBoxResult.Yes)
                {
                    oUnitOfWork.MemberRepository.DeleteById(CurrentId);

                    oUnitOfWork.Save();

                    DevExpress.Xpf.Core.DXMessageBox.Show
                    (
                        caption: Infrastructure.MessageBoxCaption.Information,
                        messageBoxText: "عضو صندوق با موفقیت از سیستم حذف گردید.",
                        button: System.Windows.MessageBoxButton.OK,
                        icon: System.Windows.MessageBoxImage.Information,
                        defaultResult: System.Windows.MessageBoxResult.OK,
                        options: System.Windows.MessageBoxOptions.RightAlign | System.Windows.MessageBoxOptions.RtlReading
                    );

                }

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

            LoadGridControl();
        }

        private void DeleteImageButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var uriSource = new System.Uri(@"/Fund;component/Resources/Images/MemberPicture.png", System.UriKind.Relative);
            MemberImage.Source = new System.Windows.Media.Imaging.BitmapImage(uriSource);
            IsPictureDeleted = true;
        }

        private void PickImageButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog oOpenFileDialog = new Microsoft.Win32.OpenFileDialog();

            oOpenFileDialog.Filter = "Image Files(*.jpg; *.jpeg; *.png; *.gif; *.bmp)|*.jpg; *.jpeg; *.png; *.gif; *.bmp";
            oOpenFileDialog.InitialDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyPictures);

            if (oOpenFileDialog.ShowDialog() == true)
            {
                MemberImage.Source = new System.Windows.Media.Imaging.BitmapImage(new System.Uri(oOpenFileDialog.FileName));

                IsPictureDeleted = false;
            }
        }

        private void LoadGridControl()
        {
            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                MembersGridControl.ItemsSource = oUnitOfWork.MemberRepository
                    .Get()
                    .Select(current => new ViewModels.MembersManagementViewModel()
                    {
                        Id = current.Id,
                        FullName = current.FullName,
                        EmailAddress = current.EmailAddress,
                        FatherName = current.FatherName,
                        GenderToString = current.GenderToString,
                        NationalCode = current.NationalCode,
                        PersianMembershipDateTime = current.PersianMembershipDateTime,
                        PhoneNumber = current.PhoneNumber,
                    })
                    .OrderBy(current => current.FullName.LastName)
                    .ThenBy(current => current.FullName.FirstName)
                    .ToList();

                oUnitOfWork.Save();
                oUnitOfWork.Dispose();
                oUnitOfWork = null;
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
