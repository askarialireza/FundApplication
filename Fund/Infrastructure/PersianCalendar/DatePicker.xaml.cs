
namespace Fund
{
    public partial class DatePicker : System.Windows.Controls.UserControl
    {

        public FarsiLibrary.Utils.PersianDate SelectedPersianDateTime { get; set; }

        public System.DateTime SelectedDateTime { get; set; }

        public string Text { get; set; }

        public DatePicker()
        {
            InitializeComponent();

            SelectedPersianDateTime = FarsiLibrary.Utils.PersianDate.Now;

            PersianDateTextBox.Text = SelectedPersianDateTime.ToString("d");

            Text = PersianDateTextBox.Text;

            SelectedDateTime = FarsiLibrary.Utils.PersianDateConverter.ToGregorianDateTime(SelectedPersianDateTime);
        }

        private void DatePickerPopup_SelectedDateTimeChanged(object sender, System.EventArgs e)
        {
            PersianDateTextBox.Text = DatePickerPopupCalendar.SelectedDateTime.ToString("d");

            DatePickerPopup.IsOpen = false;

            Text = PersianDateTextBox.Text;

            SelectedDateTime = FarsiLibrary.Utils.PersianDateConverter.ToGregorianDateTime(DatePickerPopupCalendar.SelectedDateTime);

            SelectedPersianDateTime = DatePickerPopupCalendar.SelectedDateTime;
        }

        private void ToggleButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(PersianDateTextBox.Text.Trim())==true)
            {
                PersianDateTextBox.Focus();

                return;
            }

            System.Text.RegularExpressions.Regex oRegex =
                new System.Text.RegularExpressions.Regex(Infrastructure.Text.RegularExpressions.PersianDate);

            if(oRegex.IsMatch(PersianDateTextBox.Text) == true)
            {
                FarsiLibrary.Utils.PersianDate oPersianDate = new FarsiLibrary.Utils.PersianDate(PersianDateTextBox.Text);

                DatePickerPopupCalendar.SelectedDateTime = oPersianDate;

                DatePickerPopupCalendar.GoToDate(oPersianDate);

                SelectedDateTime = FarsiLibrary.Utils.PersianDateConverter.ToGregorianDateTime(oPersianDate);

                SelectedPersianDateTime = oPersianDate;

                DatePickerPopup.IsOpen = true;
            }
            else
            {
                Infrastructure.MessageBox.Show(caption: Infrastructure.MessageBox.Caption.Error, text: "تاریخ نامعتبر");

                DatePickerPopupCalendar.GoToDate(SelectedPersianDateTime);

                SelectedDateTime = FarsiLibrary.Utils.PersianDateConverter.ToGregorianDateTime(FarsiLibrary.Utils.PersianDate.Today);

                SelectedPersianDateTime = FarsiLibrary.Utils.PersianDate.Today;
            }

            Text = PersianDateTextBox.Text;
        }

    }
}
