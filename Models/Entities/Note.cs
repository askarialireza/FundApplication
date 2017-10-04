
namespace Models
{
    public class Note : BaseEntity
    {
        #region Configuration

        internal class Configuration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Note>
        {
            public Configuration() : base()
            {
                ToTable("Notes");

                Property(current => current.Text)
                    .HasColumnType("ntext")
                    .IsMaxLength();

                //HasRequired(current => current.Fund)
                //    .WithMany(fund => fund.Reminders)
                //    .HasForeignKey(current => current.FundId)
                //    .WillCascadeOnDelete(true);
            }
        }

        #endregion /Configuration

        public Note() : base()
        {
               
        }

        public System.DateTime DateTime { get; set; }
        public string PersianDateTime { get; set; }
        public string Subject { get; set; }
        public string Text { get; set; }

    }
}
