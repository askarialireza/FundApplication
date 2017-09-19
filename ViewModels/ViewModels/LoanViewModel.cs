
namespace ViewModels
{
    public class LoanViewModel
    {
        public LoanViewModel() : base()
        {
        }

        public System.Guid Id { get; set; }

        private System.DateTime _startDate;
        public System.DateTime StartDate
        {
            get
            {
                return _startDate;
            }
            set
            {
                _startDate = value;

                PersianStartDate = new FarsiLibrary.Utils.PersianDate(_startDate).ToString("d");
            }
        }

        private System.DateTime _endDate;
        public System.DateTime EndDate
        {
            get
            {
                return _endDate;
            }
            set
            {
                _endDate = value;

               PersianEndDate = new FarsiLibrary.Utils.PersianDate(_endDate).ToString("d");
            }
        }

        public string PersianStartDate { get; set; }

        public string PersianEndDate { get; set; }

        public int InstallmentsCount { get; set; }


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

                IsPayedDescrption = (_isPayed == true) ? "بله" : "خیر";

                IsActive = !(_isPayed);
            }
        }

        public bool IsActive { get; set; }
        public string IsPayedDescrption { get; set; }

        private long _loanAmount;
        public long LoanAmount
        {
            get
            {
                return _loanAmount;
            }
            set
            {
                _loanAmount = value;

                LoanAmountRialFormat = _loanAmount.ToString("#,##0 ریال");
            }
        }

        public string LoanAmountRialFormat { get; set; }

        private long _refundAmount;
        public long RefundAmount
        {
            get
            {
                return _refundAmount;
            }
            set
            {
                _refundAmount = value;

                RefundAmountRialFormat = _refundAmount.ToString("#,##0 ریال");
            }
        }

        public string RefundAmountRialFormat { get; set; }

        private System.Guid _memberId;
        public System.Guid MemberId
        {
            get
            {
                return _memberId;
            }
            set
            {
                _memberId = value;

                DAL.UnitOfWork oUnitOfWork = new DAL.UnitOfWork();

                FullName = oUnitOfWork.MemberRepository
                    .GetById(_memberId).FullName;

                oUnitOfWork.Dispose();
            }
        }

        public Models.ComplexTypes.FullName FullName { get; set; }

        public string Description { get; set; }
    }
}
