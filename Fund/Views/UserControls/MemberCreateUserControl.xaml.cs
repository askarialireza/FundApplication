using System.Linq;

namespace Fund
{
    public partial class MemberCreateUserControl : System.Windows.Controls.UserControl
    {
        public bool IsPictureSelected { get; set; }
        public MemberCreateUserControl()
        {
            InitializeComponent();
        }

        private System.Collections.Generic.List<ViewModels.GenderComboBoxItemViewModel> Genders
                    = new System.Collections.Generic.List<ViewModels.GenderComboBoxItemViewModel>();

        private void userControlLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            IsPictureSelected = false;

            LoadGenderComboBox();

        }

        private void AcceptButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            #region Error Handling Messages

            if (string.IsNullOrWhiteSpace(firstNameTextBox.Text) == true)
            {
                DevExpress.Xpf.Core.DXMessageBox.Show
                (
                    caption: "خطا",
                    messageBoxText: "تکمیل فیلد نام الزامی است.",
                    button: System.Windows.MessageBoxButton.OK,
                    icon: System.Windows.MessageBoxImage.Error,
                    defaultResult: System.Windows.MessageBoxResult.OK,
                    options: System.Windows.MessageBoxOptions.RightAlign | System.Windows.MessageBoxOptions.RtlReading

                );

                return;
            }

            if (string.IsNullOrWhiteSpace(lastNameTextBox.Text) == true)
            {
                DevExpress.Xpf.Core.DXMessageBox.Show
                (
                    caption: "خطا",
                    messageBoxText: "تکمیل فیلد نام خانوادگی الزامی است.",
                    button: System.Windows.MessageBoxButton.OK,
                    icon: System.Windows.MessageBoxImage.Error,
                    defaultResult: System.Windows.MessageBoxResult.OK,
                    options: System.Windows.MessageBoxOptions.RightAlign | System.Windows.MessageBoxOptions.RtlReading

                );

                return;
            }

            if (string.IsNullOrWhiteSpace(fatherNameTextBox.Text) == true)
            {
                DevExpress.Xpf.Core.DXMessageBox.Show
                (
                    caption: "خطا",
                    messageBoxText: "تکمیل فیلد نام پدر الزامی است.",
                    button: System.Windows.MessageBoxButton.OK,
                    icon: System.Windows.MessageBoxImage.Error,
                    defaultResult: System.Windows.MessageBoxResult.OK,
                    options: System.Windows.MessageBoxOptions.RightAlign | System.Windows.MessageBoxOptions.RtlReading

                );

                return;
            }

            if (string.IsNullOrWhiteSpace(nationalCodeTextBox.Text) == true)
            {
                DevExpress.Xpf.Core.DXMessageBox.Show
                (
                    caption: "خطا",
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
                    caption: "خطا",
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
                    caption: "خطا",
                    messageBoxText: "تکمیل فیلد شماره تلفن الزامی است.",
                    button: System.Windows.MessageBoxButton.OK,
                    icon: System.Windows.MessageBoxImage.Error,
                    defaultResult: System.Windows.MessageBoxResult.OK,
                    options: System.Windows.MessageBoxOptions.RightAlign | System.Windows.MessageBoxOptions.RtlReading

                );

                return;
            }

            #endregion

            #region Transcation

            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                Models.Member oMember = new Models.Member();

                oMember.FullName.FirstName = firstNameTextBox.Text.Trim();
                oMember.FullName.LastName = lastNameTextBox.Text.Trim();
                oMember.FatherName = fatherNameTextBox.Text.Trim();
                oMember.GenderType = (GendersCombobox.SelectedIndex == 0) ? Models.Gender.Male : Models.Gender.Female;
                oMember.GenderToString = (oMember.GenderType == Models.Gender.Male) ? "آقا" : "خانم";
                oMember.NationalCode = nationalCodeTextBox.Text.Trim();
                oMember.EmailAddress = emailAddressTextBox.Text.Trim();
                oMember.PhoneNumber = phoneNumberTextBox.Text.Trim();
                System.Windows.Media.Imaging.BmpBitmapEncoder oBmpBitmapEncoder = new System.Windows.Media.Imaging.BmpBitmapEncoder();
                oMember.Picture = (IsPictureSelected == false) ? null : Utility.ImageToBytes(encoder: oBmpBitmapEncoder, imageSource: MemberImage.Source);
                oMember.FundId = Utility.CurrentFund.Id;

                oUnitOfWork.MemberRepository.Insert(oMember);

                oUnitOfWork.Save();

                DevExpress.Xpf.Core.DXMessageBox.Show
                    (
                        caption: "پیغام",
                        messageBoxText: "عضو جدید با موفقیت در بانک اطلاعاتی ایجاد گردید",
                        button: System.Windows.MessageBoxButton.OK,
                        icon: System.Windows.MessageBoxImage.Information,
                        defaultResult: System.Windows.MessageBoxResult.OK,
                        options: System.Windows.MessageBoxOptions.RightAlign | System.Windows.MessageBoxOptions.RtlReading
                    );

                Utility.MainWindow.RefreshUserInterface();
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
            #endregion

        }

        private void CloseButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            (this.Parent as System.Windows.Controls.Panel).Children.Remove(this);
        }

        private void DeleteImageButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var uriSource = new System.Uri(@"/Fund;component/Resources/Images/MemberPicture.png", System.UriKind.Relative);
            MemberImage.Source = new System.Windows.Media.Imaging.BitmapImage(uriSource);
            IsPictureSelected = false;
        }

        private void PickImageButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog oOpenFileDialog = new Microsoft.Win32.OpenFileDialog();

            oOpenFileDialog.Filter = "Image Files(*.jpg; *.jpeg; *.png; *.gif; *.bmp)|*.jpg; *.jpeg; *.png; *.gif; *.bmp";
            oOpenFileDialog.InitialDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyPictures);

            if (oOpenFileDialog.ShowDialog() == true)
            {
                MemberImage.Source = new System.Windows.Media.Imaging.BitmapImage(new System.Uri(oOpenFileDialog.FileName));
                IsPictureSelected = true;
            }
        }

        private void LoadGenderComboBox()
        {
            ViewModels.GenderComboBoxItemViewModel oViewModel
                 = new ViewModels.GenderComboBoxItemViewModel();

            oViewModel.GenderString = "آقا";

            System.Uri oUri = new System.Uri(@"/Fund;component/Resources/Icons/male32.png" , System.UriKind.Relative);
            oViewModel.GenderLogo = new System.Windows.Media.Imaging.BitmapImage(oUri);

            Genders.Add(oViewModel);

            oViewModel = new ViewModels.GenderComboBoxItemViewModel();

            oViewModel.GenderString = "خانم";

            oUri = new System.Uri(@"/Fund;component/Resources/Icons/female32.png", System.UriKind.Relative);
            oViewModel.GenderLogo = new System.Windows.Media.Imaging.BitmapImage(oUri);

            Genders.Add(oViewModel);

            GendersCombobox.ItemsSource = Genders
                .OrderBy(current => current.GenderString);

        }
    }
}
