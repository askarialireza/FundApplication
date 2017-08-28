
namespace Fund
{
    public partial class AboutWindow : System.Windows.Window
    {
        public AboutWindow()
        {
            InitializeComponent();
        }

        private void Label_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            CloseButton.Opacity = 1;
        }

        private void Label_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Label_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            CloseButton.Opacity = 0.5;
        }

        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            this.DragMove();
        }

        private void Window_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
