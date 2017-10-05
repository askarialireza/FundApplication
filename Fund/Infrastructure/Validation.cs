
namespace Infrastructure
{
    public static class Validation
    {
        static Validation()
        {
        }

        public static void EmailAddressValidation(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            System.Text.RegularExpressions.Regex oRegex =
                new System.Text.RegularExpressions.Regex(Infrastructure.Text.RegularExpressions.EmailAddress);

            if (oRegex.IsMatch(((System.Windows.Controls.TextBox)sender).Text.Trim()) == false)
            {

                Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: "ایمیل نامعتبر" + System.Environment.NewLine + "شکل قابل قبول : x@y.z");

                e.Handled = true;
            }
        }

        public static void MobileNumberValidation(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            System.Text.RegularExpressions.Regex oRegex =
                new System.Text.RegularExpressions.Regex(Dtx.Text.RegularExpressions.Patterns.NationalCode);

            if (oRegex.IsMatch(((System.Windows.Controls.TextBox)sender).Text.Trim()) == false)
            {

                Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: "شماره موبایل درج شده نامعتبر می‌باشد. " + System.Environment.NewLine + "نمونه شماره موبایل قابل قبول : 09123456789");

                e.Handled = true;
            }
        }

        public static void UsernameValidation(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            System.Text.RegularExpressions.Regex oRegex =
                new System.Text.RegularExpressions.Regex(Infrastructure.Text.RegularExpressions.Username);

            if (oRegex.IsMatch(((System.Windows.Controls.TextBox)sender).Text.Trim()) == false)
            {

                Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: "نام کاربری فقط می‌تواند شامل حروف لاتین، اعداد و _ باشد.");

                e.Handled = true;
            }
        }

        public static void PercentValueValidation(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            System.Text.RegularExpressions.Regex oRegex =
                new System.Text.RegularExpressions.Regex(Infrastructure.Text.RegularExpressions.PercentValue);

            if (oRegex.IsMatch(((System.Windows.Controls.TextBox)sender).Text.Trim()) == false)
            {
                Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: "مقدار درصد باید عددی بین 0 تا 100 باشد.");

                e.Handled = true;
            }
        }

        public static void PersianDateValidation(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {

        }

        public static void NationalCodeValidation(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            System.Text.RegularExpressions.Regex oRegex =
                new System.Text.RegularExpressions.Regex(Dtx.Text.RegularExpressions.Patterns.NationalCode);

            if (oRegex.IsMatch(((System.Windows.Controls.TextBox)sender).Text.Trim()) == false)
            {

                Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: "کد ملی بایست عددی 10 رقمی باشد.");

                e.Handled = true;
            }
        }
    }
}
