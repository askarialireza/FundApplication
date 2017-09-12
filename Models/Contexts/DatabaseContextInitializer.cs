
namespace Models
{
    internal class DatabaseContextInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<DatabaseContext>
    {
        #region Constructor

        public DatabaseContextInitializer()
        {

        }

        #endregion /Constructor

        #region Methods

        protected override void Seed(DatabaseContext context)
        {
            
        }

        #endregion /Methods
    }
}
