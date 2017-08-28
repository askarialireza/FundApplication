
namespace ViewModels
{
    public class MembersManagementViewModel
    {
        public MembersManagementViewModel()
        {
        }

        public System.Guid Id { get; set; }
        public Models.ComplexTypes.FullName FullName { get; set; }
        public string FatherName { get; set; }
        public string NationalCode { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string PersianMembershipDateTime { get; set; }
        public string GenderToString { get; set; }

    }
}
