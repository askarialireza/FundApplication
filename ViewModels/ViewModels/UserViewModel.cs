
namespace ViewModels
{
    public class UserViewModel : object
    {
        public UserViewModel() : base()
        {
        }

        private bool _isAdmin;

        private System.DateTime _registerationDate;

        private System.DateTime? _lastLoginTime;

        public System.Guid Id { get; set; }

        public Models.ComplexTypes.FullName FullName { get; set; }

        public string Username { get; set; }

        public string PersianRegisterationDate { get; set; }

        public string PersianLastLoginTime { get; set; }

        public string IsAdminDescription { get; set; }

        public bool IsAdmin
        {
            get
            {
                return _isAdmin;
            }
            set
            {
                _isAdmin = value;

                IsAdminDescription = (_isAdmin == true) ? "کاربر مدیر" : "کاربر عادی";
            }
        }

        public System.DateTime RegisterationDate
        {
            get
            {
                return _registerationDate;
            }
            set
            {
                _registerationDate = value;

                PersianRegisterationDate = new FarsiLibrary.Utils.PersianDate(_registerationDate).ToString("d");
            }
        }

        public System.DateTime? LastLoginTime
        {
            get
            {
                return _lastLoginTime;
            }
            set
            {
                _lastLoginTime = value;

                if (_lastLoginTime != null)
                {
                    PersianLastLoginTime = new FarsiLibrary.Utils.PersianDate((System.DateTime)_lastLoginTime).ToString();
                }
                else
                {
                    PersianLastLoginTime = string.Empty;
                }

            }
        }

    }
}
