namespace Models
{
    public class UserSetting : object
    {
        #region Configuration

        internal class Configuration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Models.UserSetting>
        {
            public Configuration() : base()
            {
                ToTable("UserSettings");

                HasRequired(current => current.User)
                    .WithOptional(user => user.UserSetting)
                    .WillCascadeOnDelete(true);
            }
        }

        #endregion /Configuration

        public UserSetting() : base()
        {
            GridWidth = new ComplexTypes.GridWidth();
            GridHeight = new ComplexTypes.GridHeight();
            Theme = new ComplexTypes.Theme();
        }

        [System.ComponentModel.DataAnnotations.Key]
        public System.Guid UserId { get; set; }

        public bool CanChangeDatabaseBackupPath { get; set; }

        public string DatabaseBackupPath { get; set; }

        public Models.ComplexTypes.GridWidth GridWidth { get; set; }

        public Models.ComplexTypes.GridHeight GridHeight { get; set; }

        public Models.ComplexTypes.Theme Theme { get; set; }

        public int PersianCalendarHijriAdjustment { get; set; }

        public virtual Models.User User { get; set; }
    }
}