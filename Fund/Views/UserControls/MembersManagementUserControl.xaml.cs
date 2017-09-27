using System.Linq;
using Infrastructure;

namespace Fund
{

    public partial class MembersManagementUserControl : System.Windows.Controls.UserControl
    {
        private System.Guid CurrentId;

        private bool IsPictureDeleted;

        private bool IsPictureChanged;

        public MembersManagementUserControl()
        {
            InitializeComponent();

            GendersCombobox.ItemsSource = Infrastructure.Gender.GendersList;

            IsPictureChanged = false;
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
            ShowReport(ReportType.ExportToPDF);
        }

        private void PrintClick(object sender, System.Windows.RoutedEventArgs e)
        {
            ShowReport(reportType: ReportType.Print);
        }

        private void CloseClick(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Close();
        }

        private void GridControlItemChanged(object sender, DevExpress.Xpf.Grid.SelectedItemChangedEventArgs e)
        {
            if (MembersGridControl.ItemsSource == null)
            {
                return;
            }

            Models.Member oMember = MembersGridControl.SelectedItem as Models.Member;

            if (oMember != null)
            {
                FirstNameTextBox.Text = oMember.FullName.FirstName;
                LastNameTextBox.Text = oMember.FullName.LastName;
                FatherNameTextBox.Text = oMember.FatherName;
                NationalCodeTextBox.Text = oMember.NationalCode;
                phoneNumberTextBox.Text = oMember.PhoneNumber;
                emailAddressTextBox.Text = oMember.EmailAddress;
                GendersCombobox.SelectedItem = (GendersCombobox.ItemsSource as System.Collections.Generic.List<ViewModels.GenderViewModel>)
                    .Where(current => current.Gender == oMember.Gender)
                    .FirstOrDefault();

                var uriSource = new System.Uri(@"/Fund;component/Resources/Images/MemberPicture.png", System.UriKind.Relative);
                MemberImage.Source = (oMember.Picture == null) ? new System.Windows.Media.Imaging.BitmapImage(uriSource) : Utility.BytesToImage(oMember.Picture);
                CurrentId = oMember.Id;
            } 
        }

        private void EditItemsToggleSwitchChecked(object sender, System.Windows.RoutedEventArgs e)
        {
            var TextBoxes = MainGroupBoxGrid.Children
                .OfType<System.Windows.Controls.TextBox>()
                .AsEnumerable()
                ;

            var TextEdites = MainGroupBoxGrid.Children
                .OfType<DevExpress.Xpf.Editors.TextEdit>()
                .AsEnumerable()
                ;

            var Lables = MainGroupBoxGrid.Children
                .OfType<System.Windows.Controls.Label>()
                .AsEnumerable()
                ;

            var Buttons1 = ButtonsGrid.Children
                .OfType<DevExpress.Xpf.Core.SimpleButton>()
                .AsEnumerable()
                ;

            var Buttons2 = MemberButtonsGrid.Children
                .OfType<DevExpress.Xpf.Core.SimpleButton>()
                .AsEnumerable()
                ;

            foreach (System.Windows.Controls.TextBox textbox in TextBoxes)
            {
                textbox.IsEnabled = true;
            }

            foreach (DevExpress.Xpf.Editors.TextEdit textedit in TextEdites)
            {
                textedit.IsEnabled = true;
            }

            foreach (System.Windows.Controls.Label label in Lables)
            {
                label.IsEnabled = true;
            }

            foreach (DevExpress.Xpf.Core.SimpleButton button in Buttons1)
            {
                button.IsEnabled = true;
            }

            foreach (DevExpress.Xpf.Core.SimpleButton button in Buttons2)
            {
                button.IsEnabled = true;
            }

            GendersCombobox.IsEnabled = true;

            MemberImage.IsEnabled = true;

            if (MembersGridControl.ItemsSource != null)
            {
                MainGroupBoxGrid.BlurDisable();
                ButtonsGrid.BlurDisable();
            }

        }

        private void EditItemsToggleSwitchUnchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            var TextBoxes = MainGroupBoxGrid.Children
                .OfType<System.Windows.Controls.TextBox>()
                .AsEnumerable()
                ;

            var TextEdites = MainGroupBoxGrid.Children
                .OfType<DevExpress.Xpf.Editors.TextEdit>()
                .AsEnumerable()
                ;

            var Lables = MainGroupBoxGrid.Children
                .OfType<System.Windows.Controls.Label>()
                .AsEnumerable()
                ;

            var Buttons1 = ButtonsGrid.Children
                .OfType<DevExpress.Xpf.Core.SimpleButton>()
                .AsEnumerable()
                ;

            var Buttons2 = MemberButtonsGrid.Children
                .OfType<DevExpress.Xpf.Core.SimpleButton>()
                .AsEnumerable()
                ;

            foreach (System.Windows.Controls.TextBox textbox in TextBoxes)
            {
                textbox.IsEnabled = false;
            }

            foreach (DevExpress.Xpf.Editors.TextEdit textedit in TextEdites)
            {
                textedit.IsEnabled = false;
            }

            foreach (System.Windows.Controls.Label label in Lables)
            {
                label.IsEnabled = false;
            }

            foreach (DevExpress.Xpf.Core.SimpleButton button in Buttons1)
            {
                button.IsEnabled = true;
            }

            foreach (DevExpress.Xpf.Core.SimpleButton button in Buttons2)
            {
                button.IsEnabled = true;
            }

            GendersCombobox.IsEnabled = false;

            MemberImage.IsEnabled = false;

            MainGroupBoxGrid.BlurApply(5);

            ButtonsGrid.BlurApply(5);

        }

        private void AcceptClick(object sender, System.Windows.RoutedEventArgs e)
        {
            #region Error Handling Messages

            if (string.IsNullOrWhiteSpace(FirstNameTextBox.Text) == true)
            {
                Infrastructure.MessageBox.Show
                (
                    caption: Infrastructure.Caption.Error,
                    text: "تکمیل فیلد نام الزامی است."
                );

                return;
            }

            if (string.IsNullOrWhiteSpace(LastNameTextBox.Text) == true)
            {
                Infrastructure.MessageBox.Show
                (
                    caption: Infrastructure.Caption.Error,
                    text: "تکمیل فیلد نام خانوادگی الزامی است."
                );

                return;
            }

            if (string.IsNullOrWhiteSpace(FatherNameTextBox.Text) == true)
            {
                Infrastructure.MessageBox.Show
                (
                    caption: Infrastructure.Caption.Error,
                    text: "تکمیل فیلد نام پدر الزامی است."
                );

                return;
            }

            if (string.IsNullOrWhiteSpace(NationalCodeTextBox.Text) == true)
            {
                Infrastructure.MessageBox.Show
                (
                    caption: Infrastructure.Caption.Error,
                    text: "تکمیل فیلد کد ملی الزامی است."
                );

                return;
            }

            if (string.IsNullOrWhiteSpace(emailAddressTextBox.Text) == true)
            {
                Infrastructure.MessageBox.Show
                (
                    caption: Infrastructure.Caption.Error,
                    text: "تکمیل فیلد پست الکترونیکی الزامی است."
                );

                return;
            }

            if (string.IsNullOrWhiteSpace(phoneNumberTextBox.Text) == true)
            {
                Infrastructure.MessageBox.Show
                (
                    caption: Infrastructure.Caption.Error,
                    text: "تکمیل فیلد شماره تلفن الزامی است.");

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
                    oMember.Gender = (GendersCombobox.SelectedItem as ViewModels.GenderViewModel).Gender;
                    oMember.NationalCode = NationalCodeTextBox.Text.Trim();
                    oMember.EmailAddress = emailAddressTextBox.Text.Trim();
                    oMember.PhoneNumber = phoneNumberTextBox.Text.Trim();
                    System.Windows.Media.Imaging.BmpBitmapEncoder oBmpBitmapEncoder = new System.Windows.Media.Imaging.BmpBitmapEncoder();

                    if (IsPictureChanged == true)
                    {
                        oMember.Picture = (IsPictureDeleted == true) ? null : Utility.ImageToBytes(encoder: oBmpBitmapEncoder, imageSource: MemberImage.Source);
                    }

                    oUnitOfWork.MemberRepository.Update(oMember);
                    oUnitOfWork.Save();

                    Infrastructure.MessageBox.Show
                        (
                            caption: Infrastructure.Caption.Information,
                            text: "مشخصات عضو صندوق با موفقیت ویرایش گردید."
                        );
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
                        Infrastructure.MessageBox.Show
                        (
                            caption: Infrastructure.Caption.Question,
                            text: "آیا مطمئن به حذف عضو صندوق از سیستم هستید ؟"
                        );

                if (oResult == System.Windows.MessageBoxResult.Yes)
                {
                    Models.Member oMember = oUnitOfWork.MemberRepository
                        .GetById(CurrentId);

                    if (oMember != null)
                    {
                        oUnitOfWork.MemberRepository.Delete(oMember);
                    }

                    var varList = oUnitOfWork.TransactionRepository
                        .Get()
                        .Where(current => current.MemberId == oMember.Id)
                        .ToList();

                    foreach (Models.Transaction oTransaction in varList)
                    {
                        oUnitOfWork.TransactionRepository.Delete(oTransaction);
                    }

                    Infrastructure.MessageBox.Show
                    (
                        caption: Infrastructure.Caption.Information,
                        text: "عضو صندوق با موفقیت از سیستم حذف گردید."
                    );

                    oUnitOfWork.Save();

                    LoadGridControl();
                }

                if (oResult == System.Windows.MessageBoxResult.No)
                {
                    return;
                }

            }
            catch (System.Exception ex)
            {
                Infrastructure.MessageBox.Show(ex.Message); ;
            }
        }

        private void DeleteImageButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var uriSource = new System.Uri(@"/Fund;component/Resources/Images/MemberPicture.png", System.UriKind.Relative);

            MemberImage.Source = new System.Windows.Media.Imaging.BitmapImage(uriSource);

            IsPictureDeleted = true;

            IsPictureChanged = true;
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

                IsPictureChanged = true;
            }
        }

        private void LoadGridControl()
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

                MembersGridControl.ItemsSource = varList;

                oUnitOfWork.Save();
                oUnitOfWork.Dispose();
                oUnitOfWork = null;

                ExportToPdfButton.IsEnabled = (varList.Count == 0) ? false : true;
                PrintButton.IsEnabled = (varList.Count == 0) ? false : true;
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

        private void ShowReport(Infrastructure.ReportType reportType)
        {
            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                var varList = oUnitOfWork.MemberRepository
                    .MembersToReport()
                    .ToList();

                Stimulsoft.Report.StiReport oReport = new Stimulsoft.Report.StiReport();

                oReport.Load(Properties.Resources.MembersViewReport);
                oReport.Dictionary.Variables.Add("Today", System.DateTime.Now.ToPersianDate());
                oReport.Dictionary.Variables.Add("FundName", Utility.CurrentFund.Name);
                oReport.Dictionary.Variables.Add("FundManagerName", Utility.CurrentFund.ManagerName);
                oReport.RegBusinessObject("Members", varList);
                oReport.Compile();
                oReport.RenderWithWpf();
                oReport.DoAction(reportType, string.Format("گزارش اعضا ({0}) ", Utility.CurrentFund.Name));

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

        private void Clear()
        {
            var TextBoxes = MainGroupBoxGrid.Children
                .OfType<System.Windows.Controls.TextBox>()
                .AsEnumerable()
                ;

            var TextEdites = MainGroupBoxGrid.Children
                .OfType<DevExpress.Xpf.Editors.TextEdit>()
                .AsEnumerable()
                ;

            foreach (System.Windows.Controls.TextBox textbox in TextBoxes)
            {
                textbox.Clear();
            }

            foreach (DevExpress.Xpf.Editors.TextEdit textedit in TextEdites)
            {
                textedit.Clear();
            }

            var uriSource = new System.Uri(@"/Fund;component/Resources/Images/MemberPicture.png", System.UriKind.Relative);

            MemberImage.Source = new System.Windows.Media.Imaging.BitmapImage(uriSource);
        }
    }
}
