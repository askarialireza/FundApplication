using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace Fund
{
    /// <summary>
    /// Interaction logic for SchedulerUserControl.xaml
    /// </summary>
    public partial class SchedulerUserControl : UserControl
    {
        public SchedulerUserControl()
        {
            InitializeComponent();
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            (this.Parent as System.Windows.Controls.Panel).Children.Clear();
        }

        private void UserControlLoaded(object sender, RoutedEventArgs e)
        {

        }

        private void PersianCalendar_DayEventsChanged(object sender, System.EventArgs e)
        {

        }
    }
}