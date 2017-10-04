
namespace Fund
{
    public partial class SchedulerUserControl : System.Windows.Controls.UserControl
    {
        public SchedulerUserControl()
        {
            InitializeComponent();
        }

        private void CloseClick(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Close();
        }
    }
}