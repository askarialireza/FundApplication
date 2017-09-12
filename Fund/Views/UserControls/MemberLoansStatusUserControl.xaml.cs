using System.Linq;

namespace Fund
{
    public partial class MemberLoansStatusUserControl : System.Windows.Controls.UserControl
    {
        public MemberLoansStatusUserControl()
        {
            InitializeComponent();
        }

        private void CancelButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Close();
        } 

        private void MembersListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void SearchMemberTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
    }
}
