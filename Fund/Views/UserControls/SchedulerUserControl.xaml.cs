
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
            (this.Parent as System.Windows.Controls.Panel).Children.Clear();
        }

        private void UserControlLoaded(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void PersianCalendar_DayEventsChanged(object sender, System.EventArgs e)
        {

        }
    }
}