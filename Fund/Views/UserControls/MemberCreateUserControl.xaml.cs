using System.Linq;

namespace Fund
{
    public partial class MemberCreateUserControl : System.Windows.Controls.UserControl
    {
        private bool IsPictureSelected;

        public MemberCreateUserControl()
        {
            InitializeComponent();

            IsPictureSelected = false;

            LoadGenderComboBox();
        }

        private void userControlLoaded(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void AcceptButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            #region Error Handling Messages

            if (string.IsNullOrWhiteSpace(firstNameTextBox.Text) == true)
            {
                Infrastructure.MessageBox.Show
                (
                    caption: Infrastructure.MessageBox.Caption.Error,
                    text: "تکمیل فیلد نام الزامی است."

                );

                return;
            }

            if (string.IsNullOrWhiteSpace(lastNameTextBox.Text) == true)
            {
                Infrastructure.MessageBox.Show
                (
                    caption: Infrastructure.MessageBox.Caption.Error,
                    text: "تکمیل فیلد نام خانوادگی الزامی است."
                );

                return;
            }

            if (string.IsNullOrWhiteSpace(fatherNameTextBox.Text) == true)
            {
                Infrastructure.MessageBox.Show
                (
                    caption: Infrastructure.MessageBox.Caption.Error,
                    text: "تکمیل فیلد نام پدر الزامی است."
                );

                return;
            }

            if (string.IsNullOrWhiteSpace(nationalCodeTextBox.Text) == true)
            {
                Infrastructure.MessageBox.Show
                (
                    caption: Infrastructure.MessageBox.Caption.Error,
                    text: "تکمیل فیلد کد ملی الزامی است."
                );

                return;
            }

            if (string.IsNullOrWhiteSpace(emailAddressTextBox.Text) == true)
            {
                Infrastructure.MessageBox.Show
                (
                    caption: Infrastructure.MessageBox.Caption.Error,
                    text: "تکمیل فیلد پست الکترونیکی الزامی است."
                );

                return;
            }

            if (string.IsNullOrWhiteSpace(phoneNumberTextBox.Text) == true)
            {
                Infrastructure.MessageBox.Show
                (
                    caption: Infrastructure.MessageBox.Caption.Error,
                    text: "تکمیل فیلد شماره تلفن الزامی است."
                );

                return;
            }

            #endregion

            #region Transcation

            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                string nationalCode = nationalCodeTextBox.Text.Trim();

                Models.Member varMember = oUnitOfWork.MemberRepository
                    .Get()
                    .Where(current => current.NationalCode == nationalCode)
                    .FirstOrDefault();

                if(varMember !=null)
                {
                    Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: "عضوی با شماره ملی درج شده در صندوق موجود می‌باشد.");

                    return;
                }
                else
                {
                    Models.Member oMember = new Models.Member();

                    oMember.FullName.FirstName = firstNameTextBox.Text.Trim();
                    oMember.FullName.LastName = lastNameTextBox.Text.Trim();
                    oMember.FatherName = fatherNameTextBox.Text.Trim();
                    oMember.Gender = ((GendersCombobox.SelectedItem as ViewModels.GenderViewModel).Gender);
                    oMember.NationalCode = nationalCodeTextBox.Text.Trim();
                    oMember.EmailAddress = emailAddressTextBox.Text.Trim();
                    oMember.PhoneNumber = phoneNumberTextBox.Text.Trim();
                    System.Windows.Media.Imaging.BmpBitmapEncoder oBmpBitmapEncoder = new System.Windows.Media.Imaging.BmpBitmapEncoder();
                    oMember.Picture = (IsPictureSelected == false) ? null : Utility.ImageToBytes(encoder: oBmpBitmapEncoder, imageSource: MemberImage.Source);
                    oMember.MembershipDate = DatePicker.SelectedDateTime;
                    oMember.FundId = Utility.CurrentFund.Id;

                    oUnitOfWork.MemberRepository.Insert(oMember);

                    oUnitOfWork.Save();

                    Infrastructure.MessageBox.Show
                        (
                            caption: Infrastructure.MessageBox.Caption.Information,
                            text: "عضو جدید با موفقیت در بانک اطلاعاتی ایجاد گردید"
                        );

                    Utility.MainWindow.RefreshUserInterface();
                }
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
            #endregion

        }

        private void CloseButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Close();
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
                System.Windows.Media.Imaging.BitmapImage oBitmapImage = new System.Windows.Media.Imaging.BitmapImage(new System.Uri(oOpenFileDialog.FileName));

                if (oBitmapImage.PixelWidth >= 1000 && oBitmapImage.PixelHeight >= 1000)
                {
                    Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: "حداکثر اندازه عکس انتخاب شده بایست 1000*1000 پیکسل باشد.");

                    return;
                }
                else
                {
                    MemberImage.Source = oBitmapImage;
                    IsPictureSelected = true;
                }
            }
        }

        private void LoadGenderComboBox()
        {
            GendersCombobox.ItemsSource = Infrastructure.Gender.GendersList;
        }
    }
}
