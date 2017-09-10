
namespace ViewModels
{
    public class LoanViewModel
    {
        public LoanViewModel() : base()
        {
        }

        public System.Guid Id { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int InstallmentsCount { get; set; }
        public bool IsPayed { get; set; }

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
