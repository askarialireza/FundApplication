
namespace Models.ComplexTypes
{
    [System.ComponentModel.DataAnnotations.Schema.ComplexType]
    public class PersianDate : object
    {
        public PersianDate() : base()
        {
            Year = 1300;
            Month = 1;
            Day = 1;
        }

        public int Year { get; set; }

        public int Month { get; set; }

        public int Day { get; set; }
    }
}
