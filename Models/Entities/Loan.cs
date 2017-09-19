namespace Models
{
    public class Loan : BaseEntity
    {

        #region Configuration

        internal class Configuration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Loan>
        {
            public Configuration() : base()
            {
                ToTable("Loans");

                HasRequired(current => current.Member)
                    .WithMany(member => member.Loans)
                    .HasForeignKey(current => current.MemberId)
                    .WillCascadeOnDelete(true);
            }
        }

        #endregion /Configuration

        #region Constructor

        public Loan() : base()
        {
            StartDate = System.DateTime.Today;

            IsActive = true;

            IsPayed = false;
        }

        #endregion /Constructor

        #region Properties

        public System.DateTime StartDate { get; set; }

        public System.DateTime EndDate { get; set; }

        public int InstallmentsCount { get; set; }

        public bool IsActive { get; set; }

        public bool IsPayed { get; set; }

        public long LoanAmount { get; set; }

        public long RefundAmount { get; set; }

        public string Description { get; set; }

        public System.Guid MemberId { get; set; }

        public virtual Member Member { get; set; }

        public virtual System.Collections.Generic.IList<Installment> Installments { get; set; }

        #endregion /Properties        

        #region Methods

        #endregion /Methods

    }
}
