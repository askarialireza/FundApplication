
namespace ViewModels
{
    public class InstallmentViewModel
    {
        public System.Guid Id { get; set; }


        private long _amount;
        public long Amount
        {
            get
            {
               return _amount;
            }
            set
            {
                _amount = value;

                AmountRialFormat = _amount.ToString("#,##0 ریال");
            }
        }
        public string AmountRialFormat { get; set; }
        public bool IsActive { get; set; }

        private bool _isPayed;
        public bool IsPayed
        {
            get
            {
                return _isPayed;
            }
            set
            {
                _isPayed = value;

                IsPayedDescription = (_isPayed == true) ? "بله" : "خیر";
            }
        }
        public string IsPayedDescription { get; set; }
        public string InstallmentDate { get; set; }
    }
}
