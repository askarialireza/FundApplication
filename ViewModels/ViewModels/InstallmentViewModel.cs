
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

                IsActive = !(_isPayed);
            }
        }

        private System.DateTime _installDate;
        public System.DateTime InstallmentDate
        {
            get
            {
                return _installDate;
            }
            set
            {
                _installDate = value;

                PersianInstallmentDate =
                    new FarsiLibrary.Utils.PersianDate(_installDate).ToString("d");
            }
        }

        private System.DateTime? _paymentDate;
        public System.DateTime? PaymentDate
        {
            get
            {
                return _paymentDate;
            }
            set
            {
                _paymentDate = value;

                if (value != null)
                {
                    PersianPaymentDate =
                        new FarsiLibrary.Utils.PersianDate((System.DateTime)_paymentDate).ToString("d");
                }
                else
                {
                    PersianPaymentDate = string.Empty;
                }
            }
        }

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

                Models.Member oMember = oUnitOfWork.MemberRepository
                    .GetById(_memberId);

                MemberFullName = oMember.FullName.FirstName + " " + oMember.FullName.LastName;
            }
        }

        public string MemberFullName { get; set; }

        public string IsPayedDescription { get; set; }

        public string PersianPaymentDate { get; set; }

        public string PersianInstallmentDate { get; set; }
    }
}
