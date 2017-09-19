
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

            DateValueTextEdit.Text = SelectedPersianDateTime.ToString("d");

            Text = DateValueTextEdit.Text;

        }

        private void SimpleButton_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            DatePickerPopup.IsOpen = (bool)ToggleButton.IsChecked;
        }

        private void DatePickerPopup_SelectedDateTimeChanged(object sender, System.EventArgs e)
        {
            DateValueTextEdit.Text = DatePickerPopupCalendar.SelectedDateTime.ToString("d");

            ToggleButton.IsChecked = false;

            DatePickerPopup.IsOpen = false;

            Text = DateValueTextEdit.Text;

            SelectedDateTime = FarsiLibrary.Utils.PersianDateConverter.ToGregorianDateTime(DatePickerPopupCalendar.SelectedDateTime);

            SelectedPersianDateTime = DatePickerPopupCalendar.SelectedDateTime;
        }

        private void DateValueTextEdit_Validate(object sender, DevExpress.Xpf.Editors.ValidationEventArgs e)
        {
            if (e.Value != null)
            {
                if (string.IsNullOrWhiteSpace(e.Value.ToString()) == false)
                {
                    FarsiLibrary.Utils.PersianDate oPersianDate = new FarsiLibrary.Utils.PersianDate(e.Value.ToString());

                    DatePickerPopupCalendar.SelectedDateTime = oPersianDate;

                    DatePickerPopupCalendar.GoToDate(oPersianDate);

                    SelectedDateTime = FarsiLibrary.Utils.PersianDateConverter.ToGregorianDateTime(oPersianDate);

                    SelectedPersianDateTime = oPersianDate;
                }
            }
            else
            {
                DatePickerPopupCalendar.GoToDate(SelectedPersianDateTime);

                SelectedDateTime = FarsiLibrary.Utils.PersianDateConverter.ToGregorianDateTime(FarsiLibrary.Utils.PersianDate.Today);

                SelectedPersianDateTime = FarsiLibrary.Utils.PersianDate.Today;
            }

            Text = DateValueTextEdit.Text;

        }

    }
}
