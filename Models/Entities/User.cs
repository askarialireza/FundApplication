namespace Models
{
    public class User : BaseEntity
    {

        #region Configuration
        internal class Configuration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<User>
        {
            public Configuration() : base()
            {
                ToTable("Users");

            }
        }
        #endregion /Configuration

        #region Constructor

        public User() : base()
        {
            FullName = new ComplexTypes.FullName();

            RegisterationDate = System.DateTime.Now;

            PersianRegisterationDate =  FarsiLibrary.Utils.PersianDateConverter
                .ToPersianDate(System.DateTime.Now).ToString("d");

            IsAdmin = false;

            CanBeDeleted = true;

            IsAdminToString = (IsAdmin == true) ?"کاربر مدیر":"کاربر عادی";
        }

        #endregion /Constructor

        #region Properties
        //********

        public ComplexTypes.FullName FullName { get; set; }

        //********
        //********

        [System.ComponentModel.DataAnnotations.Schema.Index(IsUnique = true)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = false)]
        public string Username { get; set; }

        //********
        //********

        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = false)]
        [System.ComponentModel.DataAnnotations.StringLength(maximumLength: 32, MinimumLength = 32)]
        public string Password { get; set; }

        //********
        //********

        public bool IsAdmin { get; set; }

        //********
        //********

        public bool CanBeDeleted { get; set; }

        //********
        //********
        public System.DateTime RegisterationDate { get; set; }

        //********
        //********

        public System.DateTime LastLoginTime { get; set; }

        //********
        //********

        public string PersianRegisterationDate { get; set; }

        //********
        //********

        public string PersianLastLoginTime { get; set; }

        //********
        //********

        public string IsAdminToString { get; set; }

        //********
        //********

        public virtual System.Collections.Generic.IList<Fund> Funds { get; set; }

        public virtual Models.UserSetting UserSetting { get; set; }

        //********
        //********

        #endregion /Properties

        #region Methods

        #endregion /Methods

    }
}
