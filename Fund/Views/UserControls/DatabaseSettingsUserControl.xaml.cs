using System.Linq;

namespace Fund
{
    public partial class DatabaseSettingsUserControl : System.Windows.Controls.UserControl
    {
        public DatabaseSettingsUserControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            BackupPathTextBox.Text = Utility.DatabaseBackupPath;
            LoadGridControl();
        }

        private void LoadGridControl()
        {
            string _defaultPath = Utility.DatabaseBackupPath;

            System.IO.DirectoryInfo oDirectoryInfo =
                new System.IO.DirectoryInfo(path: _defaultPath);

            System.Collections.Generic.List<ViewModels.DatabaseBackupViewModel> Backups =
                new System.Collections.Generic.List<ViewModels.DatabaseBackupViewModel>();

            foreach (System.IO.FileInfo oFileInfo in oDirectoryInfo.GetFiles())
            {
                if ((string.Compare(oFileInfo.Extension, ".bkdb", true) == 0) == true)
                {
                    ViewModels.DatabaseBackupViewModel oViewModel = new ViewModels.DatabaseBackupViewModel();
                    oViewModel.BackupFileName = oFileInfo.Name;
                    oViewModel.BackupDateTime = oFileInfo.CreationTime;
                    oViewModel.PersianBackupDateTime =
                        FarsiLibrary.Utils.PersianDateConverter.ToPersianDate(oViewModel.BackupDateTime).ToString();

                    Backups.Add(oViewModel);
                }
            }

            BackupsGridControl.ItemsSource = Backups
                .OrderBy(current => current.BackupDateTime)
                .ToList();

            if (Backups.Count == 0)
            {
                DeleteAllBackupButton.IsEnabled = false;
                DeleteBackupButton.IsEnabled = false;
                RestoreBackupButton.IsEnabled = false;
            }
            else
            {
                DeleteAllBackupButton.IsEnabled = true;
                DeleteBackupButton.IsEnabled = true;
                RestoreBackupButton.IsEnabled = true;
            }
        }

        private void EditPathButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog oFolderBrowserDialog =
                new System.Windows.Forms.FolderBrowserDialog();

            string firstPath = Utility.DatabaseBackupPath;

            if (oFolderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                BackupPathTextBox.Text = oFolderBrowserDialog.SelectedPath;
                Utility.DatabaseBackupPath = oFolderBrowserDialog.SelectedPath;

                DAL.UnitOfWork oUnitOfWork = null;

                try
                {
                    oUnitOfWork = new DAL.UnitOfWork();

                    var varUsers = oUnitOfWork.UserRepository
                         .Get()
                         .Where(current => current.UserSetting.CanChangeDatabaseBackupPath == true)
                         .ToList();

                    foreach (var item in varUsers)
                    {
                        item.UserSetting.DatabaseBackupPath = Utility.DatabaseBackupPath;
                        oUnitOfWork.UserRepository.Update(item);
                    }

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

                Utility.MoveBackupFiles(firstPath, Utility.DatabaseBackupPath);
            }

            LoadGridControl();
        }

        private void MakeBackupButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string sourcePath = System.Environment.CurrentDirectory + "\\Files\\Database\\FundDatabase.sdf";

            string date = FarsiLibrary.Utils.PersianDate.Now.ToString("d").Replace("/", string.Empty);
            string hour = System.DateTime.Now.Hour.ToString("00");
            string minute = System.DateTime.Now.Minute.ToString("00"); ;
            string second = System.DateTime.Now.Second.ToString("00"); ;

            string backupFileName = string.Format("dbBackup-{0}-{1}-{2}-{3}.bkdb", date, hour, minute, second);

            string destinationPath = Utility.DatabaseBackupPath + "\\" + backupFileName;

            System.IO.File.Copy(sourcePath, destinationPath);

            DevExpress.Xpf.Core.DXMessageBox.Show
                (
                    messageBoxText: "تهیه نسخه پشتیبان با موفقیت انجام گردید",
                    caption: Infrastructure.MessageBoxCaption.Information,
                    button: System.Windows.MessageBoxButton.OK,
                    icon: System.Windows.MessageBoxImage.Information,
                    defaultResult: System.Windows.MessageBoxResult.OK,
                    options: System.Windows.MessageBoxOptions.RightAlign | System.Windows.MessageBoxOptions.RtlReading
                );

            LoadGridControl();
        }

        private void ResetToDefaultButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string currentPath = BackupPathTextBox.Text;

            BackupPathTextBox.Text = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + @"\Fund\Backups\";
            Utility.DatabaseBackupPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + @"\Fund\Backups\";

            DAL.UnitOfWork oUnitOfWork = null;

            try
            {
                oUnitOfWork = new DAL.UnitOfWork();

                var varUsers = oUnitOfWork.UserRepository
                     .Get()
                     .Where(current => current.UserSetting.CanChangeDatabaseBackupPath == true)
                     .ToList();

                foreach (var item in varUsers)
                {
                    item.UserSetting.DatabaseBackupPath = Utility.DatabaseBackupPath;
                    oUnitOfWork.UserRepository.Update(item);
                }

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

            Utility.MoveBackupFiles(currentPath, Utility.DatabaseBackupPath);
        }

        private void DeleteBackupButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ViewModels.DatabaseBackupViewModel selectedBackup = BackupsGridControl.SelectedItem as ViewModels.DatabaseBackupViewModel;

            System.IO.DirectoryInfo oDirectoryInfo = new System.IO.DirectoryInfo(Utility.DatabaseBackupPath);

            System.IO.FileInfo oFile = oDirectoryInfo.GetFiles()
                 .Where(current => current.Name == selectedBackup.BackupFileName)
                 .Where(current => current.CreationTime == selectedBackup.BackupDateTime)
                 .FirstOrDefault();

            if (oFile != null)
            {

                System.Windows.MessageBoxResult oResult =
                     DevExpress.Xpf.Core.DXMessageBox.Show
                     (
                         messageBoxText: "آیا مطمئن به حذف نسخه پشتیبان هستید؟",
                         caption: Infrastructure.MessageBoxCaption.Question,
                         button: System.Windows.MessageBoxButton.YesNo,
                         icon: System.Windows.MessageBoxImage.Question,
                         defaultResult: System.Windows.MessageBoxResult.No,
                         options: System.Windows.MessageBoxOptions.RightAlign | System.Windows.MessageBoxOptions.RtlReading
                     );

                if (oResult == System.Windows.MessageBoxResult.Yes)
                {
                    oFile.Delete();

                    DevExpress.Xpf.Core.DXMessageBox.Show
                    (
                        messageBoxText: "نسخه پشتیبان با موفقیت حذف گردید",
                        caption: Infrastructure.MessageBoxCaption.Information,
                        button: System.Windows.MessageBoxButton.OK,
                        icon: System.Windows.MessageBoxImage.Information,
                        defaultResult: System.Windows.MessageBoxResult.OK,
                        options: System.Windows.MessageBoxOptions.RightAlign | System.Windows.MessageBoxOptions.RtlReading
                    );
                }

                LoadGridControl();
            }
        }

        private void RestoreBackupButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ViewModels.DatabaseBackupViewModel selectedBackup = BackupsGridControl.SelectedItem as ViewModels.DatabaseBackupViewModel;

            System.IO.DirectoryInfo oDirectoryInfo = new System.IO.DirectoryInfo(Utility.DatabaseBackupPath);

            System.IO.FileInfo oFile = oDirectoryInfo.GetFiles()
                 .Where(current => current.Name == selectedBackup.BackupFileName)
                 .Where(current => current.CreationTime == selectedBackup.BackupDateTime)
                 .FirstOrDefault();

            if (oFile != null)
            {
                System.Windows.MessageBoxResult oResult =
                     DevExpress.Xpf.Core.DXMessageBox.Show
                     (
                         messageBoxText: "آیا مطمئن به بازیابی نسخه پشتیبان هستید؟",
                         caption: Infrastructure.MessageBoxCaption.Question,
                         button: System.Windows.MessageBoxButton.YesNo,
                         icon: System.Windows.MessageBoxImage.Question,
                         defaultResult: System.Windows.MessageBoxResult.No,
                         options: System.Windows.MessageBoxOptions.RightAlign | System.Windows.MessageBoxOptions.RtlReading
                     );

                if (oResult == System.Windows.MessageBoxResult.Yes)
                {
                    string destinationFileName = System.Environment.CurrentDirectory + "\\Files\\Database\\FundDatabase.sdf";
                    oFile.CopyTo(destFileName: destinationFileName, overwrite: true);

                    DevExpress.Xpf.Core.DXMessageBox.Show
                    (
                        messageBoxText: "نسخه پشتیبان با موفقیت بازیابی گردید" + System.Environment.NewLine + "برنامه با پایگاه جدید بازیابی شده مجددا راه اندازی خواهد شد",
                        caption: Infrastructure.MessageBoxCaption.Information,
                        button: System.Windows.MessageBoxButton.OK,
                        icon: System.Windows.MessageBoxImage.Information,
                        defaultResult: System.Windows.MessageBoxResult.OK,
                        options: System.Windows.MessageBoxOptions.RightAlign | System.Windows.MessageBoxOptions.RtlReading
                    );

                    System.Diagnostics.Process.Start(System.Windows.Application.ResourceAssembly.Location);
                    System.Windows.Application.Current.Shutdown();
                }
            }
        }

        private void DeleteAllBackupButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            System.IO.DirectoryInfo oDirectoryInfo = new System.IO.DirectoryInfo(Utility.DatabaseBackupPath);

            System.Windows.MessageBoxResult oResult =
                 DevExpress.Xpf.Core.DXMessageBox.Show
                 (
                     messageBoxText: "آیا مطمئن به حذف تمامی نسخه‌های پشتیبان هستید؟",
                     caption: Infrastructure.MessageBoxCaption.Question,
                     button: System.Windows.MessageBoxButton.YesNo,
                     icon: System.Windows.MessageBoxImage.Question,
                     defaultResult: System.Windows.MessageBoxResult.No,
                     options: System.Windows.MessageBoxOptions.RightAlign | System.Windows.MessageBoxOptions.RtlReading
                 );

            if (oResult == System.Windows.MessageBoxResult.Yes)
            {
                foreach (System.IO.FileInfo oFileInfo in oDirectoryInfo.GetFiles())
                {
                    oFileInfo.Delete();
                }

                DevExpress.Xpf.Core.DXMessageBox.Show
                (
                    messageBoxText: "تمامی نسخه‌های پشتیبان با موفقیت حذف گردیدند",
                    caption: Infrastructure.MessageBoxCaption.Information,
                    button: System.Windows.MessageBoxButton.OK,
                    icon: System.Windows.MessageBoxImage.Information,
                    defaultResult: System.Windows.MessageBoxResult.OK,
                    options: System.Windows.MessageBoxOptions.RightAlign | System.Windows.MessageBoxOptions.RtlReading
                );
            }

            LoadGridControl();
        }

        private void CloseButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            System.Windows.Controls.Panel oPanel = this.Parent as System.Windows.Controls.Panel;
            oPanel.Children.Remove(this);
        }
    }
}
