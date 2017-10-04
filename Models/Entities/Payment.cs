namespace Models
{
    public class Payment : BaseEntity
    {

        #region Configuration
        internal class Configuration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Payment>
        {
            public Configuration() : base()
            {
                ToTable("Payments");

                HasRequired(current => current.Loan)
                    .WithMany(loan => loan.Payments)
                    .HasForeignKey(current => current.LoanId)
                    .WillCascadeOnDelete(true);

                Property(current => current.Description)
                    .HasColumnType("ntext")
                    .IsMaxLength();

            }
        }
        #endregion /Configuration

        #region Constructor

        public Payment() : base()
        {
            IsPayed = false;
        }

        #endregion /Constructor

        #region Properties
        public System.DateTime InstallmentDate { get; set; }
        public System.DateTime PaymentDate { get; set; }
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
