
namespace ViewModels
{
    public class DebtorViewModel : object
    {
        public DebtorViewModel() : base()
        {

        }


        public Models.ComplexTypes.FullName FullName { get; set; }

        private long _debtAmount;
        public long DebtAmount
        {
            get
            {
                return _debtAmount;
            }
            set
            {
                _debtAmount = value;

                DebtAmountRialFormat = _debtAmount.ToString("#,##0 ریال");
            }
        }

        private System.DateTime? _lastPaymentDate;

        public System.DateTime? LastPaymentDate
        {
            get
            {
                return _lastPaymentDate;
            }
            set
            {
                _lastPaymentDate = value;

                if (_lastPaymentDate != null)
                {
                    PersianLastPaymentDate =
                        new FarsiLibrary.Utils.PersianDate((System.DateTime)_lastPaymentDate).ToString("d");
                }
                else
                {
                    PersianLastPaymentDate = string.Empty;
                }
            }
        }

        public string PersianLastPaymentDate { get; set; }

        public string DebtAmountRialFormat { get; set; }
    }
}
