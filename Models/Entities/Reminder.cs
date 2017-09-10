
namespace Models
{
    public class Reminder : BaseEntity
    {
        #region Configuration

        internal class Configuration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Reminder>
        {
            public Configuration() : base()
            {
                ToTable("Reminders");

                Property(current => current.Description)
                    .HasColumnType("ntext")
                    .IsMaxLength();

                HasRequired(current => current.Fund)
                    .WithMany(fund => fund.Reminders)
                    .HasForeignKey(current => current.FundId)
                    .WillCascadeOnDelete(true);
            }
        }

        #endregion /Configuration
        public Reminder()
        {

        }
        public System.DateTime DateTime { get; set; }
        public string PersianDateTime { get; set; }
        public Models.Event EventType { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public string Description { get; set; }
        public System.Guid FundId { get; set; }
        public virtual Fund Fund { get; set; }

    }
}
