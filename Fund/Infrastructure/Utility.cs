
namespace Fund
{
    public static class Utility
    {

        #region Constructor

        static Utility()
        {

        }

        #endregion /Constructor

        #region Fields

        private static Microsoft.Win32.SaveFileDialog _saveFileDialog;

        #endregion /Fields

        #region Properties

        public static Models.User CurrentUser { get; set; }

        public static Models.Fund CurrentFund { get; set; }

        public static Models.Member CurrentMember { get; set; }

        public static Models.Loan CurrentLoan { get; set; }

        public static System.Guid AdminUserId { get; set; }

        public static MainRibbonWindow MainWindow { get; set; }

        public static string DatabaseBackupPath { get; set; }


        #endregion /Properties

        #region Methods

        public static void FarsiLocalization()
        {
            System.Globalization.CultureInfo oCultureInfo =
                new System.Globalization.CultureInfo("fa-IR");

            System.Threading.Thread.CurrentThread.CurrentUICulture = oCultureInfo;
            System.Threading.Thread.CurrentThread.CurrentCulture = oCultureInfo;

            System.Globalization.CultureInfo.DefaultThreadCurrentCulture = oCultureInfo;
            System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = oCultureInfo;
        }

        public static void SetStimulsoftLicense()
        {
            Stimulsoft.LicenseHelper.StimulsoftLicenseHelper.Activate();

            var lic = "6vJhGtLLLz2GNviWmUTrhSqnOItdDwjBylQzQcAOiHlEk/yd1EGdkgZdNFakiWoaW9Ykbfr0OFjvmv0s4pLeUhF35oAHI0VA04sU" +
                    "o2zZ9eZu7w8j9fHGqMKoff+Lk13joMi+6NWhFMbUVxUbJsUi96WtyXh1xyQU6rP6jnDsbGo3PPqG9y/5112tkW3/bb85NdKw67WT" +
                    "kep1STyVJ18zzpcTMift9iCjoGfp+0+ZwdiKTIfvdVUsxAGtvhWFFfoUi8bnipWNtgYB7YDCMnZFYfIdPFOWaNvUVB7hEWQhlW+a" +
                    "tzQPHq3yY9K9OnYxZjmdINWDELCzXh8wpO2cXVYedNXQZxf7XO3Ksp/hySqEv15FaJWQBeddVTjN7819SiVVQWMdEp2AmkF3AOYZ" +
                    "d+wQkEaBsVgZLz+miv3i22Z/5z7nbr/cJuIMHJKr0sDisgdajN7CRfxU1ucEfQzHfxEldPyXz3BHxuiw62wiWL8fuPV1fwWug85p" +
                    "rmWM5HShczq+3N5LgcTGD8/7bRvPDTKbaIl5WQt5mQkVelS72G0i1y4F9o21pN5u4SHzZTK2AL2/24FcBfgG3YhgQK6zf4u4Egdq" +
                    "qOz27TX6WsS1Th3HgDDZR8aYoH0LDx3U7IXcQUVMrKBIXxT5QQr6WIjrybANJDFuOndCY2eNweq5jtKK/Drej2E3JJIAQSwWLMqp" +
                    "Fa6AH3roCE3L40qOP0y9yva11Ae8puh2/pIw2tABXUQQfXvwdH62xmdqotm1T0fy6EeMLv+fM6MetpJ5vYWKUfwHMDi/YssgZIRv" +
                    "bHE8TTv7mV6d66hLySoigWBxO3TaKQEzs8D3BeKzk5pCLsYfh7PztReN/9n3zm9rSLpRzVYtqrC668qO02/Y6fHBWm4oEMqASPzw" +
                    "35gA4HuuBTEgYai8F2RDRwN1eLlPiZXjDVHwsQ7zuDsH/tFIEXIR2g8jlVSKrgoHWsUOnDP41crZHg3vL4hcaVZ7iEnNSj/wfIEt" +
                    "DA0lge2K+7a/yzZ8oEYtoVK0/N+8NbcNa0rWq2ESuxZR9MjEGbaQRCx7zAbBNhGVGrx+y8gFtUIcLEORRPCP5A66AWjdrowR+KHC" +
                    "+leCSTE65FuXyRhyv4R3gJZhhYY6vSE7L1IRbkmCIDY/k4abyJBXeXIau1LGEfX3PRitQ/bvjFM6c0f80wrRoucWQyN5nFGW2WlS" +
                    "ht97pjQmz/tLQ6lm1baRiwfMlDyc7/2hp1B6eYSiO7HF8GTGWcPEBoav4ysZbSaYkIA89Klbeg/PuCetW3it7vsLG999LT0c016M" +
                    "wl5deeYPJpSyT3IRY10URaQBJPZT8sgsLcaW/WS9jOa79K5veMTF5RbbqMEa5HNoozIhrEtNZFfLDCEZccuctPEkzJUAIqVgLI9K" +
                    "A8EpkeuK4cQJ/nLh+FmV72N7O+jMin9GwELKuuUEy5/n3pKK3KR2PYTF4TTywrnuwOeCCpTZgBk5lo9x30mrx5z57UsBBqiN4s9M" +
                    "b57vvtiAIIJHwBudimQM6IGC/CEZgTKHnXJ+6apNHoRf+F4fIU+ix0b9lamUD+GayQfiO7bjYOP7hrlvhMghu+tm3xcGsM3Kl9Xo" +
                    "4DEUmEDVPn0+c1JJ0Ucvj5UMCSEFUnvF/+zl/3rkcW8aweS0MROJnT9n/1S38vYGrQVzd74nTQ9aaotBdWPUPA==";

            Stimulsoft.Base.StiLicense.Key = lic;
        }

        public static void ExportToPdf(this Stimulsoft.Report.StiReport report, string reportTitle)
        {
            _saveFileDialog = new Microsoft.Win32.SaveFileDialog();

            _saveFileDialog.Filter = "PDF Files | *.pdf";
            _saveFileDialog.Title = "Save as Pdf";
            _saveFileDialog.OverwritePrompt = true;

            FarsiLibrary.Utils.PersianDate oPersianDate =
                    FarsiLibrary.Utils.PersianDateConverter.ToPersianDate(System.DateTime.Now);

            _saveFileDialog.FileName =
                string.Format("{6} {5}-{4}-{3}-{2}-{1}-{0}",
                oPersianDate.Year,
                oPersianDate.Month.ToString("00"),
                oPersianDate.Day.ToString("00"),
                oPersianDate.Hour.ToString("00"),
                oPersianDate.Minute.ToString("00"),
                oPersianDate.Second.ToString("00"),
                reportTitle
                );

            if (_saveFileDialog.ShowDialog() == true)
            {
                string path = System.IO.Path.GetFullPath(_saveFileDialog.FileName);
                report.ExportDocument(Stimulsoft.Report.StiExportFormat.Pdf, path);

                System.Windows.MessageBoxResult oDialogResult =
                     Infrastructure.MessageBox.Show
                     (
                         caption: Infrastructure.MessageBoxCaption.Question,
                         text: "خروجی گزارش با موفقیت ذخیره گردید." + System.Environment.NewLine + "آیا مایل به مشاهده خروجی می‌باشید؟"
                     );

                if (oDialogResult == System.Windows.MessageBoxResult.Yes)
                {
                    System.Diagnostics.Process.Start(path);
                }
            }
        }

        public static void ExportToImage(this Stimulsoft.Report.StiReport report, string reportTitle)
        {
            _saveFileDialog = new Microsoft.Win32.SaveFileDialog();

            _saveFileDialog.Filter = "Image Files(*.jpg; *.jpeg; *.png; *.gif; *.bmp)|*.jpg; *.jpeg; *.png; *.gif; *.bmp";
            _saveFileDialog.Title = "Save as Image";
            _saveFileDialog.OverwritePrompt = true;

            FarsiLibrary.Utils.PersianDate oPersianDate =
                    FarsiLibrary.Utils.PersianDateConverter.ToPersianDate(System.DateTime.Now);

            _saveFileDialog.FileName =
                string.Format("{6} {5}-{4}-{3}-{2}-{1}-{0}",
                oPersianDate.Year,
                oPersianDate.Month.ToString("00"),
                oPersianDate.Day.ToString("00"),
                oPersianDate.Hour.ToString("00"),
                oPersianDate.Minute.ToString("00"),
                oPersianDate.Second.ToString("00"),
                reportTitle
                );

            if (_saveFileDialog.ShowDialog() == true)
            {
                string path = System.IO.Path.GetFullPath(_saveFileDialog.FileName);
                report.ExportDocument(Stimulsoft.Report.StiExportFormat.Image, path);

                System.Windows.MessageBoxResult oDialogResult =
                     Infrastructure.MessageBox.Show
                     (
                         caption: Infrastructure.MessageBoxCaption.Question,
                         text: "خروجی گزارش با موفقیت ذخیره گردید." + System.Environment.NewLine + "آیا مایل به مشاهده خروجی می‌باشید؟"
                     );

                if (oDialogResult == System.Windows.MessageBoxResult.Yes)
                {
                    System.Diagnostics.Process.Start(path);
                }
            }
        }

        public static string ToPersianDate(this System.DateTime dateTime)
        {
            string result = string.Empty;

            result = new FarsiLibrary.Utils.PersianDate(dateTime).ToString("d");

            return result;
        }

        public static string ToPersianDateTime(this System.DateTime dateTime)
        {
            string result = string.Empty;

            result = new FarsiLibrary.Utils.PersianDate(dateTime).ToString("g");

            return result;
        }

        public static string ToRialStringFormat(this long value)
        {
            return value.ToString("#,##0 ریال");
        }

        public static string ToRial(this long value)
        {
            if (value == 0)
            {
                return ("0 ریال");
            }

            string strValue = System.Convert.ToString(value);

            int commaCount = strValue.Length / 3;

            string result = string.Empty;

            if (strValue.Length % 3 == 0)
            {
                while (strValue.Length != 0)
                {
                    result = result + strValue.Substring(0, 3);

                    if (commaCount != 1)
                    {
                        result = result + ",";

                        commaCount--;
                    }

                    strValue = strValue.Substring(3);
                }
            }
            else
            {
                result = strValue.Substring(0, (strValue.Length % 3));

                strValue = strValue.Substring((strValue.Length % 3));

                result = result + ",";

                commaCount--;

                while (strValue.Length != 0)
                {
                    result = result + strValue.Substring(0, 3);

                    if (commaCount != 0)
                    {
                        result = result + ",";

                        commaCount--;
                    }

                    strValue = strValue.Substring(3);
                }
            }

            result = result + " ریال";

            return result;
        }

        public static long StringToMoney(this string text)
        {
            if (string.IsNullOrWhiteSpace(text) == true)
            {
                return 0;
            }
            else
            {
                return (System.Convert.ToInt64(text.Replace(" ریال", string.Empty).Replace(",", string.Empty)));
            }
        }

        public static void MoveBackupFiles(string sourcePath, string destinationPath)
        {
            System.IO.DirectoryInfo oDirectoryInfo = new System.IO.DirectoryInfo(sourcePath);

            foreach (System.IO.FileInfo oFileInfo in oDirectoryInfo.GetFiles())
            {
                if ((string.Compare(oFileInfo.Extension, ".bkdb", true) == 0) == true)
                {
                    oFileInfo.MoveTo(destinationPath + "\\" + oFileInfo.Name);
                }
            }
        }

        public static byte[] ImageToBytes(System.Windows.Media.Imaging.BitmapEncoder encoder, System.Windows.Media.ImageSource imageSource)
        {
            byte[] bytes = null;
            var bitmapSource = imageSource as System.Windows.Media.Imaging.BitmapSource;

            if (bitmapSource != null)
            {
                encoder.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(bitmapSource));

                using (var stream = new System.IO.MemoryStream())
                {
                    encoder.Save(stream);
                    bytes = stream.ToArray();
                }
            }

            return bytes;
        }

        public static System.Windows.Media.ImageSource BytesToImage(byte[] bytes)
        {
            System.Windows.Media.Imaging.BitmapImage biImg = new System.Windows.Media.Imaging.BitmapImage();
            System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
            biImg.BeginInit();
            biImg.StreamSource = ms;
            biImg.EndInit();

            System.Windows.Media.ImageSource imgSrc = biImg as System.Windows.Media.ImageSource;

            return imgSrc;
        }

        public static void SetUserTheme()
        {
            System.Windows.Media.FontFamily font = new System.Windows.Media.FontFamily(CurrentUser.UserSetting.Theme.FontFamily);

            App.Current.Resources[Infrastructure.Text.PersianFontResources] = font;

            DevExpress.Xpf.Core.ApplicationThemeHelper.ApplicationThemeName = CurrentUser.UserSetting.Theme.ApplicationTheme;

            System.Windows.Media.ImageBrush oImageBrush = App.Current.Resources[Infrastructure.Text.BackgroundResources] as System.Windows.Media.ImageBrush;

            SetThemeBackground(DevExpress.Xpf.Core.ApplicationThemeHelper.ApplicationThemeName);

        }

        public static void SetThemeBackground(string applicationThemeName)
        {
            System.Windows.Media.ImageBrush oImageBrush =
                App.Current.Resources[applicationThemeName + "Background"] as System.Windows.Media.ImageBrush;

            App.Current.Resources[Infrastructure.Text.BackgroundResources] = oImageBrush;

            System.Windows.Media.LinearGradientBrush oLinearGradientBrush =
                App.Current.Resources[applicationThemeName + "PanelGradient"] as System.Windows.Media.LinearGradientBrush;

            App.Current.Resources[Infrastructure.Text.GradientResources] = oLinearGradientBrush;
        }

        public static void Close(this System.Windows.Controls.UserControl userControl)
        {
            System.Windows.Controls.Panel oPanel = userControl.Parent as System.Windows.Controls.Panel;

            oPanel.Children.Remove(userControl);
        }

        public static void Show(this System.Windows.Controls.UserControl userControl)
        {
            Utility.MainWindow.UserControlsPanel.Children.Clear();

            Utility.MainWindow.UserControlsPanel.Children.Add(userControl);
        }

        public static void DoAction(this Stimulsoft.Report.StiReport report, Infrastructure.ReportType action, string fileName = null)
        {
            switch (action)
            {
                case Infrastructure.ReportType.Print:
                    {
                        report.Print();
                        break;
                    }

                case Infrastructure.ReportType.ExportToPDF:
                    {
                        report.ExportToPdf(fileName);
                        break;
                    }

                case Infrastructure.ReportType.SaveAsImage:
                    {
                        report.ExportToImage(fileName);
                        break;
                    }

                case Infrastructure.ReportType.Show:
                    {
                        report.ShowWithWpfRibbonGUI();
                        break;
                    }

                default:
                    break;
            }
        }
        #endregion /Methods

    }
}
