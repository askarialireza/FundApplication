
namespace ViewModels
{
    public class DebtorViewModel : object
    {
        public DebtorViewModel() : base()
        {

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

                if (oMember != null)
                {
                    MemberPhoto = oMember.Picture;

                    Balance = oMember.Balance;
                }
            }
        }

        public long Balance { get; set; }

        public byte[] MemberPhoto { get; set; }

        public Models.ComplexTypes.FullName FullName { get; set; }

        public long DebtAmount { get; set; }

        public System.DateTime? NextPaymentDate { get; set; }

        public int InstallmentCount { get; set; }
    }
}
