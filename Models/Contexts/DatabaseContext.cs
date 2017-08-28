using System.Data.Entity;

namespace Models
{
    public class DatabaseContext : System.Data.Entity.DbContext
    {

        #region Constructors

        static DatabaseContext()
        {
            //System.Data.Entity.Database
            //    .SetInitializer(new DatabaseContextInitializer());
            System.Data.Entity.Database
                .SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<DatabaseContext>());
        }
        public DatabaseContext() : base()
        {

        }

        #endregion /Constructors

        #region Properties

        public System.Data.Entity.DbSet<User> Users { get; set; }
        public System.Data.Entity.DbSet<Fund> Funds { get; set; }
        public System.Data.Entity.DbSet<Loan> Loans { get; set; }
        public System.Data.Entity.DbSet<Member> Members { get; set; }
        public System.Data.Entity.DbSet<Reminder> Reminders { get; set; }
        public System.Data.Entity.DbSet<Payment> Payments { get; set; }

        public System.Data.Entity.DbSet<UserSetting> UserSettings { get; set; }

        #endregion /Properties

        #region Methods

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new User.Configuration());
            modelBuilder.Configurations.Add(new Fund.Configuration());
            modelBuilder.Configurations.Add(new Loan.Configuration());
            modelBuilder.Configurations.Add(new Member.Configuration());
            modelBuilder.Configurations.Add(new Reminder.Configuration());
            modelBuilder.Configurations.Add(new Payment.Configuration());
            modelBuilder.Configurations.Add(new UserSetting.Configuration());
        }

        #endregion /Methods
    }
}
