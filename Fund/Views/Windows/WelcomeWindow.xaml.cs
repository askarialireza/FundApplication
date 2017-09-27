
namespace Fund
{
    public partial class WelcomeWindow : System.Windows.Window
    {
        public WelcomeWindow()
        {
            InitializeComponent();
        }

        private void Window_ContentRendered(object sender, System.EventArgs e)
        {
            System.ComponentModel.BackgroundWorker oBackgroundWorker = new System.ComponentModel.BackgroundWorker();

            oBackgroundWorker.WorkerReportsProgress = true;

            oBackgroundWorker.DoWork += OBackgroundWorker_DoWork;

            oBackgroundWorker.ProgressChanged += OBackgroundWorker_ProgressChanged;

            oBackgroundWorker.RunWorkerCompleted += OBackgroundWorker_RunWorkerCompleted;

            oBackgroundWorker.RunWorkerAsync();
        }

        private void OBackgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            MainRibbonWindow oMainRibbonWindow = new MainRibbonWindow();

            System.Threading.Thread oThread = new System.Threading.Thread(new System.Threading.ThreadStart(delegate ()
            {
                System.Windows.Threading.DispatcherOperation oDispatcherOperation =
                    oMainRibbonWindow.Dispatcher.BeginInvoke( System.Windows.Threading.DispatcherPriority.Normal,
                        new System.Action(delegate ()
                        {
                            DAL.UnitOfWork oUnitOfWork = new DAL.UnitOfWork();

                            Utility.CurrentUser = oUnitOfWork.UserRepository
                                .GetById(Utility.CurrentUser.Id);

                            oMainRibbonWindow.Show();

                            oMainRibbonWindow.WindowState = System.Windows.WindowState.Maximized;

                            this.Hide();
                        }));
            }));

            oThread.SetApartmentState(System.Threading.ApartmentState.STA);

            oThread.Start();
        }

        private void OBackgroundWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        private void OBackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            for (int i = 0; i < 100; i++)
            {
                (sender as System.ComponentModel.BackgroundWorker).ReportProgress(i);

                System.Threading.Thread.Sleep(25);
            }
        }

    }
}
