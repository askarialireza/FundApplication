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

        public Installment() : base()
        {
            IsPayed = false;
        }

        public System.DateTime InstallmentDate { get; set; }

        public System.DateTime? PaymentDate { get; set; }

        public long PaymentAmount { get; set; }

        public bool IsPayed { get; set; }

        public string Description { get; set; }

        public System.Guid LoanId { get; set; }

        public virtual Loan Loan { get; set; }

    }
}
