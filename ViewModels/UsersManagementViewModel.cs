
namespace ViewModels
{
    public class UsersManagementViewModel
    {
        public UsersManagementViewModel() : base()
        {
        }

        public System.Guid Id { get; set; }
        public string Username { get; set; }
        public Models.ComplexTypes.FullName FullName { get; set; }
        public string PersianRegisterationDate { get; set; }
        public string IsAdminToString { get; set; }
        public string PersianLastLoginTime { get; set; }
    }
}
