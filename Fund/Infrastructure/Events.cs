using Fund;

namespace Infrastructure
{
    public static class Events
    {
        static Events()
        {
        }

        public static void MoneyTextBoxesLostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(((System.Windows.Controls.TextBox)sender).Text) == false)
            {
                System.Text.RegularExpressions.Regex oRegex =
                    new System.Text.RegularExpressions.Regex(Infrastructure.Text.RegularExpressions.NumbersOnly);

                if (oRegex.IsMatch(((System.Windows.Controls.TextBox)sender).Text) == false)
                {
                    Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: "فقط درج اعداد قابل قبول می‌باشد.");
                }


                long value = System.Convert.ToInt64(((System.Windows.Controls.TextBox)sender).Text.Replace(" ریال", string.Empty).Replace(",", string.Empty));

                ((System.Windows.Controls.TextBox)sender).Text = value.ToRialStringFormat();
            }
            else
            {
                long zero = 0;
                ((System.Windows.Controls.TextBox)sender).Text = zero.ToRialStringFormat();
            }
        }
    }
}
