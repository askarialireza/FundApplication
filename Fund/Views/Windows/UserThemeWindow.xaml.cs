using System.Linq;

namespace Fund
{

    public partial class UserThemeWindow : DevExpress.Xpf.Core.DXWindow
    {

        private string ApplicationThemeName;

        private string FontFamilyName;

        private bool ignoreMessageBox = false;

        private bool IsThemeChanged = false;

        private System.Collections.Generic.List<ViewModels.ThemeViewModel> ThemesComboBoxList =
                 new System.Collections.Generic.List<ViewModels.ThemeViewModel>();

        private System.Collections.Generic.List<ViewModels.FontViewModel> FontsComboBoxList =
             new System.Collections.Generic.List<ViewModels.FontViewModel>();

        public UserThemeWindow()
        {
            InitializeComponent();
        }

        private void DXWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            FontsComboBox();

            ThemesComboBox();


            switch (Utility.CurrentUser.UserSetting.Theme.FontFamily)
            {
                case "B Yagut":
                    FontComboBox.SelectedIndex = 2;
                    break;

                case "B Traffic":
                    FontComboBox.SelectedIndex = 0;
                    break;

                case "B Nazanin":
                    FontComboBox.SelectedIndex = 1;
                    break;

                case "B Yekan":
                    FontComboBox.SelectedIndex = 3;
                    break;
                default:
                    break;
            }

            ApplicationThemeName = Utility.CurrentUser.UserSetting.Theme.ApplicationTheme;
            FontFamilyName = Utility.CurrentUser.UserSetting.Theme.FontFamily;

            ThemeComboBox.SelectedItem = ThemesComboBoxList
                .OrderBy(current => current.ApplicationThemeName)
                .Where(current => current.ApplicationThemeName == Utility.CurrentUser.UserSetting.Theme.ApplicationTheme)
                .FirstOrDefault();

            IsThemeChanged = false;

        }

        private void FontsComboBox()
        {

            ViewModels.FontViewModel oViewModel = new ViewModels.FontViewModel();

            oViewModel.FontName = "یاقوت";
            oViewModel.FontFamily = new System.Windows.Media.FontFamily("B Yagut");

            FontsComboBoxList.Add(oViewModel);

            oViewModel = new ViewModels.FontViewModel();

            oViewModel.FontName = "ترافیک";
            oViewModel.FontFamily = new System.Windows.Media.FontFamily("B Traffic");

            FontsComboBoxList.Add(oViewModel);

            oViewModel = new ViewModels.FontViewModel();

            oViewModel.FontName = "نازنین";
            oViewModel.FontFamily = new System.Windows.Media.FontFamily("B Nazanin");

            FontsComboBoxList.Add(oViewModel);

            oViewModel = new ViewModels.FontViewModel();

            oViewModel.FontName = "یکان";
            oViewModel.FontFamily = new System.Windows.Media.FontFamily("B Yekan");

            FontsComboBoxList.Add(oViewModel);

            FontComboBox.ItemsSource = FontsComboBoxList
                .OrderBy(current => current.FontName);
        }

        private void ThemesComboBox()
        {

            ViewModels.ThemeViewModel oViewModel = new ViewModels.ThemeViewModel();

            oViewModel.ApplicationThemeName = DevExpress.Xpf.Core.Theme.SevenName;
            oViewModel.DisplayName = DevExpress.Xpf.Core.Theme.SevenFullName;

            ThemesComboBoxList.Add(oViewModel);

            oViewModel = new ViewModels.ThemeViewModel();

            oViewModel.ApplicationThemeName = DevExpress.Xpf.Core.Theme.VS2010Name;
            oViewModel.DisplayName = DevExpress.Xpf.Core.Theme.VS2010FullName;

            ThemesComboBoxList.Add(oViewModel);

            oViewModel = new ViewModels.ThemeViewModel();

            oViewModel.ApplicationThemeName = DevExpress.Xpf.Core.Theme.Office2016ColorfulName;
            oViewModel.DisplayName = DevExpress.Xpf.Core.Theme.Office2016ColorfulFullName;

            ThemesComboBoxList.Add(oViewModel);

            oViewModel = new ViewModels.ThemeViewModel();

            oViewModel.ApplicationThemeName = DevExpress.Xpf.Core.Theme.Office2016WhiteName;
            oViewModel.DisplayName = DevExpress.Xpf.Core.Theme.Office2016WhiteFullName;

            ThemesComboBoxList.Add(oViewModel);

            oViewModel = new ViewModels.ThemeViewModel();

            oViewModel.ApplicationThemeName = DevExpress.Xpf.Core.Theme.Office2013LightGrayName;
            oViewModel.DisplayName = DevExpress.Xpf.Core.Theme.Office2013LightGrayFullName;

            ThemesComboBoxList.Add(oViewModel);

            oViewModel = new ViewModels.ThemeViewModel();

            oViewModel.ApplicationThemeName = DevExpress.Xpf.Core.Theme.Office2010BlueName;
            oViewModel.DisplayName = DevExpress.Xpf.Core.Theme.Office2010BlueFullName;

            ThemesComboBoxList.Add(oViewModel);

            oViewModel = new ViewModels.ThemeViewModel();

            oViewModel.ApplicationThemeName = DevExpress.Xpf.Core.Theme.Office2010SilverName;
            oViewModel.DisplayName = DevExpress.Xpf.Core.Theme.Office2010SilverFullName;

            ThemesComboBoxList.Add(oViewModel);

            oViewModel = new ViewModels.ThemeViewModel();

            oViewModel.ApplicationThemeName = DevExpress.Xpf.Core.Theme.Office2007BlueName;
            oViewModel.DisplayName = DevExpress.Xpf.Core.Theme.Office2007BlueFullName;

            ThemesComboBoxList.Add(oViewModel);

            oViewModel = new ViewModels.ThemeViewModel();

            oViewModel.ApplicationThemeName = DevExpress.Xpf.Core.Theme.Office2007SilverName;
            oViewModel.DisplayName = DevExpress.Xpf.Core.Theme.Office2007SilverFullName;

            ThemesComboBoxList.Add(oViewModel);

            oViewModel = new ViewModels.ThemeViewModel();

            oViewModel.ApplicationThemeName = DevExpress.Xpf.Core.Theme.DXStyleName;
            oViewModel.DisplayName = DevExpress.Xpf.Core.Theme.DXStyleFullName;

            ThemesComboBoxList.Add(oViewModel);

            ThemeComboBox.ItemsSource = ThemesComboBoxList
                .OrderBy(current => current.DisplayName);

            ThemeComboBox.DisplayMember = "DisplayName";
        }

        private void AcceptButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SaveTheme(doByMessageBox: false);
        }

        private void CancelButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            UndoTheme(doByMessageBox: false);
        }

        private void ResetToDefaultButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ResetThemeToDefault(doByMessageBox: false);
        }

        private void FontComboBox_SelectedIndexChanged(object sender, System.Windows.RoutedEventArgs e)
        {

            ViewModels.FontViewModel oViewModel = FontComboBox.SelectedItem as ViewModels.FontViewModel;

            App.Current.Resources[Infrastructure.Text.PersianFontResources] = oViewModel.FontFamily;

            IsThemeChanged = true;

        }

        private void ThemeComboBox_SelectedIndexChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            ViewModels.ThemeViewModel oViewModel = ThemeComboBox.SelectedItem as ViewModels.ThemeViewModel;

            DevExpress.Xpf.Core.ApplicationThemeHelper.ApplicationThemeName = oViewModel.ApplicationThemeName;

            Utility.SetThemeBackground(DevExpress.Xpf.Core.ApplicationThemeHelper.ApplicationThemeName);

            IsThemeChanged = true;
        }

        private void SaveTheme(bool doByMessageBox)
        {
            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                Models.User oUser = oUnitOfWork.UserRepository
                    .GetById(Utility.CurrentUser.Id);

                ViewModels.FontViewModel oFViewModel = FontComboBox.SelectedItem as ViewModels.FontViewModel;

                ViewModels.ThemeViewModel oTViewModel = ThemeComboBox.SelectedItem as ViewModels.ThemeViewModel;

                oUser.UserSetting.Theme.ApplicationTheme = oTViewModel.ApplicationThemeName;

                DevExpress.Xpf.Core.ApplicationThemeHelper.ApplicationThemeName = oUser.UserSetting.Theme.ApplicationTheme;

                Utility.SetThemeBackground(DevExpress.Xpf.Core.ApplicationThemeHelper.ApplicationThemeName);

                oUser.UserSetting.Theme.FontFamily = oFViewModel.FontFamily.Source;

                App.Current.Resources[Infrastructure.Text.PersianFontResources] = oFViewModel.FontFamily;

                oUnitOfWork.UserRepository.Update(oUser);

                Utility.CurrentUser = oUser;

                oUnitOfWork.Save();

                DevExpress.Xpf.Core.DXMessageBox.Show
                    (
                        caption: "پیغام",
                        messageBoxText: "تنظیمات شخصی سازی با موفقیت ذخیره گردید.",
                        button: System.Windows.MessageBoxButton.OK,
                        icon: System.Windows.MessageBoxImage.Information,
                        defaultResult: System.Windows.MessageBoxResult.OK,
                        options: System.Windows.MessageBoxOptions.RtlReading | System.Windows.MessageBoxOptions.RightAlign
                    );

                ignoreMessageBox = true;


                if (doByMessageBox == false)
                {
                    this.Close();
                }
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

        private void UndoTheme(bool doByMessageBox)
        {
            System.Windows.Media.FontFamily font =
                new System.Windows.Media.FontFamily(FontFamilyName);

            App.Current.Resources[Infrastructure.Text.PersianFontResources] = font;

            DevExpress.Xpf.Core.ApplicationThemeHelper.ApplicationThemeName = ApplicationThemeName;

            Utility.SetThemeBackground(DevExpress.Xpf.Core.ApplicationThemeHelper.ApplicationThemeName);

            ignoreMessageBox = true;

            if (doByMessageBox == false)
            {
                this.Close();
            }

        }

        private void ResetThemeToDefault(bool doByMessageBox)
        {
            System.Windows.Media.FontFamily font = new System.Windows.Media.FontFamily("B Yagut");

            App.Current.Resources[Infrastructure.Text.PersianFontResources] = font;

            DevExpress.Xpf.Core.ApplicationThemeHelper.ApplicationThemeName =
                DevExpress.Xpf.Core.Theme.Office2010BlueName;

            System.Windows.Media.ImageBrush oImageBrush = App.Current.Resources["Office2010BlueBackground"] as System.Windows.Media.ImageBrush;

            App.Current.Resources[Infrastructure.Text.BackgroundResources] = oImageBrush;

            ignoreMessageBox = true;

            if (doByMessageBox == false)
            {
                this.Close();
            }
        }

        private void DXWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            if (ignoreMessageBox == false)
            {
                if (IsThemeChanged == true)
                {
                    System.Windows.MessageBoxResult oResult =
                        DevExpress.Xpf.Core.DXMessageBox.Show
                        (
                            messageBoxText: "تنظیمات اعمال شده ذخیره گردند؟",
                            caption: "سوال",
                            button: System.Windows.MessageBoxButton.YesNoCancel,
                            icon: System.Windows.MessageBoxImage.Warning,
                            defaultResult: System.Windows.MessageBoxResult.Cancel,
                            options: System.Windows.MessageBoxOptions.RightAlign | System.Windows.MessageBoxOptions.RtlReading
                        );

                    if (oResult == System.Windows.MessageBoxResult.Yes)
                    {
                        SaveTheme(doByMessageBox: true);
                    }

                    if (oResult == System.Windows.MessageBoxResult.No)
                    {
                        UndoTheme(doByMessageBox: true);
                    }

                    if (oResult == System.Windows.MessageBoxResult.Cancel)
                    {
                        e.Cancel = true;
                    }
                }
            }
        }
    }
}
