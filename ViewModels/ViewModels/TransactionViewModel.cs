
namespace ViewModels
{
    public class TransactionViewModel : object
    {
        public TransactionViewModel() : base()
        {
        }

        public System.Guid Id { get; set; }

        private System.DateTime _date;
        public System.DateTime Date
        {
            get
            {
                return _date;
            }
            set
            {
                _date = value;

                PersianDate =
                    new FarsiLibrary.Utils.PersianDate(_date).ToString("d");
            }
        }
        public string PersianDate { get; set; }

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

                switch (TransactionType)
                {
                    case Models.TransactionType.Loan:
                        AmountRialFormat = string.Format("{0} - ریال" ,_amount.ToString("#,##0"));
                        break;
                    case Models.TransactionType.Installment:
                        AmountRialFormat = string.Format("{0} + ریال", _amount.ToString("#,##0"));
                        break;
                    case Models.TransactionType.Deposit:
                        AmountRialFormat = string.Format("{0} + ریال", _amount.ToString("#,##0"));
                        break;
                    default:
                        break;
                }
            }
        }

        public string AmountRialFormat { get; set; }

        private long _balance;
        public long Balance
        {
            get
            {
                return _balance;
            }
            set
            {
                _balance = value;

                BalanceRialFormat = _balance.ToString("#,##0 ریال");
            }
        }


        public string BalanceRialFormat { get; set; }

        private Models.TransactionType _transaction;
        public Models.TransactionType TransactionType
        {
            get
            {
                return _transaction;
            }
            set
            {
                _transaction = value;

                switch (_transaction)
                {
                    case Models.TransactionType.Loan:
                        TransactionDescription = "پرداخت وام به اعضا";
                        break;
                    case Models.TransactionType.Installment:
                        TransactionDescription = "پرداخت قسط از طرف اعضا";
                        break;
                    case Models.TransactionType.Deposit:
                        TransactionDescription = "افزایش موجودی صندوق";
                        break;
                    default:
                        break;
                }
            }
        }

        public string TransactionDescription { get; set; }

        public string Description { get; set; }

        public System.Guid FundId { get; set; }

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

                MemberFullName = oUnitOfWork.MemberRepository
                    .GetById(_memberId).FullName;

                oUnitOfWork.Dispose();
            }
        }

        public Models.ComplexTypes.FullName MemberFullName { get; set; }

    }
}
