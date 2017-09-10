namespace Models
{
    public class Fund : BaseEntity
    {

        #region Configuration

        internal class Configuration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Fund>
        {
            public Configuration() : base()
            {
                ToTable("Funds");

                HasRequired(current => current.User)
                    .WithMany(user => user.Funds)
                    .HasForeignKey(current => current.UserId)
                    .WillCascadeOnDelete(true);
            }
        }

        #endregion /Configuration

        #region Constructor
        public Fund() : base()
        {
            Balance = 10000000;
            RemovalLimit = 1000000;
        }

        #endregion /Constructor

        #region Properties

        [System.ComponentModel.DataAnnotations.StringLength(maximumLength: 30, MinimumLength = 2)]
        public string Name { get; set; }

        public string ManagerName { get; set; }

        [System.ComponentModel.DataAnnotations.StringLength(maximumLength: 32, MinimumLength = 32)]
        public string ManagerPassword { get; set; }

        [System.ComponentModel.DataAnnotations.Range(1000, 3000)]
        public int FoundationYear { get; set; }

        public long Balance { get; set; }

        public long RemovalLimit { get; set; }

        [System.ComponentModel.DataAnnotations.Range(minimum: 0, maximum: 100)]
        public int Percent { get; set; }

        public System.Guid UserId { get; set; }

        public virtual User User { get; set; }

        public virtual System.Collections.Generic.IList<Member> Members { get; set; }

        public virtual System.Collections.Generic.IList<Reminder> Reminders { get; set; }

        public virtual System.Collections.Generic.IList<Transaction> Transactions { get; set; }

        #endregion /Properties

        #region Methods

        #endregion /Methods

    }
}
