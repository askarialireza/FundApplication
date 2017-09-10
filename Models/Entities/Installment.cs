namespace Models
{
    public class Installment : BaseEntity
    {

        #region Configuration
        internal class Configuration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Installment>
        {
            public Configuration() : base()
            {
                ToTable("Installments");

                HasRequired(current => current.Loan)
                    .WithMany(loan => loan.Installments)
                    .HasForeignKey(current => current.LoanId)
                    .WillCascadeOnDelete(true);

                Property(current => current.Description)
                    .HasColumnType("ntext")
                    .IsMaxLength();

            }
        }
        #endregion /Configuration

        #region Constructor

        public Installment() : base()
        {
            IsPayed = false;
        }

        #endregion /Constructor

        #region Properties
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

        public long PaymentAmount { get; set; }

        public string PersianPaymentDate { get; set; }

        public string PersianInstallmentDate { get; set; }

        public bool IsPayed { get; set; }

        public string Description { get; set; }

        public System.Guid LoanId { get; set; }

        public virtual Loan Loan { get; set; }

        #endregion /Properties

        #region Methods

        #endregion /Methods

    }
}
