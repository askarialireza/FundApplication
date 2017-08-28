
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
            //User oUser = new User();

            //string password = Dtx.Security.Hashing.GetMD5("admin");

            //oUser.Username = "admin";
            //oUser.Password = password;
            //oUser.RegisterationDate = System.DateTime.Now;
            //context.Users.Add(oUser);
            //context.SaveChanges();
            
        }

        #endregion /Methods
    }
}
