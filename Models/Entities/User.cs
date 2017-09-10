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

            IsAdmin = false;

            CanBeDeleted = true;
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
        private bool _isAdmin;
        public bool IsAdmin
        {
            get
            {
                return _isAdmin;
            }
            set
            {
                _isAdmin = value;

                IsAdminPropertyChanged(_isAdmin);
            }
        }

        //********
        //********

        public bool CanBeDeleted { get; set; }

        //********
        //********
        private System.DateTime _registerationDate;
        public System.DateTime RegisterationDate
        {
            get
            {
                return _registerationDate;
            }
            set
            {
                _registerationDate = System.DateTime.Now;

                RegisterationDatePropertyChanged(_registerationDate);
            }
        }

        //********
        //********
        private System.DateTime _lastLoginTime;
        public System.DateTime LastLoginTime
        {
            get
            {
                return _lastLoginTime;
            }
            set
            {
                _lastLoginTime = value;

                LastLoginTimePropertyChanged(_lastLoginTime);
            }
        }

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

        private void IsAdminPropertyChanged(bool isAdmin)
        {
            IsAdminToString = (isAdmin == true) ? "کاربر مدیر" : "کاربر عادی";
        }

        private void RegisterationDatePropertyChanged(System.DateTime dateTime)
        {
            PersianRegisterationDate = FarsiLibrary.Utils.PersianDateConverter
               .ToPersianDate(dateTime).ToString("d");
        }

        private void LastLoginTimePropertyChanged(System.DateTime dateTime)
        {
            PersianLastLoginTime = FarsiLibrary.Utils.PersianDateConverter
               .ToPersianDate(dateTime).ToString();
        }

        #endregion /Methods

    }
}
