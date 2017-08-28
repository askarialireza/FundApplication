
namespace Models.ComplexTypes
{
    [System.ComponentModel.DataAnnotations.Schema.ComplexType]
    public class GridHeight : object
    {
        public GridHeight() : base()
        {
            DatePicker = 0;
            EventsReminder = 0;
            FundDetails = 0;
        }

        public double DatePicker { get; set; }

        public double EventsReminder { get; set; }

        public double FundDetails { get; set; }
    }
}
