namespace Models
{
    public class Manager : BaseEntity
    {

        #region Configuration

        internal class Configuration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Manager>
        {
            public Configuration() : base()
            {
                ToTable("Managers");
            }
        }

        #endregion /Configuration

        #region Constructor

        public Manager():base()
        {
            
        }

        #endregion /Constructor

        #region Properties


        #endregion /Properties        

        #region Methods

        #endregion /Methods

    }
}
