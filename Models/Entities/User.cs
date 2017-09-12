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

        public User() : base()
        {
            FullName = new ComplexTypes.FullName();

            IsAdmin = false;

            CanBeDeleted = true;
        }

        public ComplexTypes.FullName FullName { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.Index(IsUnique = true)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = false)]
        public string Username { get; set; }

        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = false)]
        [System.ComponentModel.DataAnnotations.StringLength(maximumLength: 32, MinimumLength = 32)]
        public string Password { get; set; }

        public bool IsAdmin { get; set; }

        public bool CanBeDeleted { get; set; }

        public System.DateTime RegisterationDate { get; set; }

        public System.DateTime? LastLoginTime { get; set; }

        public virtual System.Collections.Generic.IList<Fund> Funds { get; set; }

        public virtual Models.UserSetting UserSetting { get; set; }

    }
}
