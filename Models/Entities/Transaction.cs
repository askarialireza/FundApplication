namespace Models
{
    public class Transaction : BaseEntity
    {
        #region Configuration

        internal class Configuration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Models.Transaction>
        {
            public Configuration() : base()
            {
                ToTable("Transactions");

                HasRequired(current => current.Fund)
                    .WithMany(fund => fund.Transactions)
                    .HasForeignKey(current => current.FundId)
                    .WillCascadeOnDelete(true);
            }
        }

        #endregion /Configuration

        public Transaction() : base()
        {

        }

        public System.DateTime Date { get; set; }

        public long Amount { get; set; }

        public long Balance { get; set; }

        public Models.TransactionType TransactionType { get; set; }

        public string Description { get; set; }

        public virtual Models.Fund Fund { get; set; }

        public System.Guid FundId { get; set; }

        public System.Guid MemberId { get; set; }


    }
}